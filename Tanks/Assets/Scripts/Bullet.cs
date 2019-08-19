using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private readonly float vel = 10;
    private Rigidbody2D rb;
    private float angle_shot;

    public float activation_period = 0.02f;
    private float activation_moment;
    public int tank_number;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        activation_moment = Time.time + activation_period;
    }

    // Update is called once per frame
    void Update()
    {
        if (!PauseMenu.GameIsPaused)
        {
            float angle = angle_shot * Mathf.Deg2Rad;

            rb.velocity = new Vector2(Mathf.Cos(angle) * vel, Mathf.Sin(angle) * vel);

            //Ray ray = new Ray(transform.position, transform.forward);
            //RaycastHit hit;

            /*if (Physics.Raycast(ray, out hit, Time.deltaTime * vel + 0.1f, collisionMask))
            {
                Vector3 reflectDir = Vector3.Reflect(ray.direction, hit.normal);
                float rot = 90 - Mathf.Atan2(reflectDir.y, reflectDir.x) * Mathf.Rad2Deg;
                transform.eulerAngles = new Vector3(0, rot, 0);
            }*/

            //Destroy(gameObject, 1f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            PlayerController playerscript = other.gameObject.GetComponent<PlayerController>();
            if ((Time.time > activation_moment) || playerscript.tank_number != tank_number)
            {
                playerscript.Hit(10); // Hit player with 10 damage
                Destroy(gameObject); // Destroy Bullet
            }
        }
        else if (other.tag == "Wall")
        {
            // ADD BOUNCE

            Destroy(gameObject); // Destroy Bullet
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
