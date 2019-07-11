using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GameManager : MonoBehaviour
{
    public GameObject tank_1;
    public GameObject crosshair_1;
    public GameObject tank_2;
    public GameObject crosshair_2;
    public GameObject tank_3;
    public GameObject crosshair_3;
    public GameObject tank_4;
    public GameObject crosshair_4;

    // Start is called before the first frame update
    void Start()
    {
        GameObject PCBinding = GameObject.Find("PCBinding");
        ControllerPlayerBinding cpBinding = PCBinding.GetComponent<ControllerPlayerBinding>();
        if (cpBinding.getControllerBinding(1) != 0)
        {
            GameObject tank_crosshair_1 = Instantiate(crosshair_1) as GameObject;
            GameObject tank = Instantiate(tank_1);
            tank.GetComponent<PlayerController>().SetCrosshair(tank_crosshair_1);
        }
        if (cpBinding.getControllerBinding(2) != 0)
        {
            GameObject tank_crosshair_2 = Instantiate(crosshair_2) as GameObject;
            GameObject tank = Instantiate(tank_2);
            tank.GetComponent<PlayerController>().SetCrosshair(tank_crosshair_2);
        }
        if (cpBinding.getControllerBinding(3) != 0)
        {
            GameObject tank_crosshair_3 = Instantiate(crosshair_3) as GameObject;
            GameObject tank = Instantiate(tank_3);
            tank.GetComponent<PlayerController>().SetCrosshair(tank_crosshair_3);
        }
        if (cpBinding.getControllerBinding(4) != 0)
        {
            GameObject tank_crosshair_4 = Instantiate(crosshair_4) as GameObject;
            GameObject tank = Instantiate(tank_4);
            tank.GetComponent<PlayerController>().SetCrosshair(tank_crosshair_4);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
