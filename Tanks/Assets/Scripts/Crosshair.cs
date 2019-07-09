using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    public int tank_number;
    private float x_co = 0, y_co = 0;
    private bool isKeyboard = false;

    // Start is called before the first frame update
    void Start()
    {
        switch (tank_number)
        {
            case 1:
                x_co = 2;
                y_co = -2;
                isKeyboard = true;
                break;
            case 2:
                x_co = 1;
                y_co = -1;
                break;
            case 3:
                x_co = 3;
                y_co = -3;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!PauseMenu.GameIsPaused)
        {
            if (isKeyboard)
            {
                transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
            }
        }
    }
}
