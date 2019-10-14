using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private readonly float vel = 1.5f;
    private Rigidbody2D rb;
    private float angle_shot;
    private float angle;

    private int bullet_damage = 10;

    public float activation_period = 0.03f;
    private float activation_moment;
    public int tank_number;

    int layerMask = (1 << 9);
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        angle = angle_shot * Mathf.Deg2Rad;
        activation_moment = Time.time + activation_period;
    }

    // Update is called once per frame
    void Update()
    {
        if (!PauseMenu.GameIsPaused)
        {
            rb.velocity = new Vector2(Mathf.Cos(angle) * vel, Mathf.Sin(angle) * vel);

            //Ray ray_left = new Ray(new Vector2(Mathf.Cos(angle + 0.1f) + transform.position.x, Mathf.Sin(angle + 0.1f) + transform.position.y) - new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)), new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)));
            //Debug.DrawRay(new Vector2(Mathf.Cos(angle + 0.1f) + transform.position.x, Mathf.Sin(angle + 0.1f) + transform.position.y) - new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)), new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)), Color.blue);
            //Ray ray_right = new Ray(new Vector2(Mathf.Cos(angle - 0.1f) + transform.position.x, Mathf.Sin(angle - 0.1f) + transform.position.y) - new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)), new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)));
            //Debug.DrawRay(new Vector2(Mathf.Cos(angle - 0.1f) + transform.position.x, Mathf.Sin(angle - 0.1f) + transform.position.y) - new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)), new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)), Color.red);

            /*var tempangle = angle * Mathf.Rad2Deg;
            RaycastHit2D rayHitLeft = Physics2D.Raycast(new Vector2(Mathf.Cos(angle + 0.1f) + transform.position.x, Mathf.Sin(angle + 0.1f) + transform.position.y) - new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)), new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)));
            RaycastHit2D rayHitRight = Physics2D.Raycast(new Vector2(Mathf.Cos(angle - 0.1f) + transform.position.x, Mathf.Sin(angle - 0.1f) + transform.position.y) - new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)), new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)));
            if (rayHitLeft && rayHitRight)
            {
                if (((rayHitLeft.transform.tag == "Wall") && (rayHitLeft.distance < 0.1f)) || ((rayHitRight.distance < 0.1f) && (rayHitRight.transform.tag == "Wall")))
                {
                    if (tempangle < 180 && tempangle >= 0)
                    {
                        tempangle = (tempangle + 2 * Mathf.Rad2Deg * Mathf.Atan(0.2f / (Mathf.Abs(rayHitLeft.distance - rayHitRight.distance))) + 360) % 360;
                    }
                    else if (tempangle >= 180 && tempangle < 360)
                    {
                        tempangle = (tempangle - 2 * Mathf.Rad2Deg * Mathf.Atan(0.2f / (Mathf.Abs(rayHitLeft.distance - rayHitRight.distance))) + 360) % 360;
                    }
                }
            }
            else if (rayHitLeft && !rayHitRight)
            {
                if ((rayHitLeft.transform.tag == "Wall") && (rayHitLeft.distance < 0.1f))
                {
                    tempangle = (tempangle - 90 + 360) % 360;
                }
            }
            else if (!rayHitLeft && rayHitRight)
            {
                if ((rayHitRight.transform.tag == "Wall") && (rayHitRight.distance < 0.1f))
                {
                    tempangle = (tempangle + 90) % 360;
                }
            }
            transform.rotation = Quaternion.Euler(0, 0, tempangle);
            angle = tempangle * Mathf.Deg2Rad;*/

            // 2nd attempt
            Ray ray = new Ray(new Vector2(Mathf.Cos(angle) + transform.position.x, Mathf.Sin(angle) + transform.position.y) - new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)), new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)));

            RaycastHit2D hit = Physics2D.Raycast(new Vector2(Mathf.Cos(angle) + transform.position.x, Mathf.Sin(angle) + transform.position.y) - new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)), new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)));
            if (hit.distance < 0.5f && hit.transform.tag != "Bullet")
            {
                Vector2 reflectDir = Vector2.Reflect(ray.direction, hit.normal);
                angle = Mathf.Atan2(reflectDir.y, reflectDir.x) * Mathf.Rad2Deg;
            }

            /*Ray ray = new Ray(new Vector2(Mathf.Cos(angle) + transform.position.x, Mathf.Sin(angle) + transform.position.y) - new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)), new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)));

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Time.deltaTime * vel + .1f))
            {
                Vector3 reflectDir = Vector3.Reflect(ray.direction, hit.normal);
                angle = 90 - Mathf.Atan2(reflectDir.z, reflectDir.x) * Mathf.Rad2Deg;
            }*/
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            PlayerController playerscript = other.gameObject.GetComponent<PlayerController>();
            if ((Time.time > activation_moment) || playerscript.tank_number != tank_number)
            {
                playerscript.Hit(bullet_damage); // Hit player with 10 damage
                Destroy(gameObject); // Destroy Bullet
            }
        }
        else if (other.tag == "Wall")
        {
            Wall wallscript = other.gameObject.GetComponent<Wall>();

            wallscript.Hit(bullet_damage); // Hit wall with 10 damage
        }
    }

    public void setTankNumber(int tanknr)
    {
        tank_number = tanknr;
        string player_b = "Barrel " + tanknr;
        PlayerGun playerGun = GameObject.Find(player_b).GetComponent<PlayerGun>();
        angle_shot = playerGun.angle;
    }
}
