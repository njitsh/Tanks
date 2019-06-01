using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float speed = 3;

    private float angle = 0;
    private float target_angle = 0;
    private float turn_speed = 3;
    private Rigidbody2D rb;
    private Vector2 moveVelocity;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);

        if (((target_angle - angle) > -1) && ((target_angle - angle) < 1)) moveVelocity = moveInput.normalized * speed;
        else moveVelocity = moveInput.normalized * 0;
        float delta_angle = (target_angle - angle);
        print(delta_angle);

        RotateTank();
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
    }

    void RotateTank()
    {
        // Get Target Angle

        if ((Input.GetAxisRaw("Vertical") != 0) || (Input.GetAxisRaw("Horizontal") != 0))
        {
            target_angle = (Mathf.Atan2(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal")) * Mathf.Rad2Deg + 360) % 360;
            for (int times = 0; times < turn_speed; times++)
            {
                // Turn clockwise
                if (((angle > target_angle) && (angle - target_angle <= 180)) || ((angle < target_angle) && (target_angle - angle >= 180))) angle = (angle - 1 + 360) % 360;
                // Turn anti-clockwise
                else if (((angle < target_angle) && (target_angle - angle < 180)) || ((angle > target_angle) && (angle - target_angle > 180))) angle = (angle + 1 + 360) % 360;
            }

            // Rotate tank
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
