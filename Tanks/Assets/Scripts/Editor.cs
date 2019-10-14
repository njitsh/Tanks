using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Editor : MonoBehaviour
{
    public Tile selectedTile = null;

    public Tilemap tilemap;
    public Tilemap selectedTilemap; // Temp place tiles on this layer to show the selected block and location

    Vector3Int currentCell;
    Vector3Int previousCell;

    bool destroyMode = false;

    // Update is called once per frame
    void Update()
    {
        if (!PauseMenu.GameIsPaused)
        {
            currentCell = tilemap.WorldToCell(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10)));
            if (selectedTile != null && currentCell != previousCell)
            {
                selectedTilemap.SetTile(currentCell, selectedTile);
                selectedTilemap.SetTile(previousCell, null);
                previousCell = currentCell;
            };


            if (selectedTile != null) Cursor.visible = false;
            else Cursor.visible = true;
            if (Input.GetKey(KeyCode.Mouse0))
            {
                if (!destroyMode && selectedTile != null)
                {
                    tilemap.SetTile(currentCell, selectedTile);
                    selectedTilemap.SetTile(currentCell, null);
                }
                else if (destroyMode)
                {
                    tilemap.SetTile(currentCell, null);
                }
                else
                {
                    // Maybe add function to select blocks
                }
            }
            else if ((Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Delete) || Input.GetKeyDown(KeyCode.Mouse1)) && selectedTile != null)
            {
                selectedTile = null;
                selectedTilemap.SetTile(currentCell, null);
                destroyMode = false;
            }
            else if (Input.GetKeyDown(KeyCode.X))
            {
                selectedTile = null;
                selectedTilemap.SetTile(currentCell, null);
                destroyMode = !destroyMode;
            }
        }
    }

    public void Select_Tile(Tile Selected_Tile)
    {
        selectedTile = Selected_Tile;
    }
}
