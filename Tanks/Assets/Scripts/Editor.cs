using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Editor : MonoBehaviour
{
    public Tile selectedTile = null;

    public Tilemap tilemapGround;
    public Tilemap selectedTilemapGround; // Temp place tiles on this layer to show the selected block and location

    Vector3Int currentCell;
    Vector3Int previousCell;

    bool eraseMode;

    public readonly int maxWidth = 30;
    public readonly int maxHeight = 20;

    private void Awake()
    {
        SaveSystem.Init();
        GameObject.Find("Main Camera").transform.position = tilemapGround.CellToWorld(new Vector3Int(maxWidth/2, maxHeight/2, 0));
    }

    // Update is called once per frame
    void Update()
    {
        if (!PauseMenu.GameIsPaused)
        {
            currentCell = tilemapGround.WorldToCell(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10)));
            if (selectedTile != null && currentCell != previousCell)
            {
                selectedTilemapGround.SetTile(currentCell, selectedTile);
                selectedTilemapGround.SetTile(previousCell, null);
                previousCell = currentCell;
            };


            //if (selectedTile != null) Cursor.visible = false;
            //else Cursor.visible = true;
            if (Input.GetKey(KeyCode.Mouse0))
            {
                Debug.Log(maxHeight + " " + maxWidth);
                if (!eraseMode && selectedTile != null && currentCell.x >= 0 && currentCell.x < maxWidth && currentCell.y >= 0 && currentCell.y < maxHeight)
                {
                    tilemapGround.SetTile(currentCell, selectedTile);
                    selectedTilemapGround.SetTile(currentCell, null);
                }
                else if (eraseMode)
                {
                    tilemapGround.SetTile(currentCell, null);
                }
                else
                {
                    // Maybe add function to select blocks
                }
            }
            else if ((Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Delete) || Input.GetKeyDown(KeyCode.Mouse1)) && selectedTile != null)
            {
                selectedTile = null;
                selectedTilemapGround.SetTile(currentCell, null);
                eraseMode = false;
            }
            else if (Input.GetKeyDown(KeyCode.X))
            {
                selectedTile = null;
                selectedTilemapGround.SetTile(currentCell, null);
                eraseMode = !eraseMode;
            }
        }
    }

    public void Select_Tile(Tile Selected_Tile)
    {
        selectedTile = Selected_Tile;
        eraseMode = false;
    }

    public void Save_Map()
    {
        // Get bounds
        BoundsInt bounds = tilemapGround.cellBounds;
        int bounds_ground_x = bounds.size.x;
        int bounds_ground_y = bounds.size.y;

        TileBase[] allTiles = tilemapGround.GetTilesBlock(bounds);

        SaveObject saveObject = new SaveObject
        {
            bounds_ground_x = bounds_ground_x,
            bounds_ground_y = bounds_ground_y,
            tile_list = new List<TileBase>(allTiles)
        };

        string json = JsonUtility.ToJson(saveObject);
        SaveSystem.Save(json);

        Debug.Log("Saved!");
    }

    public void Load_Map()
    {
        string mapString = SaveSystem.Load();
        if (mapString != null)
        {
            SaveObject saveObject = JsonUtility.FromJson<SaveObject>(mapString);

            Open_Map(saveObject.tile_list.ToArray(), saveObject.bounds_ground_x, saveObject.bounds_ground_y);

            Debug.Log("Loaded!");
        }
        else
        {
            Debug.Log("No map was loaded!");
        }
    }

    private void Open_Map(TileBase[] map_ground, int bounds_x, int bounds_y)
    {
        for (int x = 0; x < bounds_x; x++)
        {
            for (int y = 0; y < bounds_y; y++)
            {
                tilemapGround.SetTile(new Vector3Int(x, y, 0), map_ground[x + y * bounds_x]);
            }
        }
    }

    public void Clear_Map()
    {
        BoundsInt bounds = tilemapGround.cellBounds;

        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                tilemapGround.SetTile(new Vector3Int(x, y, 0), null);
            }
        }
    }

    public class SaveObject
    {
        public int bounds_ground_x;
        public int bounds_ground_y;
        public List<TileBase> tile_list;
    }
}
