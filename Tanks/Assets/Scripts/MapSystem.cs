using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class MapSystem
{
    public static void Load_Map(Tilemap tMGround, Tilemap tMWall, Tilemap tMObjects, Tilemap tMTop, int folder)
    {
        string mapString = SaveSystem.Load(folder);
        if (mapString != null)
        {
            SaveObject saveObject = JsonUtility.FromJson<SaveObject>(mapString);

            Open_Map(tMGround, saveObject.tile_list_ground.ToArray(), saveObject.bounds_ground_x, saveObject.bounds_ground_y, tMWall, saveObject.tile_list_wall.ToArray(), saveObject.bounds_wall_x, saveObject.bounds_wall_y, tMObjects, saveObject.tile_list_objects.ToArray(), saveObject.bounds_objects_x, saveObject.bounds_objects_y, tMTop, saveObject.tile_list_top.ToArray(), saveObject.bounds_top_x, saveObject.bounds_top_y);
        }
        else Debug.Log("No map was loaded!");
    }

    private static void Open_Map(Tilemap tMGround, TileBase[] map_ground, int bounds_g_x, int bounds_g_y, Tilemap tMWall, TileBase[] map_wall, int bounds_w_x, int bounds_w_y, Tilemap tMObjects, TileBase[] map_objects, int bounds_o_x, int bounds_o_y, Tilemap tMTop, TileBase[] map_top, int bounds_t_x, int bounds_t_y)
    {
        if (map_ground.Length != 0)
        {
            for (int x = 0; x < bounds_g_x; x++)
            {
                for (int y = 0; y < bounds_g_y; y++)
                {
                    tMGround.SetTile(new Vector3Int(x, y, 0), map_ground[x + y * bounds_g_x]);
                }
            }
        }
        if (map_wall.Length != 0)
        {
            for (int x = 0; x < bounds_w_x; x++)
            {
                for (int y = 0; y < bounds_w_y; y++)
                {
                    tMWall.SetTile(new Vector3Int(x, y, 0), map_wall[x + y * bounds_w_x]);
                }
            }
        }
        if (map_objects.Length != 0)
        {
            for (int x = 0; x < bounds_o_x; x++)
            {
                for (int y = 0; y < bounds_o_y; y++)
                {
                    tMObjects.SetTile(new Vector3Int(x, y, 0), map_objects[x + y * bounds_o_x]);
                }
            }
        }
        if (map_top.Length != 0)
        {
            for (int x = 0; x < bounds_t_x; x++)
            {
                for (int y = 0; y < bounds_t_y; y++)
                {
                    tMTop.SetTile(new Vector3Int(x, y, 0), map_top[x + y * bounds_t_x]);
                }
            }
        }
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
