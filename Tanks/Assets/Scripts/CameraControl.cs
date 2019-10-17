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

    float cameraSizeMax = 14f;
    float cameraSizeMin = 5f;
    float cameraSize = 14f;
    float zoomSpeed = 0.5f;

    public float dragSpeed = 2;
    private Vector3 dragOrigin;

    void Start()
    {
        transform.position = tilemapGround.CellToWorld(new Vector3Int(maxWidth / 2, maxHeight / 2, 0)) + new Vector3Int(0, 0, -10);
    }

    void Update()
    {
        // Drag Camera
        if (Input.GetMouseButtonDown(1))
        {
            dragOrigin = Input.mousePosition;
            return;
        }

        if (!Input.GetMouseButton(1)) return;

        Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
        Vector3 move = new Vector3(pos.x * dragSpeed, pos.y * dragSpeed, 0);

        movement = move;
    }

    void LateUpdate()
    {
        // Camera Zoom
        cameraSize += Input.mouseScrollDelta.y * zoomSpeed;
        cameraSize = Mathf.Clamp(cameraSize, cameraSizeMin, cameraSizeMax);
        Camera.main.orthographicSize = cameraSize;

        // Camera Movement
        for (int i = 5; i >= 1; i--)
        {
            if (Input.GetAxisRaw("J" + i + "Horizontal") != 0 || Input.GetAxisRaw("J" + i + "Vertical") != 0)
            {
                movement = new Vector3(Input.GetAxisRaw("J" + i + "Horizontal") * camera_speed, Input.GetAxisRaw("J" + i + "Vertical") * camera_speed, 0.0f);
            }
        }
        if (tilemapGround.WorldToCell(transform.position + movement).x > 0 && tilemapGround.WorldToCell(transform.position + movement).x < maxWidth) transform.Translate(movement.x, 0, 0);
        if (tilemapGround.WorldToCell(transform.position + movement).y > 0 && tilemapGround.WorldToCell(transform.position + movement).y < maxHeight) transform.Translate(0, movement.y, 0);
        movement = new Vector3();
    }
}
