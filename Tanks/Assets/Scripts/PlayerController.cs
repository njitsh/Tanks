using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed = 6;

    private float angle = 0;
    private float target_angle = 0;
    private readonly float turn_speed = 5;
    private float mag;
    private Rigidbody2D rb;
    private Vector3 moveVelocity;

    public int health;
    public int healthMax;
    public bool isDead;
    public GameObject HealthBar;

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

    private float groundSpeed;

    public Tile sandTile;
    public Tile grassTile;

    public Tilemap groundmap;

    GameManager gameManager;

    // Player stats
    public int kills = 0;
    public int deaths = 0;


    // TODO ?
    // setup custom constructed class with HP, ammo?, and wich class is currently being used. http://ilkinulas.github.io/development/unity/2016/05/30/monobehaviour-constructor.html

    public void SetCrosshair(GameObject crosshair_tank)
    {
        player_tank_crosshair = crosshair_tank;
        PlayerGun player_tank_gun_script = player_tank_gun.GetComponent<PlayerGun>();
        player_tank_gun_script.SetCrosshairBarrel(player_tank_crosshair);
    }

    public void SetHealthBar(GameObject healthbar_tank)
    {
        HealthBar = healthbar_tank;
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
        switch (button)
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

    // Start is called before the first frame update
    void Start()
    {
        isDead = false;
        gameManager = FindObjectOfType(typeof(GameManager)) as GameManager;
        healthMax = 100; // temp
        health = healthMax;
        GameObject PCBinding = GameObject.Find("PCBinding");
        ControllerPlayerBinding cpBinding = PCBinding.GetComponent<ControllerPlayerBinding>();
        SetControllerNumber(cpBinding.getControllerBinding(tank_number));
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        groundmap = GameObject.Find("GroundGrid").GetComponent<Tilemap>();
    }
    
    // Update is called once per frame
    void Update()
    {
        if (!PauseMenu.GameIsPaused)
        {
            Vector3 moveInput = new Vector3(Input.GetAxisRaw(horizontalAxis), Input.GetAxisRaw(verticalAxis), 0);
            if (Mathf.Abs(target_angle - angle) < 1)
            {
                mag = Mathf.Clamp01(new Vector3(Input.GetAxis(horizontalAxis), Input.GetAxis(verticalAxis), 0).magnitude);
                if (MapSystem.Get_Tile_Type(transform.position, groundmap) == sandTile) groundSpeed = 0.5f;
                else if (MapSystem.Get_Tile_Type(transform.position, groundmap) == grassTile) groundSpeed = 0.8f;
                else groundSpeed = 1;
                moveVelocity = moveInput.normalized * speed * groundSpeed * mag;
            }
            else moveVelocity = new Vector3(0,0,0);

            RotateTank();
        }
        if (health <= 0)
        {
            Die(0);
        }

    }

    void FixedUpdate()
    {
        if (!PauseMenu.GameIsPaused)
        {
            transform.position += moveVelocity * Time.fixedDeltaTime;
        }
    }

    void RotateTank()
    {
        // Get Target Angle

        if ((Input.GetAxisRaw(verticalAxis) != 0) || (Input.GetAxisRaw(horizontalAxis) != 0))
        {
            target_angle = (Mathf.Atan2(Input.GetAxisRaw(verticalAxis), Input.GetAxisRaw(horizontalAxis)) * Mathf.Rad2Deg + 360) % 360;
            for (int times = 0; times < turn_speed * groundSpeed; times++)
            {
                if (Mathf.Abs(target_angle - angle) < 1) times = (int) (turn_speed * groundSpeed);
                // Turn clockwise
                else if (((angle > target_angle) && (angle - target_angle <= 180)) || ((angle < target_angle) && (target_angle - angle >= 180))) angle = (angle - 1 + 360) % 360;
                // Turn anti-clockwise
                else if (((angle < target_angle) && (target_angle - angle < 180)) || ((angle > target_angle) && (angle - target_angle > 180))) angle = (angle + 1 + 360) % 360;
            }

            // Rotate tank
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    public void Hit(int damage, int fired_by)
    {
        if (health > damage) health -= damage;
        else
        {
            health = 0;
            Die(fired_by);
        }
        setHealthState(health);
    }

    void Die(int fired_by)
    {
        deaths++; // Increase deaths for player that died
        if (fired_by > 0) gameManager.allplayers[fired_by - 1].GetComponent<PlayerController>().kills++; // Increase kills for player that killed this player if it was a player
        gameObject.SetActive(false);
        player_tank_crosshair.SetActive(false);
        gameManager.CheckGameOver();
    }

    public void ResetPlayer(Vector2 respawnPos)
    {
        gameObject.SetActive(true);
        player_tank_crosshair.SetActive(true);
        transform.position = respawnPos;
        health = healthMax;
        setHealthState(health);
    }

    private void setHealthState(int health)
    {
        HealthBar HBar = HealthBar.GetComponent<HealthBar>();
        HBar.SetHealthState((float)health / healthMax);
    }
}