using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    private float crosshairSpeed = 0.15f;
    private float crosshairMag;
    private Vector2 moveCrosshairVelocity;
    public int tank_number;
    private float x_co = 0, y_co = 0;
    private bool isKeyboard = false;
    private int crosshairSize = 50;

    // Start is called before the first frame update
    void Start()
    {
        switch (tank_number)
        {
            case 1:
                x_co = 2;
                y_co = -2;
                isKeyboard = false;
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
                x_co = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y)).x;
                y_co = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y)).y;
            }
            else
            {
                Vector3 moveCrosshairInput = new Vector3(Input.GetAxisRaw("J" + tank_number + "RightHorizontal"), Input.GetAxisRaw("J" + tank_number + "RightVertical"), 0);
                //Variable crosshair speed
                //crosshairMag = Mathf.Clamp01(new Vector2(Input.GetAxisRaw("J" + tank_number + "RightHorizontal"), Input.GetAxisRaw("J" + tank_number + "RightVertical")).magnitude);
                //moveCrosshairVelocity = moveCrosshairInput.normalized * crosshairSpeed * crosshairMag;
                moveCrosshairVelocity = moveCrosshairInput.normalized * crosshairSpeed * Time.fixedDeltaTime;
                if ((x_co + moveCrosshairVelocity.x < Camera.main.ScreenToWorldPoint(new Vector2(Screen.width - (crosshairSize / 2), Screen.height)).x) && (x_co + moveCrosshairVelocity.x > Camera.main.ScreenToWorldPoint(new Vector2((crosshairSize / 2), 0)).x))
                {
                    x_co += moveCrosshairVelocity.x;
                }
                if ((y_co + moveCrosshairVelocity.y < Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height - (crosshairSize / 2))).y) && (y_co + moveCrosshairVelocity.y > Camera.main.ScreenToWorldPoint(new Vector2(0, (crosshairSize / 2))).y))
                {
                    y_co += moveCrosshairVelocity.y;
                }
            }
            transform.position = new Vector2(x_co, y_co);
        }
    }
}
