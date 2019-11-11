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

    public class SaveObject
    {
        // Info
        public int aSP; // Number of available spawn points

        // Bounds ground
        public int bgx;
        public int bgy;
        public List<int> tlg;

        // Bounds wall
        public int bwx;
        public int bwy;
        public List<int> tlw;

        // Bounds objects
        public int box;
        public int boy;
        public List<int> tlo;

        // Bounds top
        public int btx;
        public int bty;
        public List<int> tlt;
    }

    public static void Load_Map(Tilemap tMGround, Tilemap tMWall, Tilemap tMObjects, Tilemap tMTop, int folder, TileBase[] ground_tiles, TileBase[] wall_tiles, TileBase[] object_tiles, TileBase[] top_tiles)
    {
        string mapString = SaveSystem.Load(folder);
        if (mapString != null)
        {
            SaveObject saveObject = JsonUtility.FromJson<SaveObject>(mapString);

            Load_Layer(tMGround, Int_List_To_Tilebase(saveObject.tlg, ground_tiles), saveObject.bgx, saveObject.bgy);
            Load_Layer(tMWall, Int_List_To_Tilebase(saveObject.tlw, wall_tiles), saveObject.bwx, saveObject.bwy);
            Load_Layer(tMObjects, Int_List_To_Tilebase(saveObject.tlo, object_tiles), saveObject.box, saveObject.boy);
            Load_Layer(tMTop, Int_List_To_Tilebase(saveObject.tlt, top_tiles), saveObject.btx, saveObject.bty);
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

    public static void Play_Map(Tilemap tMGround, Tilemap tMWall, Tilemap tMObjects, Tilemap tMTop, int folder, tile_to_prefab[] tile_prefab_array, TileBase[] ground_tiles, TileBase[] wall_tiles, TileBase[] object_tiles, TileBase[] top_tiles)
    {
        string mapString = SaveSystem.Load(folder);
        if (mapString != null)
        {
            SaveObject saveObject = JsonUtility.FromJson<SaveObject>(mapString);

            Load_And_Play_Layer(tMGround, Int_List_To_Tilebase(saveObject.tlg, ground_tiles), saveObject.bgx, saveObject.bgy, tile_prefab_array);
            Load_And_Play_Layer(tMWall, Int_List_To_Tilebase(saveObject.tlw, wall_tiles), saveObject.bwx, saveObject.bwy, tile_prefab_array);
            Load_And_Play_Layer(tMObjects, Int_List_To_Tilebase(saveObject.tlo, object_tiles), saveObject.box, saveObject.boy, tile_prefab_array);
            Load_And_Play_Layer(tMTop, Int_List_To_Tilebase(saveObject.tlt, top_tiles), saveObject.btx, saveObject.bty, tile_prefab_array);
        }
        else Debug.Log("No map was loaded!");
    }

    public static void Play_Selected_Map(Tilemap tMGround, Tilemap tMWall, Tilemap tMObjects, Tilemap tMTop, string filepath, tile_to_prefab[] tile_prefab_array, TileBase[] ground_tiles, TileBase[] wall_tiles, TileBase[] object_tiles, TileBase[] top_tiles)
    {
        string mapString = SaveSystem.LoadFromPath(filepath);
        if (mapString != null)
        {
            SaveObject saveObject = JsonUtility.FromJson<SaveObject>(mapString);

            Load_And_Play_Layer(tMGround, Int_List_To_Tilebase(saveObject.tlg, ground_tiles), saveObject.bgx, saveObject.bgy, tile_prefab_array);
            Load_And_Play_Layer(tMWall, Int_List_To_Tilebase(saveObject.tlw, wall_tiles), saveObject.bwx, saveObject.bwy, tile_prefab_array);
            Load_And_Play_Layer(tMObjects, Int_List_To_Tilebase(saveObject.tlo, object_tiles), saveObject.box, saveObject.boy, tile_prefab_array);
            Load_And_Play_Layer(tMTop, Int_List_To_Tilebase(saveObject.tlt, top_tiles), saveObject.btx, saveObject.bty, tile_prefab_array);
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

    public static void Save_Map(Tilemap tMGround, Tilemap tMWall, Tilemap tMObjects, Tilemap tMTop, int folder, TileBase[] ground_tiles, TileBase[] wall_tiles, TileBase[] object_tiles, TileBase[] top_tiles)
    {
        // Set Info
        int available_spawn_points = TileCounter(tMObjects, object_tiles[0]);

        // Get bounds
        // Ground
        BoundsInt bounds_ground = tMGround.cellBounds;
        int bounds_ground_x = bounds_ground.size.x;
        int bounds_ground_y = bounds_ground.size.y;

        List<int> allTiles_ground = TileBase_To_Int_List(tMGround.GetTilesBlock(bounds_ground), ground_tiles);

        // Wall
        BoundsInt bounds_wall = tMWall.cellBounds;
        int bounds_wall_x = bounds_wall.size.x;
        int bounds_wall_y = bounds_wall.size.y;

        List<int> allTiles_wall = TileBase_To_Int_List(tMWall.GetTilesBlock(bounds_wall), wall_tiles);

        // Objects
        BoundsInt bounds_objects = tMObjects.cellBounds;
        int bounds_objects_x = bounds_objects.size.x;
        int bounds_objects_y = bounds_objects.size.y;

        List<int> allTiles_objects = TileBase_To_Int_List(tMObjects.GetTilesBlock(bounds_objects), object_tiles);

        // Top
        BoundsInt bounds_top = tMTop.cellBounds;
        int bounds_top_x = bounds_top.size.x;
        int bounds_top_y = bounds_top.size.y;

        List<int> allTiles_top = TileBase_To_Int_List(tMTop.GetTilesBlock(bounds_top), top_tiles);

        SaveObject saveObject = new SaveObject
        {
            // Info
            aSP = available_spawn_points,

            // Ground
            bgx = bounds_ground_x,
            bgy = bounds_ground_y,
            tlg = new List<int>(allTiles_ground),

            // Wall
            bwx = bounds_wall_x,
            bwy = bounds_wall_y,
            tlw = new List<int>(allTiles_wall),

            // Objects
            box = bounds_objects_x,
            boy = bounds_objects_y,
            tlo = new List<int>(allTiles_objects),

            // Top
            btx = bounds_top_x,
            bty = bounds_top_y,
            tlt = new List<int>(allTiles_top)
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

    // Convert TileBase array to custom int array
    public static List<int> TileBase_To_Int_List(TileBase[] input_tile_array, TileBase[] ordered_tile_array)
    {
        List<int> int_list = new List<int>();
        bool didSomething;

        // Go through all tiles in array
        for (int i = 0; i < input_tile_array.Length; i++)
        {
            didSomething = false;
            for (int j = 0; j < ordered_tile_array.Length; j++)
            {
                // If tile is empty, skip tile
                if (input_tile_array[i] == null) j = ordered_tile_array.Length;

                // If both tiles are the same, take the location number of the tile from ordered_tile_array and store it
                else if (input_tile_array[i] == ordered_tile_array[j])
                {
                    int_list.Add(j + 1);

                    // Go to the end of the for loop
                    j = ordered_tile_array.Length;
                    didSomething = true;
                }
            }
            if (!didSomething) int_list.Add(0);
        }
        return int_list;
    }

    // Convert custom int array to Tilebase array
    public static TileBase[] Int_List_To_Tilebase(List<int> input_tile_list, TileBase[] ordered_tile_array)
    {
        TileBase[] tile_list = new TileBase[input_tile_list.Count];

        // Go through all tiles in list
        for (int i = 0; i < input_tile_list.Count; i++)
        {
            for (int j = 0; j < ordered_tile_array.Length; j++)
            {
                // If tile is empty, skip tile
                if (input_tile_list[i] == 0) j = ordered_tile_array.Length;

                // If the number in the list is the same as the location in an array, get the Tile and add it to the array
                else if (input_tile_list[i] == (j + 1))
                {
                    tile_list[i] = ordered_tile_array[j];

                    j = ordered_tile_array.Length;
                }
            }
        }
        return tile_list;
    }

    public static int TileCounter(Tilemap map_to_count_from, TileBase tile_to_count)
    {
        int amount_of_tiles = 0;
        TileBase[] all_countable_tiles = map_to_count_from.GetTilesBlock(map_to_count_from.cellBounds);

        for (int i = 0; i < all_countable_tiles.Length; i++)
        {
            if (all_countable_tiles[i] == tile_to_count) amount_of_tiles++;
        }

        return amount_of_tiles;
    }
}
