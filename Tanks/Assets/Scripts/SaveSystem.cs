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
    public static string Load(int folder)
    {
        FileInfo[] mapFiles = GetSavesFromFolder(folder);
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

    public static FileInfo[] GetSavesFromFolder(int folder)
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(MAP_FOLDER[folder]);
        return directoryInfo.GetFiles("*.json").OrderBy(p => p.CreationTime).ToArray();
    }
}
