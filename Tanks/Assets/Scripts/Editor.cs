using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Editor : MonoBehaviour
{
    public Tile selectedTile = null;

    public Tilemap tilemapGround;
    public Tilemap tilemapObjects;

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
        GameObject.Find("Main Camera").transform.position = tilemapGround.CellToWorld(new Vector3Int(maxWidth/2, maxHeight/2, 0)) + new Vector3Int(0,0,-10);
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

    public void Save_Map()
    {
        // Get bounds
        BoundsInt bounds_ground = tilemapGround.cellBounds;
        int bounds_ground_x = bounds_ground.size.x;
        int bounds_ground_y = bounds_ground.size.y;

        TileBase[] allTiles_ground = tilemapGround.GetTilesBlock(bounds_ground);

        BoundsInt bounds_objects = tilemapObjects.cellBounds;
        int bounds_objects_x = bounds_objects.size.x;
        int bounds_objects_y = bounds_objects.size.y;

        TileBase[] allTiles_objects = tilemapObjects.GetTilesBlock(bounds_objects);

        SaveObject saveObject = new SaveObject
        {
            bounds_ground_x = bounds_ground_x,
            bounds_ground_y = bounds_ground_y,
            tile_list_ground = new List<TileBase>(allTiles_ground),
            bounds_objects_x = bounds_objects_x,
            bounds_objects_y = bounds_objects_y,
            tile_list_objects = new List<TileBase>(allTiles_objects)
        };

        string json = JsonUtility.ToJson(saveObject);
        SaveSystem.Save(json);
    }

    public void Load_Map()
    {
        string mapString = SaveSystem.Load();
        if (mapString != null)
        {
            SaveObject saveObject = JsonUtility.FromJson<SaveObject>(mapString);

            Open_Map(saveObject.tile_list_ground.ToArray(), saveObject.bounds_ground_x, saveObject.bounds_ground_y, saveObject.tile_list_objects.ToArray(), saveObject.bounds_objects_x, saveObject.bounds_objects_y);
        }
        else
        {
            Debug.Log("No map was loaded!");
        }
    }

    private void Open_Map(TileBase[] map_ground, int bounds_g_x, int bounds_g_y, TileBase[] map_objects, int bounds_o_x, int bounds_o_y)
    {
        for (int x = 0; x < bounds_g_x; x++)
        {
            for (int y = 0; y < bounds_g_y; y++)
            {
                tilemapGround.SetTile(new Vector3Int(x, y, 0), map_ground[x + y * bounds_g_x]);
            }
        }
        for (int x = 0; x < bounds_o_x; x++)
        {
            for (int y = 0; y < bounds_o_y; y++)
            {
                tilemapObjects.SetTile(new Vector3Int(x, y, 0), map_objects[x + y * bounds_o_x]);
            }
        }
    }

    public void Clear_Map()
    {
        BoundsInt bounds_ground = tilemapGround.cellBounds;
        for (int x = 0; x < bounds_ground.size.x; x++)
        {
            for (int y = 0; y < bounds_ground.size.y; y++)
            {
                tilemapGround.SetTile(new Vector3Int(x, y, 0), null);
            }
        }

        BoundsInt bounds_objects = tilemapObjects.cellBounds;
        for (int x = 0; x < bounds_objects.size.x; x++)
        {
            for (int y = 0; y < bounds_objects.size.y; y++)
            {
                tilemapObjects.SetTile(new Vector3Int(x, y, 0), null);
            }
        }
    }

    public void Clear_Layer()
    {
        BoundsInt bounds_objects = activeMap.cellBounds;
        for (int x = 0; x < bounds_objects.size.x; x++)
        {
            for (int y = 0; y < bounds_objects.size.y; y++)
            {
                activeMap.SetTile(new Vector3Int(x, y, 0), null);
            }
        }
    }

    public class SaveObject
    {
        public int bounds_ground_x;
        public int bounds_ground_y;
        public List<TileBase> tile_list_ground;
        public int bounds_objects_x;
        public int bounds_objects_y;
        public List<TileBase> tile_list_objects;
    }
}
