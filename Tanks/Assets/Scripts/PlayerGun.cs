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
    private float barrel_length = 0.4f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        transform.position = GameObject.Find("Player").transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = GameObject.Find("Player").transform.position;
        var pos = Camera.main.WorldToScreenPoint(transform.position);
        var dir = Input.mousePosition - pos;
        angle = (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 360) % 360;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if (Input.GetButtonDown("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            fire();
        }
    }

    void fire()
    {
        bulletPos = transform.position;
        var rAngle = angle * Mathf.Deg2Rad;
        bulletPos += new Vector2(Mathf.Cos(rAngle) * barrel_length, Mathf.Sin(rAngle) * barrel_length);
        Instantiate(Bullet, bulletPos, Quaternion.AngleAxis(angle, Vector3.forward));
    }
}
