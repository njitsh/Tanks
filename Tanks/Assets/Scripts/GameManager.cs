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

    PlayerController player;
    
    public int[,] player_info = new int[4, 3];

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

        if (cpBinding.getControllerBinding(1) != 0)
        {
            GameObject tank_crosshair_1 = Instantiate(crosshair_1) as GameObject;
            GameObject tank = Instantiate(tank_1);
            tank.GetComponent<PlayerController>().SetCrosshair(tank_crosshair_1);
            tank.GetComponent<PlayerController>().SetHealthBar(health_bar_1);
            //tank.GetComponent<PlayerController>().SendPlayerInfo(player_info);
        }
        if (cpBinding.getControllerBinding(2) != 0)
        {
            GameObject tank_crosshair_2 = Instantiate(crosshair_2) as GameObject;
            GameObject tank = Instantiate(tank_2);
            tank.GetComponent<PlayerController>().SetCrosshair(tank_crosshair_2);
            tank.GetComponent<PlayerController>().SetHealthBar(health_bar_2);
        }
        if (cpBinding.getControllerBinding(3) != 0)
        {
            GameObject tank_crosshair_3 = Instantiate(crosshair_3) as GameObject;
            GameObject tank = Instantiate(tank_3);
            tank.GetComponent<PlayerController>().SetCrosshair(tank_crosshair_3);
            tank.GetComponent<PlayerController>().SetHealthBar(health_bar_3);
        }
        if (cpBinding.getControllerBinding(4) != 0)
        {
            GameObject tank_crosshair_4 = Instantiate(crosshair_4) as GameObject;
            GameObject tank = Instantiate(tank_4);
            tank.GetComponent<PlayerController>().SetCrosshair(tank_crosshair_4);
            tank.GetComponent<PlayerController>().SetHealthBar(health_bar_4);
        }

        MapSystem.Play_Map(tilemapGround, tilemapWall, tilemapObjects, tilemapTop, 1, tile_prefab_array);
        tilemapGround.CompressBounds();
        tilemapWall.CompressBounds();
        tilemapObjects.CompressBounds();
        tilemapTop.CompressBounds();
        /*
        Vector3 offset = transform.up * (transform.localScale.y / 2f) * -1f;
        Vector3 pos = transform.position + offset; //This is the position
        */

        // Center camera
        xMin = -1;
        xMax = -1;
        yMin = -1;
        yMax = -1;

        if (tilemapGround.size.x != 0)
        {
            xMin = tilemapGround.CellToWorld(new Vector3Int(tilemapGround.cellBounds.xMin, 0, 0)).x;
            xMax = tilemapGround.CellToWorld(new Vector3Int(tilemapGround.cellBounds.xMax, 0, 0)).x;
            yMin = tilemapGround.CellToWorld(new Vector3Int(0, tilemapGround.cellBounds.yMin, 0)).y;
            yMax = tilemapGround.CellToWorld(new Vector3Int(0, tilemapGround.cellBounds.yMax, 0)).y;
        }
        if (tilemapWall.size.x != 0)
        {
            if (tilemapWall.CellToWorld(new Vector3Int(tilemapWall.cellBounds.xMin, 0, 0)).x < xMin || xMin == -1) xMin = tilemapWall.CellToWorld(new Vector3Int(tilemapWall.cellBounds.xMin, 0, 0)).x;
            if (tilemapWall.CellToWorld(new Vector3Int(tilemapWall.cellBounds.xMax, 0, 0)).x > xMax || xMax == -1) xMax = tilemapWall.CellToWorld(new Vector3Int(tilemapWall.cellBounds.xMax, 0, 0)).x;
            if (tilemapWall.CellToWorld(new Vector3Int(0, tilemapWall.cellBounds.yMin, 0)).y < yMin || yMin == -1) yMin = tilemapWall.CellToWorld(new Vector3Int(0, tilemapWall.cellBounds.yMin, 0)).y;
            if (tilemapWall.CellToWorld(new Vector3Int(0, tilemapWall.cellBounds.yMax, 0)).y > yMax || yMax == -1) yMax = tilemapWall.CellToWorld(new Vector3Int(0, tilemapWall.cellBounds.yMax, 0)).y;
        }
        if (tilemapObjects.size.x != 0)
        {
            if (tilemapObjects.CellToWorld(new Vector3Int(tilemapObjects.cellBounds.xMin, 0, 0)).x < xMin || xMin == -1) xMin = tilemapObjects.CellToWorld(new Vector3Int(tilemapObjects.cellBounds.xMin, 0, 0)).x;
            if (tilemapObjects.CellToWorld(new Vector3Int(tilemapObjects.cellBounds.xMax, 0, 0)).x > xMax || xMax == -1) xMax = tilemapObjects.CellToWorld(new Vector3Int(tilemapObjects.cellBounds.xMax, 0, 0)).x;
            if (tilemapObjects.CellToWorld(new Vector3Int(0, tilemapObjects.cellBounds.yMin, 0)).y < yMin || yMin == -1) yMin = tilemapObjects.CellToWorld(new Vector3Int(0, tilemapObjects.cellBounds.yMin, 0)).y;
            if (tilemapObjects.CellToWorld(new Vector3Int(0, tilemapObjects.cellBounds.yMax, 0)).y > yMax || yMax == -1) yMax = tilemapObjects.CellToWorld(new Vector3Int(0, tilemapObjects.cellBounds.yMax, 0)).y;
        }
        if (tilemapTop.size.x != 0)
        {
            if (tilemapTop.CellToWorld(new Vector3Int(tilemapTop.cellBounds.xMin, 0, 0)).x < xMin || xMin == -1) xMin = tilemapTop.CellToWorld(new Vector3Int(tilemapTop.cellBounds.xMin, 0, 0)).x;
            if (tilemapTop.CellToWorld(new Vector3Int(tilemapTop.cellBounds.xMax, 0, 0)).x > xMax || xMax == -1) xMax = tilemapTop.CellToWorld(new Vector3Int(tilemapTop.cellBounds.xMax, 0, 0)).x;
            if (tilemapTop.CellToWorld(new Vector3Int(0, tilemapTop.cellBounds.yMin, 0)).y < yMin || yMin == -1) yMin = tilemapTop.CellToWorld(new Vector3Int(0, tilemapTop.cellBounds.yMin, 0)).y;
            if (tilemapTop.CellToWorld(new Vector3Int(0, tilemapTop.cellBounds.yMax, 0)).y > yMax || yMax == -1) yMax = tilemapTop.CellToWorld(new Vector3Int(0, tilemapTop.cellBounds.yMax, 0)).y;
        }
        Camera.main.transform.position = new Vector3(xMin + (xMax-xMin)/2, yMin + (yMax - yMin) / 2, -10);

        // Zoom camera
        if (Camera.main.aspect > (xMax - xMin) / (yMax - yMin)) // Map height is higher than screen height compared to width of both
        {
            // Base camera size on map height
            Camera.main.orthographicSize = (yMax - yMin) / 2 + borderSize * 2;
        }
        else
        {
            // Base camera size on map width
            Camera.main.orthographicSize = (xMax - xMin) / 2 + borderSize * 2;
        }
    }
}