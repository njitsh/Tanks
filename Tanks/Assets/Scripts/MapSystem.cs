using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class MapSystem
{
    // Tile to prefab array class
    [System.Serializable]
    public class tile_to_prefab
    {
        public TileBase tile;
        public GameObject prefab_object;
    }

    public static void Load_Map(Tilemap tMGround, Tilemap tMWall, Tilemap tMObjects, Tilemap tMTop, int folder)
    {
        string mapString = SaveSystem.Load(folder);
        if (mapString != null)
        {
            SaveObject saveObject = JsonUtility.FromJson<SaveObject>(mapString);

            Load_Layer(tMGround, saveObject.tile_list_ground.ToArray(), saveObject.bounds_ground_x, saveObject.bounds_ground_y);
            Load_Layer(tMWall, saveObject.tile_list_wall.ToArray(), saveObject.bounds_wall_x, saveObject.bounds_wall_y);
            Load_Layer(tMObjects, saveObject.tile_list_objects.ToArray(), saveObject.bounds_objects_x, saveObject.bounds_objects_y);
            Load_Layer(tMTop, saveObject.tile_list_top.ToArray(), saveObject.bounds_top_x, saveObject.bounds_top_y);
        }
        else Debug.Log("No map was loaded!");
    }

    private static void Load_Layer(Tilemap tilemap, TileBase[] map, int bounds_x, int bounds_y)
    {
        if (map.Length != 0)
        {
            for (int x = 0; x < bounds_x; x++)
            {
                for (int y = 0; y < bounds_y; y++)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), map[x + y * bounds_x]);
                }
            }
        }
    }

    public static void Play_Map(Tilemap tMGround, Tilemap tMWall, Tilemap tMObjects, Tilemap tMTop, int folder, tile_to_prefab[] tile_prefab_array)
    {
        string mapString = SaveSystem.Load(folder);
        if (mapString != null)
        {
            SaveObject saveObject = JsonUtility.FromJson<SaveObject>(mapString);

            Load_And_Play_Layer(tMGround, saveObject.tile_list_ground.ToArray(), saveObject.bounds_ground_x, saveObject.bounds_ground_y, tile_prefab_array);
            Load_And_Play_Layer(tMWall, saveObject.tile_list_wall.ToArray(), saveObject.bounds_wall_x, saveObject.bounds_wall_y, tile_prefab_array);
            Load_And_Play_Layer(tMObjects, saveObject.tile_list_objects.ToArray(), saveObject.bounds_objects_x, saveObject.bounds_objects_y, tile_prefab_array);
            Load_And_Play_Layer(tMTop, saveObject.tile_list_top.ToArray(), saveObject.bounds_top_x, saveObject.bounds_top_y, tile_prefab_array);
        }
        else Debug.Log("No map was loaded!");
    }

    private static void Load_And_Play_Layer(Tilemap tilemap, TileBase[] map, int bounds_x, int bounds_y, tile_to_prefab[] tile_prefab_array)
    {
        if (map.Length != 0)
        {
            for (int x = 0; x < bounds_x; x++)
            {
                for (int y = 0; y < bounds_y; y++)
                {
                    if (!Placed_Prefab(map[x + y * bounds_x], tile_prefab_array, x, y, tilemap)) tilemap.SetTile(new Vector3Int(x, y, 0), map[x + y * bounds_x]);
                }
            }
        }
    }

    public static bool Placed_Prefab(TileBase tile, tile_to_prefab[] tile_prefab_array, int x, int y, Tilemap tilemap)
    {
        for (int i = 0; i < tile_prefab_array.GetLength(0); i++)
        {
            if (tile == tile_prefab_array[i].tile)
            {
                UnityEngine.Object.Instantiate(tile_prefab_array[i].prefab_object, tilemap.CellToWorld(new Vector3Int(x, y, 0)) + new Vector3(0.25f, 0.25f, 0.25f), Quaternion.identity);
                return true;
            }
        }
        return false;
    }

    public static void Save_Map(Tilemap tMGround, Tilemap tMWall, Tilemap tMObjects, Tilemap tMTop, int folder)
    {
        // Get bounds

        // Ground
        BoundsInt bounds_ground = tMGround.cellBounds;
        int bounds_ground_x = bounds_ground.size.x;
        int bounds_ground_y = bounds_ground.size.y;

        TileBase[] allTiles_ground = tMGround.GetTilesBlock(bounds_ground);

        // Wall
        BoundsInt bounds_wall = tMWall.cellBounds;
        int bounds_wall_x = bounds_wall.size.x;
        int bounds_wall_y = bounds_wall.size.y;

        TileBase[] allTiles_wall = tMWall.GetTilesBlock(bounds_wall);

        // Objects
        BoundsInt bounds_objects = tMObjects.cellBounds;
        int bounds_objects_x = bounds_objects.size.x;
        int bounds_objects_y = bounds_objects.size.y;

        TileBase[] allTiles_objects = tMObjects.GetTilesBlock(bounds_objects);

        // Top
        BoundsInt bounds_top = tMTop.cellBounds;
        int bounds_top_x = bounds_top.size.x;
        int bounds_top_y = bounds_top.size.y;

        TileBase[] allTiles_top = tMTop.GetTilesBlock(bounds_top);

        SaveObject saveObject = new SaveObject
        {
            // Ground
            bounds_ground_x = bounds_ground_x,
            bounds_ground_y = bounds_ground_y,
            tile_list_ground = new List<TileBase>(allTiles_ground),

            // Wall
            bounds_wall_x = bounds_wall_x,
            bounds_wall_y = bounds_wall_y,
            tile_list_wall = new List<TileBase>(allTiles_wall),

            // Objects
            bounds_objects_x = bounds_objects_x,
            bounds_objects_y = bounds_objects_y,
            tile_list_objects = new List<TileBase>(allTiles_objects),

            // Top
            bounds_top_x = bounds_top_x,
            bounds_top_y = bounds_top_y,
            tile_list_top = new List<TileBase>(allTiles_top)
        };

        string json = JsonUtility.ToJson(saveObject);
        SaveSystem.Save(json, folder);
    }

    public static void Clear_Whole_Map(Tilemap tMGround, Tilemap tMWall, Tilemap tMObjects, Tilemap tMTop)
    {
        Clear_Layer(tMGround);
        Clear_Layer(tMWall);
        Clear_Layer(tMObjects);
        Clear_Layer(tMTop);
    }

    public static void Clear_Layer(Tilemap layer_to_clear)
    {
        BoundsInt bounds_layer = layer_to_clear.cellBounds;
        for (int x = 0; x < bounds_layer.size.x; x++)
        {
            for (int y = 0; y < bounds_layer.size.y; y++)
            {
                layer_to_clear.SetTile(new Vector3Int(x, y, 0), null);
            }
        }
    }

    public static TileBase Get_Tile_Type(Vector3 pos, Tilemap map)
    {
        return map.GetTile(map.WorldToCell(pos));
    }

    public class SaveObject
    {
        // Ground
        public int bounds_ground_x;
        public int bounds_ground_y;
        public List<TileBase> tile_list_ground;

        // Wall
        public int bounds_wall_x;
        public int bounds_wall_y;
        public List<TileBase> tile_list_wall;

        // Objects
        public int bounds_objects_x;
        public int bounds_objects_y;
        public List<TileBase> tile_list_objects;

        // Top
        public int bounds_top_x;
        public int bounds_top_y;
        public List<TileBase> tile_list_top;
    }
}
