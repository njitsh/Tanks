using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float vel = 10;
    private Rigidbody2D rb;
    private float angle_shot;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GameObject barrel = GameObject.Find("Barrel");
        PlayerGun playerGun = barrel.GetComponent<PlayerGun>();
        angle_shot = playerGun.angle;
    }

    // Update is called once per frame
    void Update()
    {
        float angle = angle_shot * Mathf.Deg2Rad;

        rb.velocity = new Vector2(Mathf.Cos(angle)*vel, Mathf.Sin(angle) * vel);

        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        /*if (Physics.Raycast(ray, out hit, Time.deltaTime * vel + 0.1f, collisionMask))
        {
            Vector3 reflectDir = Vector3.Reflect(ray.direction, hit.normal);
            float rot = 90 - Mathf.Atan2(reflectDir.y, reflectDir.x) * Mathf.Rad2Deg;
            transform.eulerAngles = new Vector3(0, rot, 0);
        }*/

        Destroy(gameObject, 1f);
    }
}
