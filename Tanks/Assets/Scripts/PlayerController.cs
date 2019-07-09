using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private float speed = 3;
    private PlayerController player;

    private float angle = 0;
    private float target_angle = 0;
    private float turn_speed = 3;
    private float mag;
    private Rigidbody2D rb;
    private Vector2 moveVelocity;
    
    private string horizontalAxis;
    private string verticalAxis;
    private string aButton;
    private string xButton;
    private string startButton;
    private int controllerNumber;
    public int tank_number;

    public enum Button
    {
        A,
        X,
        Start,
    }

    internal bool ButtonIsDown(Button button)
    {
        switch(button)
        {
            case Button.A:
                return Input.GetButton(aButton);
            case Button.X:
                return Input.GetButton(xButton);
            case Button.Start:
                return Input.GetButton(startButton);
            default:
                return false;
        }
    }

    internal void SetControllerNumber(int number)
    {
        controllerNumber = number;
        horizontalAxis = "J" + controllerNumber + "Horizontal";
        verticalAxis = "J" + controllerNumber + "Vertical";
        aButton = "J" + controllerNumber + "A";
        xButton = "J" + controllerNumber + "X";
        startButton = "J" + controllerNumber + "Start";
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!PauseMenu.GameIsPaused)
        {
            Vector3 moveInput = new Vector3(Input.GetAxisRaw("J" + tank_number + "Horizontal"), Input.GetAxisRaw("J" + tank_number + "Vertical"), 0);

            if (Mathf.Abs(target_angle - angle) < 1)
            {
                mag = Mathf.Clamp01(new Vector2(Input.GetAxis("J" + tank_number + "Horizontal"), Input.GetAxis("J" + tank_number + "Vertical")).magnitude);
                moveVelocity = moveInput.normalized * speed * mag;
            }
            else moveVelocity = moveInput.normalized * 0;

            RotateTank();
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
    }

    void RotateTank()
    {
        // Get Target Angle

        if ((Input.GetAxis("J" + tank_number + "Vertical") != 0) || (Input.GetAxis("J" + tank_number + "Horizontal") != 0))
        {
            target_angle = (Mathf.Atan2(Input.GetAxis("J" + tank_number + "Vertical"), Input.GetAxis("J" + tank_number +  "Horizontal")) * Mathf.Rad2Deg + 360) % 360;
            for (int times = 0; times < turn_speed; times++)
            {
                if (Mathf.Abs(target_angle - angle) < 1) times = 3;
                // Turn clockwise
                else if (((angle > target_angle) && (angle - target_angle <= 180)) || ((angle < target_angle) && (target_angle - angle >= 180))) angle = (angle - 1 + 360) % 360;
                // Turn anti-clockwise
                else if (((angle < target_angle) && (target_angle - angle < 180)) || ((angle > target_angle) && (angle - target_angle > 180))) angle = (angle + 1 + 360) % 360;
            }

            // Rotate tank
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
