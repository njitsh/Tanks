using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class MapSystem
{
    public static void Load_Map(Tilemap tMGround, Tilemap tMObjects, int folder)
    {
        string mapString = SaveSystem.Load(folder);
        if (mapString != null)
        {
            SaveObject saveObject = JsonUtility.FromJson<SaveObject>(mapString);

            Open_Map(tMGround, saveObject.tile_list_ground.ToArray(), saveObject.bounds_ground_x, saveObject.bounds_ground_y, tMObjects, saveObject.tile_list_objects.ToArray(), saveObject.bounds_objects_x, saveObject.bounds_objects_y);
        }
        else Debug.Log("No map was loaded!");
    }

    private static void Open_Map(Tilemap tMGround, TileBase[] map_ground, int bounds_g_x, int bounds_g_y, Tilemap tMObjects, TileBase[] map_objects, int bounds_o_x, int bounds_o_y)
    {
        if (tMGround != null)
        {
            for (int x = 0; x < bounds_g_x; x++)
            {
                for (int y = 0; y < bounds_g_y; y++)
                {
                    tMGround.SetTile(new Vector3Int(x, y, 0), map_ground[x + y * bounds_g_x]);
                }
            }
        }
        if (tMObjects != null)
        {
            for (int x = 0; x < bounds_o_x; x++)
            {
                for (int y = 0; y < bounds_o_y; y++)
                {
                    tMObjects.SetTile(new Vector3Int(x, y, 0), map_objects[x + y * bounds_o_x]);
                }
            }
        }
    }

    public static void Save_Map(Tilemap tMGround, Tilemap tMObjects, int folder)
    {
        // Get bounds
        BoundsInt bounds_ground = tMGround.cellBounds;
        int bounds_ground_x = bounds_ground.size.x;
        int bounds_ground_y = bounds_ground.size.y;

        TileBase[] allTiles_ground = tMGround.GetTilesBlock(bounds_ground);

        BoundsInt bounds_objects = tMObjects.cellBounds;
        int bounds_objects_x = bounds_objects.size.x;
        int bounds_objects_y = bounds_objects.size.y;

        TileBase[] allTiles_objects = tMObjects.GetTilesBlock(bounds_objects);

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
        SaveSystem.Save(json, folder);
    }

    public static void Clear_Whole_Map(Tilemap tMGround, Tilemap tMObjects)
    {
        Clear_Layer(tMGround);
        Clear_Layer(tMObjects);
    }

    public static void Clear_Layer(Tilemap layer_to_clear)
    {
        BoundsInt bounds_objects = layer_to_clear.cellBounds;
        for (int x = 0; x < bounds_objects.size.x; x++)
        {
            for (int y = 0; y < bounds_objects.size.y; y++)
            {
                layer_to_clear.SetTile(new Vector3Int(x, y, 0), null);
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
