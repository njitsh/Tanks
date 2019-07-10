using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector3 moveVelocity;
    private float target_angle;
    public float angle;

    public GameObject Bullet;
    private Vector2 bulletPos;
    private float fireRate = 0.2f;
    private float nextFire = 0.0f;
    private bool releasedFireTrigger = true;
    private float barrel_length = 0.4f;

    public GameObject parentTank;
    public GameObject crosshair;
    public int tank_number;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        transform.position = parentTank.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!PauseMenu.GameIsPaused)
        {
            transform.position = parentTank.transform.position;
            var dir = crosshair.transform.position - transform.position;
            angle = (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 360) % 360;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);


            if ((Input.GetButtonDown("J" + tank_number + "X") || Input.GetAxis("J" + tank_number + "RightTrigger") > 0) && Time.time > nextFire && releasedFireTrigger)
            {
                nextFire = Time.time + fireRate;
                releasedFireTrigger = false;
                fire();
            }
            else if (!Input.GetButtonDown("J" + tank_number + "X") && Input.GetAxis("J" + tank_number + "RightTrigger") == 0)
            {
                releasedFireTrigger = true;
            }
        }
    }

    void fire()
    {
        if (!PauseMenu.GameIsPaused)
        {
            bulletPos = transform.position;
            var rAngle = angle * Mathf.Deg2Rad;
            bulletPos += new Vector2(Mathf.Cos(rAngle) * barrel_length, Mathf.Sin(rAngle) * barrel_length);
            GameObject bullet = Instantiate(Bullet, bulletPos, Quaternion.AngleAxis(angle, Vector3.forward));
            bullet.GetComponent<Bullet>().setTankNumber(tank_number);
        }
    }
}
