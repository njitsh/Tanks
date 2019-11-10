using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    private float crosshairSpeed = 10f;
    private float crosshairMag;
    private Vector2 moveCrosshairVelocity;

    public string horizontalRightAxis;
    public string verticalRightAxis;

    private float x_co, y_co;
    private bool isKeyboard = false;
    private int crosshairSize = 35;

    public void SetCrosshairControls(string hRightAxis, string vRightAxis, bool isKB)
    {
        horizontalRightAxis = hRightAxis;
        verticalRightAxis = vRightAxis;
        isKeyboard = isKB;
    }

    void Start()
    {
        // Set crosshairs to the middle of the screen
        x_co = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width / 2, 0)).x;
        y_co = Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height / 2)).y;
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
                Vector3 moveCrosshairInput = new Vector3(Input.GetAxisRaw(horizontalRightAxis), Input.GetAxisRaw(verticalRightAxis), 0);
                //Variable crosshair speed
                crosshairMag = Mathf.Clamp01(new Vector2(Input.GetAxisRaw(horizontalRightAxis), Input.GetAxisRaw(verticalRightAxis)).magnitude);
                moveCrosshairVelocity = moveCrosshairInput.normalized * crosshairSpeed * crosshairMag * Time.fixedDeltaTime;

                // Add velocity to position
                x_co += moveCrosshairVelocity.x;
                y_co += moveCrosshairVelocity.y;
            }

            // Limit the crosshair to the screen
            x_co = Mathf.Clamp(x_co, Camera.main.ScreenToWorldPoint(new Vector2((crosshairSize / 2), 0)).x, Camera.main.ScreenToWorldPoint(new Vector2(Screen.width - (crosshairSize / 2), 0)).x);
            y_co = Mathf.Clamp(y_co, Camera.main.ScreenToWorldPoint(new Vector2(0, (crosshairSize / 2))).y, Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height - (crosshairSize / 2))).y);

            // Set new crosshair position
            transform.position = new Vector2(x_co, y_co);
        }
    }
}
