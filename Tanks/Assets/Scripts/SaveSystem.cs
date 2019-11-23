using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;

public static class SaveSystem
{
    public static readonly string[] MAP_FOLDER = { Application.dataPath + "/Maps/Local/", Application.dataPath + "/Maps/Editor/" }; // 0. Local 1. Editor

    public static void Init()
    {
        for (int i = 0; i < MAP_FOLDER.Length; i++)
        {
            // Test if map folder exists
            if (!Directory.Exists(MAP_FOLDER[i]))
            {
                // Create map folder
                Directory.CreateDirectory(MAP_FOLDER[i]);
            }
        }
    }

    // SAVE MAP
    public static void Save(string mapString, int folder)
    {
        int mapNumber = 1;
        while (File.Exists(MAP_FOLDER[folder] + "map_" + mapNumber + ".json"))
        {
            mapNumber++;
        }
        File.WriteAllText(MAP_FOLDER[folder] + "map_" + mapNumber + ".json", mapString);
    }

    // LOAD MAP
    public static string[] Load(int level_index)
    {
        List<FileInfo> mapFiles = new List<FileInfo>();
        for (int i = 0; i < MAP_FOLDER.Length; i++)
        {
            mapFiles = mapFiles.Union(GetSavesFromFolder(i)).ToList();
        }

        // Return null if there are no maps available
        if (mapFiles.Count == 0) return null;

        FileInfo chosenFile;

        if (level_index > mapFiles.Count - 1) level_index = 0; // Set index to 0 if index is too heigh
        if (level_index < 0) level_index = Random.Range(0, mapFiles.Count); // Set random level index

        // Set chosen file
        chosenFile = mapFiles[level_index];

        if (chosenFile != null)
        {
            string[] mapString = new string[2];
            mapString[0] = File.ReadAllText(chosenFile.FullName);
            mapString[1] = level_index.ToString();
            return mapString;
        }
        else return null;
    }

    // LOAD MAP
    public static string LoadFromPath(string path)
    {
        string mapString = File.ReadAllText(path);
        return mapString;
    }

    public static List<FileInfo> GetSavesFromFolder(int folder)
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(MAP_FOLDER[folder]);
        return directoryInfo.GetFiles("*.json").OrderBy(p => p.CreationTime).ToList();
    }
}
