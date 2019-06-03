using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector3 moveVelocity;
    private float target_angle, angle;

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
    }
}
