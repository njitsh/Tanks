using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    // Linked tanks
    public GameObject tank_1;
    public GameObject crosshair_1;
    public GameObject health_bar_1;
    public GameObject tank_2;
    public GameObject crosshair_2;
    public GameObject health_bar_2;
    public GameObject tank_3;
    public GameObject crosshair_3;
    public GameObject health_bar_3;
    public GameObject tank_4;
    public GameObject crosshair_4;
    public GameObject health_bar_4;

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

    PauseMenu pause;

    int totalAlive = 0;

    int currentRound;
    public int amountOfRounds = 3;

    GameObject[] players = new GameObject[4];
    GameObject[] allplayers = new GameObject[4];

    public int[,] player_info = new int[4, 3];

    float cameraSizeMax = 14f;
    float cameraSizeMin = 5f;
    float cameraSize = 14f;

    float xMin;
    float xMax;
    float yMin;
    float yMax;

    float borderSize = 1f;

    // Start is called before the first frame update
    void Start()
    {
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

        MapSystem.Play_Map(tilemapGround, tilemapWall, tilemapObjects, tilemapTop, 1, tile_prefab_array, ground_tiles_array, wall_tiles_array, objects_tiles_array, top_tiles_array);

        // Find spawnpoints
        GameObject spawn_p1 = GameObject.Find("PlayerSpawn1(Clone)");
        GameObject spawn_p2 = GameObject.Find("PlayerSpawn2(Clone)");
        GameObject spawn_p3 = GameObject.Find("PlayerSpawn3(Clone)");
        GameObject spawn_p4 = GameObject.Find("PlayerSpawn4(Clone)");

        if (cpBinding.getControllerBinding(1) != 0)
        {
            // Instantiate tank from spawnpoint if available
            GameObject tank;
            GameObject tank_crosshair_1;

            if (spawn_p1 != null)
            {
                tank_crosshair_1 = Instantiate(crosshair_1, spawn_p1.transform.position, Quaternion.identity);
                tank = Instantiate(tank_1, spawn_p1.transform.position, Quaternion.identity);
            }
            else
            {
                tank_crosshair_1 = Instantiate(crosshair_1);
                tank = Instantiate(tank_1);
            }

            allplayers[0] = tank;

            tank.GetComponent<PlayerController>().SetCrosshair(tank_crosshair_1);
            tank.GetComponent<PlayerController>().SetHealthBar(health_bar_1);
            tank.GetComponent<PlayerController>().SendPlayerInfo(player_info);
            playingPlayers++;
        }
        if (cpBinding.getControllerBinding(2) != 0)
        {
            // Instantiate tank from spawnpoint if available
            GameObject tank;
            GameObject tank_crosshair_2;

            if (spawn_p2 != null)
            {
                tank_crosshair_2 = Instantiate(crosshair_2, spawn_p2.transform.position, Quaternion.identity);
                tank = Instantiate(tank_2, spawn_p2.transform.position, Quaternion.identity);
            }
            else
            {
                tank_crosshair_2 = Instantiate(crosshair_2);
                tank = Instantiate(tank_2);
            }

            allplayers[1] = tank;

            tank.GetComponent<PlayerController>().SetCrosshair(tank_crosshair_2);
            tank.GetComponent<PlayerController>().SetHealthBar(health_bar_2);
            playingPlayers++;
        }
        if (cpBinding.getControllerBinding(3) != 0)
        {
            // Instantiate tank from spawnpoint if available
            GameObject tank;
            GameObject tank_crosshair_3;

            if (spawn_p3 != null)
            {
                tank_crosshair_3 = Instantiate(crosshair_3, spawn_p3.transform.position, Quaternion.identity);
                tank = Instantiate(tank_3, spawn_p3.transform.position, Quaternion.identity);
            }
            else
            {
                tank_crosshair_3 = Instantiate(crosshair_3);
                tank = Instantiate(tank_3);
            }

            allplayers[2] = tank;

            tank.GetComponent<PlayerController>().SetCrosshair(tank_crosshair_3);
            tank.GetComponent<PlayerController>().SetHealthBar(health_bar_3);
            playingPlayers++;
        }
        if (cpBinding.getControllerBinding(4) != 0)
        {
            // Instantiate tank from spawnpoint if available
            GameObject tank;
            GameObject tank_crosshair_4;

            if (spawn_p2 != null)
            {
                tank_crosshair_4 = Instantiate(crosshair_4, spawn_p4.transform.position, Quaternion.identity);
                tank = Instantiate(tank_4, spawn_p4.transform.position, Quaternion.identity);
            }
            else
            {
                tank_crosshair_4 = Instantiate(crosshair_4);
                tank = Instantiate(tank_4);
            }

            allplayers[3] = tank;

            tank.GetComponent<PlayerController>().SetCrosshair(tank_crosshair_4);
            tank.GetComponent<PlayerController>().SetHealthBar(health_bar_4);
            playingPlayers++;
        }
        tilemapGround.CompressBounds();
        tilemapWall.CompressBounds();
        tilemapObjects.CompressBounds();
        tilemapTop.CompressBounds();

        // Destroy spawnpoints
        if (spawn_p1 != null) Destroy(spawn_p1);
        if (spawn_p2 != null) Destroy(spawn_p2);
        if (spawn_p3 != null) Destroy(spawn_p3);
        if (spawn_p4 != null) Destroy(spawn_p4);

        /*
        Vector3 offset = transform.up * (transform.localScale.y / 2f) * -1f;
        Vector3 pos = transform.position + offset; //This is the position
        */

        // Center camera
        xMin = -1;
        xMax = -1;
        yMin = -1;
        yMax = -1;

        setMaxCoordinates(tilemapGround);
        //setMaxCoordinates(tilemapWall); NEEDS TO BE PLACED ON GROUND
        //setMaxCoordinates(tilemapObjects); NEEDS TO BE PLACED ON GROUND
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
            Debug.Log(players[i]);
            if (players[i] == null)
            {
                Debug.Log("This Player Is dead");
                totalAlive--;
            }
            else
            {
                Debug.Log("This player is alive");
            }
        }

        if (totalAlive < 2) GameOver();
    }

    void GameOver()
    {
        // if there are as many rounds played as the amount of rounds that that should be played
        if (currentRound == amountOfRounds)
        {
            pause = FindObjectOfType(typeof(PauseMenu)) as PauseMenu;
            pause.Pause();
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
    }

    int GetPlayers()
    {
        int totalPlayers = playingPlayers;

        return totalPlayers;
    }
}