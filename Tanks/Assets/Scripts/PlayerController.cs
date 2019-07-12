using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private float speed = 3;

    private float angle = 0;
    private float target_angle = 0;
    private float turn_speed = 3;
    private float mag;
    private Rigidbody2D rb;
    private Vector2 moveVelocity;

    public int health;
    public int healthMax;

    public int tank_color;

    private string horizontalAxis;
    private string verticalAxis;
    public string horizontalRightAxis;
    public string verticalRightAxis;
    private string aButton;
    private string xButton;
    private string rightTriggerButton;
    private string startButton;
    private int controllerNumber;
    private bool isKeyboard;
    public int tank_number;

    public GameObject player_tank_crosshair;
    public GameObject player_tank_gun;

    private PlayerController player;

    // TODO ?
    // setup custom constructed class with HP, ammo?, and wich class is currently being used. http://ilkinulas.github.io/development/unity/2016/05/30/monobehaviour-constructor.html

    public void SetCrosshair(GameObject crosshair_tank)
    {
        player_tank_crosshair = crosshair_tank;
        PlayerGun player_tank_gun_script = player_tank_gun.GetComponent<PlayerGun>();
        player_tank_gun_script.SetCrosshairBarrel(player_tank_crosshair);
    }

    public void SendPlayerInfo(int[,] player_info)
    {
        SetControllerNumber(player_info[tank_number, 0]);
        SetColor(player_info[tank_number, 1]);
    }

    public void SetColor(int tcolor)
    {
        tank_color = tcolor;
    }

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
        horizontalRightAxis = "J" + controllerNumber + "RightHorizontal";
        verticalRightAxis = "J" + controllerNumber + "RightVertical";
        aButton = "J" + controllerNumber + "A";
        xButton = "J" + controllerNumber + "X";
        rightTriggerButton = "J" + controllerNumber + "RightTrigger";
        startButton = "J" + controllerNumber + "Start";

        // Set all controls
        GameObject PCBinding = GameObject.Find("PCBinding");
        ControllerPlayerBinding cpBinding = PCBinding.GetComponent<ControllerPlayerBinding>();

        Crosshair player_tank_crosshair_script = player_tank_crosshair.GetComponent<Crosshair>();
        if (cpBinding.getControllerBinding(tank_number) == 5) isKeyboard = true;
        player_tank_crosshair_script.SetCrosshairControls(horizontalRightAxis, verticalRightAxis, isKeyboard);

        PlayerGun player_tank_gun_script = player_tank_gun.GetComponent<PlayerGun>();
        player_tank_gun_script.SetGunController(xButton, rightTriggerButton);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "player") {
            //player


        }
    }

    // Start is called before the first frame update
    void Start()
    {
        health = healthMax;
        GameObject PCBinding = GameObject.Find("PCBinding");
        ControllerPlayerBinding cpBinding = PCBinding.GetComponent<ControllerPlayerBinding>();
        SetControllerNumber(cpBinding.getControllerBinding(tank_number));
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!PauseMenu.GameIsPaused)
        {
            Vector3 moveInput = new Vector3(Input.GetAxisRaw(horizontalAxis), Input.GetAxisRaw(verticalAxis), 0);

            if (Mathf.Abs(target_angle - angle) < 1)
            {
                mag = Mathf.Clamp01(new Vector2(Input.GetAxis(horizontalAxis), Input.GetAxis(verticalAxis)).magnitude);
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

        if ((Input.GetAxisRaw(verticalAxis) != 0) || (Input.GetAxisRaw(horizontalAxis) != 0))
        {
            target_angle = (Mathf.Atan2(Input.GetAxisRaw(verticalAxis), Input.GetAxisRaw(horizontalAxis)) * Mathf.Rad2Deg + 360) % 360;
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
