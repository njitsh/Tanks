using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float camera_speed = 0.3f;

    private Vector3 movement;

    public UnityEngine.Tilemaps.Tilemap tilemapGround;

    public readonly int maxWidth = 30;
    public readonly int maxHeight = 20;

    void Start()
    {
        transform.position = tilemapGround.CellToWorld(new Vector3Int(maxWidth / 2, maxHeight / 2, 0)) + new Vector3Int(0, 0, -10);
    }

    void Update()
    {
        movement = new Vector3();
        for (int i = 5; i >= 1; i--)
        {
            if (Input.GetAxisRaw("J" + i + "Horizontal") != 0 || Input.GetAxisRaw("J" + i + "Vertical") != 0)
            {
                movement = new Vector3(Input.GetAxisRaw("J" + i + "Horizontal") * camera_speed, Input.GetAxisRaw("J" + i + "Vertical") * camera_speed, 0.0f);
            }
        }
        if (tilemapGround.WorldToCell(transform.position + movement).x > 0 && tilemapGround.WorldToCell(transform.position + movement).x < maxWidth && tilemapGround.WorldToCell(transform.position + movement).y > 0 && tilemapGround.WorldToCell(transform.position + movement).y < maxHeight) transform.Translate(movement);
    }
}
