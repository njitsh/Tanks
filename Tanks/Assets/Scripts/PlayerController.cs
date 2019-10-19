using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private float speed = 3;

    private float angle = 0;
    private float target_angle = 0;
    private float turn_speed = 3;
    private float mag;
    private Rigidbody2D rb;
    private Vector3 moveVelocity;

    public int health;
    public int healthMax;
    public bool isDead;
    public GameObject HealthBar;
    private HealthBar HBar;

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

    PauseMenu pause;
    GameManager gameManager;

    int totalDeaths = 0;
    int totalAlive = 0;

    int roundsPlayed;
    public int amountOfRounds =2;
    GameObject[] players = new GameObject[3];
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
        groundmap = GameObject.Find("Ground").GetComponent<Tilemap>();
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
            Die();
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

    public void Hit(int damage)
    {
        if (health > damage) health -= damage;
        else
        {
            health = 0;
            Die();
        }
        HealthBar HBar = HealthBar.GetComponent<HealthBar>();
        HBar.SetHealthState((float)health / healthMax);
    }

    void Die()
    {
        gameObject.SetActive(false);
        player_tank_crosshair.SetActive(false);
        CheckGameOver();
        //      Debug.Log(pause);


    }
    void CheckGameOver()
    {

        int totalPlayers = GetPlayers();

        // if there are as many dead players as there are alive -1, that means that there is only 1 player remaining and that means that that player has won.
        
        // For every player that is currently playing, check if they are dead
        for (int i = 1; i <= totalPlayers;)
        {
            // You can easily check if a player is dead by trying to find the player, if you cant find the player, the player is dead or not playing, wich is pretty much the same thing.
            players[i] = GameObject.Find("Player " + i + "(Clone)");
            Debug.Log(players[i]);
            if (players[i] == null)
            {
                Debug.Log("This Player Is dead");
                totalDeaths++;
            }
            else
            {
                Debug.Log("This player is alive");
                totalAlive++;
                // if there are more than 2 players alive, there is no point in going on.
                if (totalAlive >= 2) return;
            }
            i++;
            Debug.Log(i);
        }

        if (totalDeaths == totalPlayers - 1) GameOver();
    }
    void GameOver()
    {
        // if there are as many rounds played as the amount of rounds that that should be played
        if (roundsPlayed == amountOfRounds)
        {
            pause = FindObjectOfType(typeof(PauseMenu)) as PauseMenu;
            pause.Pause();
        }
        else
        {
            roundsPlayed++;
            for(int i=1; i<= gameManager.playingPlayers;)
            {
                players[i] = GameObject.Find("Player " + i + "(Clone)");
                players[i].AddComponent<PlayerController>();
                players[i].SetActive(true);
                Debug.Log(players[i]);
                // TODO:: PLACE ALL THE PLAYERS BACK TO THEIR OLD POS
                // TODO:: ACTIVATE ALL THE PLAYERS AGAIN
                // TODO:: RESET THEIR HEALTH
            }
        }
        
    }
    int GetPlayers()
    {     
        int totalPlayers = gameManager.playingPlayers;

        return totalPlayers;
    }
}
//if(player2 != null)
//            {
//                if (player3 != null)
//                {
//                    if (player4 != null)
//                    {

//                    }
//                }
//            }