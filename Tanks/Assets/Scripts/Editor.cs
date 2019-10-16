using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Editor : MonoBehaviour
{
    public Tile selectedTile = null;

    public Tilemap tilemapGround;
    public Tilemap tilemapObjects;
    public Tilemap tilemapTop;

    public Tilemap tilemapSelectedGround;

    Vector3Int currentCell;
    Vector3Int previousCell;

    Tilemap activeMap;
    Tilemap activeSelectedMap;

    bool eraseMode;

    public readonly int maxWidth = 30;
    public readonly int maxHeight = 20;

    private void Awake()
    {
        SaveSystem.Init();

        // Resize maps
        tilemapGround.size = new Vector3Int(maxWidth, maxHeight, 0);
        tilemapGround.ResizeBounds();
        tilemapObjects.size = new Vector3Int(maxWidth * 2, maxHeight * 2, 0);
        tilemapObjects.ResizeBounds();
        tilemapTop.size = new Vector3Int(maxWidth, maxHeight, 0);
        tilemapTop.ResizeBounds();

        activeMap = tilemapGround;
        activeSelectedMap = tilemapSelectedGround;
    }

    // Update is called once per frame
    void Update()
    {
        if (!PauseMenu.GameIsPaused)
        {
            if (activeMap != null && activeSelectedMap != null)
            {
                currentCell = activeSelectedMap.WorldToCell(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10)));
                if (selectedTile != null && currentCell != previousCell)
                {
                    activeSelectedMap.SetTile(currentCell, selectedTile);
                    activeSelectedMap.SetTile(previousCell, null);
                    previousCell = currentCell;
                }

                if (Input.GetKey(KeyCode.Mouse0))
                {
                    if (!eraseMode && selectedTile != null && ((currentCell.x >= 0 && currentCell.x < maxWidth && currentCell.y >= 0 && currentCell.y < maxHeight && activeMap == tilemapGround) || (currentCell.x >= 0 && currentCell.x < maxWidth * 2 && currentCell.y >= 0 && currentCell.y < maxHeight * 2 && activeMap == tilemapObjects)))
                    {
                        activeMap.SetTile(currentCell, selectedTile);
                        activeSelectedMap.SetTile(currentCell, null);
                    }
                    else if (eraseMode)
                    {
                        activeMap.SetTile(currentCell, null);
                    }
                    else
                    {
                        // Maybe add function to select blocks
                    }
                }
                else if ((Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Delete) || Input.GetKeyDown(KeyCode.Mouse1)) && selectedTile != null)
                {
                    selectedTile = null;
                    activeSelectedMap.SetTile(currentCell, null);
                    eraseMode = false;
                }
                else if (Input.GetKeyDown(KeyCode.X))
                {
                    selectedTile = null;
                    activeSelectedMap.SetTile(currentCell, null);
                    eraseMode = !eraseMode;
                }
            }
        }
    }

    public void Select_Tile(Tile Selected_Tile)
    {
        selectedTile = Selected_Tile;
        eraseMode = false;
    }

    public void SetActiveMap(Tilemap active_map)
    {
        activeMap = active_map;
    }

    public void SetActiveSelectedMap(Tilemap active_selected_map)
    {
        activeSelectedMap = active_selected_map;
    }

    public void Load_Map()
    {
        MapSystem.Load_Map(tilemapGround, tilemapObjects, 0);
    }

    public void Save_Map()
    {
        MapSystem.Save_Map(tilemapGround, tilemapObjects, 0);
    }

    public void Clear_Map()
    {
        MapSystem.Clear_Whole_Map(tilemapGround, tilemapObjects);
    }

    public void Clear_Layer()
    {
        MapSystem.Clear_Layer(activeMap);
    }
}
