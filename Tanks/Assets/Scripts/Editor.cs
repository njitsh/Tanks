using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Editor : MonoBehaviour
{
    public Tile selectedTile = null;

    public Tilemap tilemapGround;
    public Tilemap tilemapWall;
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

    // Tiles with a limit
    [System.Serializable]
    public class limited_tiles
    {
        public TileBase limited_tile;
        public int limit;
        public int current_amount;
    }

    public limited_tiles[] tile_limits;

    // Full tiles array
    public TileBase[] ground_tiles_array;
    public TileBase[] wall_tiles_array;
    public TileBase[] objects_tiles_array;
    public TileBase[] top_tiles_array;

    private void Awake()
    {
        SaveSystem.Init();
        Reset_Bounds();

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
                // Show active tile in selected map layer
                currentCell = activeSelectedMap.WorldToCell(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10)));
                if (selectedTile != null && currentCell != previousCell)
                {
                    activeSelectedMap.SetTile(currentCell, selectedTile);
                    activeSelectedMap.SetTile(previousCell, null);
                    previousCell = currentCell;
                }

                if (Input.GetKey(KeyCode.Mouse0))
                {
                    // Place tile on active map
                    if (!eraseMode && selectedTile != null && ((currentCell.x >= 0 && currentCell.x < maxWidth && currentCell.y >= 0 && currentCell.y < maxHeight && (activeMap == tilemapGround || activeMap == tilemapObjects || activeMap == tilemapTop)) || (currentCell.x >= 0 && currentCell.x < maxWidth * 2 && currentCell.y >= 0 && currentCell.y < maxHeight * 2 && activeMap == tilemapWall)))
                    {
                        if (!Tile_limit_reached(selectedTile))
                        {
                            activeMap.SetTile(currentCell, selectedTile);
                            activeSelectedMap.SetTile(currentCell, null);
                        }
                        else
                        {
                            Debug.Log("Tile Limit Has Been Reached!");
                        }
                    }
                    // Remove tile from active map
                    else if (eraseMode)
                    {
                        Tile_limit_update(activeMap.GetTile(currentCell));
                        activeMap.SetTile(currentCell, null);
                    }
                    else
                    {
                        // Maybe add function to select blocks
                    }
                }
                // Deselect active tile
                else if ((Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Delete) || Input.GetMouseButtonDown(1)) && selectedTile != null)
                {
                    selectedTile = null;
                    activeSelectedMap.SetTile(currentCell, null);
                    eraseMode = false;
                }
                // Toggle erase mode
                else if (Input.GetKeyDown(KeyCode.X))
                {
                    selectedTile = null;
                    activeSelectedMap.SetTile(currentCell, null);
                    eraseMode = !eraseMode;
                }
            }
        }
    }

    public void Reset_Bounds()
    {
        // Resize maps
        tilemapGround.size = new Vector3Int(maxWidth, maxHeight, 1);
        tilemapGround.ResizeBounds();
        tilemapWall.size = new Vector3Int(maxWidth * 2, maxHeight * 2, 1);
        tilemapWall.ResizeBounds();
        tilemapObjects.size = new Vector3Int(maxWidth, maxHeight, 1);
        tilemapObjects.ResizeBounds();
        tilemapTop.size = new Vector3Int(maxWidth, maxHeight, 1);
        tilemapTop.ResizeBounds();
    }

    private bool Tile_limit_reached(Tile s_tile)
    {
        for (int j = 0; j < tile_limits.GetLength(0); j++)
        {
            if (s_tile == tile_limits[j].limited_tile)
            {
                if (tile_limits[j].limit <= tile_limits[j].current_amount)
                {
                    return true;
                }
                else
                {
                    tile_limits[j].current_amount++;
                    return false;
                }
            }
        }
        return false;
    }

    private void Tile_limit_update(TileBase removed_tile)
    {
        for (int j = 0; j < tile_limits.GetLength(0); j++)
        {
            if (removed_tile == tile_limits[j].limited_tile)
            {
                tile_limits[j].current_amount--;
            }
        }
    }

    private void Reset_tile_amount()
    {
        for (int j = 0; j < tile_limits.GetLength(0); j++)
        {
            tile_limits[j].current_amount = 0;
        }
    }

    // Select a tile
    public void Select_Tile(Tile Selected_Tile)
    {
        if (selectedTile == Selected_Tile) Deselect_Tile();
        else
        {
            selectedTile = Selected_Tile;
            eraseMode = false;
        }
    }

    // Deselect active tile
    public void Deselect_Tile()
    {
        selectedTile = null;
        eraseMode = false;
    }

    // Set active map
    public void SetActiveMap(Tilemap active_map)
    {
        Deselect_Tile();
        activeMap = active_map;
    }

    // Set active selected map
    public void SetActiveSelectedMap(Tilemap active_selected_map)
    {
        activeSelectedMap.ClearAllTiles();
        Reset_Bounds();
        activeSelectedMap = active_selected_map;
    }

    // Load last map (in editor folder)
    public void Load_Map()
    {
        MapSystem.Load_Map(tilemapGround, tilemapWall, tilemapObjects, tilemapTop, 0, ground_tiles_array, wall_tiles_array, objects_tiles_array, top_tiles_array);
    }

    // Save map (in editor folder)
    public void Save_Map()
    {
        MapSystem.Save_Map(tilemapGround, tilemapWall, tilemapObjects, tilemapTop, 0, ground_tiles_array, wall_tiles_array, objects_tiles_array, top_tiles_array);
    }

    // Clear current map fully
    public void Clear_Map()
    {
        MapSystem.Clear_Whole_Map(tilemapGround, tilemapWall, tilemapObjects, tilemapTop);
        Reset_tile_amount();
    }

    // Clear current layer
    public void Clear_Layer()
    {
        MapSystem.Clear_Layer(activeMap);
        Reset_tile_amount();
    }
}
