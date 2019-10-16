using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveSystem
{
    public static readonly string[] MAP_FOLDER = { Application.dataPath + "/Maps/Editor/", Application.dataPath + "/Maps/Local/" }; // 0. Editor 1. Local

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
    public static string Load(int folder)
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(MAP_FOLDER[folder]);
        FileInfo[] mapFiles = directoryInfo.GetFiles("*.json");
        FileInfo mostRecentFile = null;
        foreach (FileInfo fileInfo in mapFiles)
        {
            if (mostRecentFile == null)
            {
                mostRecentFile = fileInfo;
            }
            else
            {
                if (fileInfo.LastWriteTime > mostRecentFile.LastWriteTime)
                {
                    mostRecentFile = fileInfo;
                }
            }
        }

        if (mostRecentFile != null)
        {
            string mapString = File.ReadAllText(mostRecentFile.FullName);
            return mapString;
        }
        else
        {
            return null;
        }
    }
}
