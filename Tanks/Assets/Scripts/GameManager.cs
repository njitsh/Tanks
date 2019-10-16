using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GameManager : MonoBehaviour
{
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

    public UnityEngine.Tilemaps.Tilemap tilemapGround;
    public UnityEngine.Tilemaps.Tilemap tilemapObjects;

    PlayerController player;

    public int[,] player_info = new int[4, 3];

    // Start is called before the first frame update
    void Start()
    {
        GameObject PCBinding = GameObject.Find("PCBinding");
        ControllerPlayerBinding cpBinding = PCBinding.GetComponent<ControllerPlayerBinding>();

        player_info = cpBinding.getPlayerInfo();

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
            tank.GetComponent<PlayerController>().SendPlayerInfo(player_info);
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

        MapSystem.Load_Map(tilemapGround, tilemapObjects, 1);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
