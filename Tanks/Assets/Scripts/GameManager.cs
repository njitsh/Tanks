using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Linked tanks, crosshairs and health bars
    public GameObject[] tanks;
    public GameObject[] crosshairs;
    public GameObject[] health_bars;

    // Tilemaps
    public Tilemap tilemapGround;
    public Tilemap tilemapWall;
    public Tilemap tilemapObjects;
    public Tilemap tilemapTop;
    
    // Tile to prefab array
    public MapSystem.tile_to_prefab[] tile_prefab_array;

    // Full tiles array
    public TileBase[] ground_tiles_array;
    public TileBase[] wall_tiles_array;
    public TileBase[] objects_tiles_array;
    public TileBase[] top_tiles_array;

    // Amount of players playing
    public int playingPlayers = 0;

    PlayerController player;

    public GameObject canvasUI;

    int totalAlive = 0;

    int currentRound = 1;
    public int amountOfRounds = 3;

    GameObject[] players = new GameObject[4];
    public GameObject[] allplayers = new GameObject[4];

    public int[,] player_info = new int[4, 3];

    float cameraSizeMax = 14f;
    float cameraSizeMin = 5f;
    float cameraSize = 14f;

    float xMin;
    float xMax;
    float yMin;
    float yMax;

    float borderSize = 1f;

    GameObject[] PlayerSpawnArray;
    bool[] player_spawned;

    public static bool countDownDone = false;

    public GameObject countdownUI;

    // Start is called before the first frame update
    void Start()
    {
        countdownUI.SetActive(true);

        SaveSystem.Init();

        // Get player controller bindings
        GameObject PCBinding = GameObject.Find("PCBinding");
        ControllerPlayerBinding cpBinding = PCBinding.GetComponent<ControllerPlayerBinding>();

        //player_info = cpBinding.getPlayerInfo();

        /* Possibly use constructor (maybe not a good idea) http://ilkinulas.github.io/development/unity/2016/05/30/monobehaviour-constructor.html
        for (int i = 0; i < 0; i++)
        {
            if (player_info[i, 0] != 0)
            {
                player = new PlayerController(player_info[i, 0], player_info[i, 1], player_info[i, 2]);
            }
        }*/

        if (cpBinding.getLevelPath() != null) MapSystem.Play_Selected_Map(tilemapGround, tilemapWall, tilemapObjects, tilemapTop, cpBinding.getLevelPath(), tile_prefab_array, ground_tiles_array, wall_tiles_array, objects_tiles_array, top_tiles_array);
        else SceneManager.LoadScene("MenuScene");

        PlayerSpawnArray = GameObject.FindGameObjectsWithTag("PlayerSpawn"); // Find all spawnpoints and put them in an array
        player_spawned = new bool[PlayerSpawnArray.Length]; // Array

        spawnPlayers(cpBinding);

        // Destroy all Spawns
        for (int i = 0; i < PlayerSpawnArray.Length; i++) Destroy(PlayerSpawnArray[i]);

        // Compress the tilemaps so the size equals the actual filled space
        tilemapGround.CompressBounds();
        tilemapWall.CompressBounds();
        tilemapObjects.CompressBounds();
        tilemapTop.CompressBounds();

        // Center camera
        xMin = -1;
        xMax = -1;
        yMin = -1;
        yMax = -1;

        setMaxCoordinates(tilemapGround);
        //setMaxCoordinates(tilemapWall); MAY ONLY BE PLACED ON GROUND --> EDIT EDITOR
        //setMaxCoordinates(tilemapObjects); MAY ONLY BE PLACED ON GROUND --> EDIT EDITOR
        setMaxCoordinates(tilemapTop);

        Camera.main.transform.position = new Vector3(xMin + (xMax-xMin)/2, yMin + (yMax - yMin) / 2, -10);

        // Zoom camera
        if (Camera.main.aspect > (xMax - xMin) / (yMax - yMin)) // Map height is higher than screen height compared to width of both
        {
            // Base camera size on map height
            cameraSize = (yMax - yMin) / 2 + borderSize * 2;
        }
        else
        {
            // Base camera size on map width
            cameraSize = (xMax - xMin) / 2 + borderSize * 2;
        }
        Camera.main.orthographicSize = Mathf.Clamp(cameraSize, cameraSizeMin, cameraSizeMax);
    }

    private void setMaxCoordinates(Tilemap tilemapMax)
    {
        if (tilemapMax.size.x != 0)
        {
            if (tilemapMax.CellToWorld(new Vector3Int(tilemapMax.cellBounds.xMin, 0, 0)).x < xMin || xMin == -1) xMin = tilemapMax.CellToWorld(new Vector3Int(tilemapMax.cellBounds.xMin, 0, 0)).x;
            if (tilemapMax.CellToWorld(new Vector3Int(tilemapMax.cellBounds.xMax, 0, 0)).x > xMax || xMax == -1) xMax = tilemapMax.CellToWorld(new Vector3Int(tilemapMax.cellBounds.xMax, 0, 0)).x;
            if (tilemapMax.CellToWorld(new Vector3Int(0, tilemapMax.cellBounds.yMin, 0)).y < yMin || yMin == -1) yMin = tilemapMax.CellToWorld(new Vector3Int(0, tilemapMax.cellBounds.yMin, 0)).y;
            if (tilemapMax.CellToWorld(new Vector3Int(0, tilemapMax.cellBounds.yMax, 0)).y > yMax || yMax == -1) yMax = tilemapMax.CellToWorld(new Vector3Int(0, tilemapMax.cellBounds.yMax, 0)).y;
        }
    }

    public void CheckGameOver()
    {
        int totalPlayers = GetPlayers();
        totalAlive = totalPlayers;

        // if there are as many dead players as there are alive -1, that means that there is only 1 player remaining and that means that that player has won.

        // For every player that is currently playing, check if they are dead
        for (int i = 0; i <= totalPlayers; i++)
        {
            // You can easily check if a player is dead by trying to find the player, if you cant find the player, the player is dead or not playing, wich is pretty much the same thing.
            players[i] = GameObject.Find("Player " + i + "(Clone)");
            if (players[i] == null) totalAlive--;
        }

        if (totalAlive < 2) GameOver();
    }

    void GameOver()
    {
        // if there are as many rounds played as the amount of rounds that that should be played
        if (currentRound == amountOfRounds)
        {
            canvasUI.GetComponent<Scoreboard>().ShowScoreboard();
        }
        else
        {
            StartNewRound();
        }

    }

    void StartNewRound()
    {
        currentRound++;

        for (int i = 0; i <= playingPlayers; i++)
        {
            if (allplayers[i] != null)
            {
                //players[i].AddComponent<PlayerController>();
                allplayers[i].GetComponent<PlayerController>().ResetPlayer();
                // TODO:: PLACE ALL THE PLAYERS BACK TO THEIR OLD POS
                // TODO:: ACTIVATE ALL THE PLAYERS AGAIN
                // TODO:: RESET THEIR HEALTH
            }
        }

        countdownUI.SetActive(true);
    }

    int GetPlayers()
    {
        int totalPlayers = playingPlayers;

        return totalPlayers;
    }

    int RandomPlayerSpawn()
    {
        while (true)
        {
            // Get a random number
            int possible_spawn = Random.Range(0, PlayerSpawnArray.Length);

            // Check if player already spawned in this spot
            if (!player_spawned[possible_spawn])
            {
                player_spawned[possible_spawn] = true;

                // Return spawn
                return possible_spawn;
            }
        }
    }

    void spawnPlayers(ControllerPlayerBinding playerControllerBinding)
    {
        for (int i = 0; i < 4; i++)
        {
            if (playerControllerBinding.getControllerBinding(i + 1) != 0 && PlayerSpawnArray.Length > i)
            {
                // Create GameObjects
                GameObject tank;
                GameObject tank_crosshair;

                // Get random spawn spot
                int spawn_spot = RandomPlayerSpawn();

                // Spawn crosshair and tank on player spawn
                tank_crosshair = Instantiate(crosshairs[i], PlayerSpawnArray[spawn_spot].transform.position, Quaternion.identity);
                tank = Instantiate(tanks[i], PlayerSpawnArray[spawn_spot].transform.position, Quaternion.identity);

                allplayers[i] = tank;

                tank.GetComponent<PlayerController>().SetCrosshair(tank_crosshair);
                tank.GetComponent<PlayerController>().SetHealthBar(health_bars[i]);
                //tank.GetComponent<PlayerController>().SendPlayerInfo(player_info);
                playingPlayers++;
            }
        }
    }
}