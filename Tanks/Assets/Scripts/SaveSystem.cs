using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveSystem
{
    public static readonly string MAP_FOLDER_EDITOR = Application.dataPath + "/Maps/Editor/";
    public static readonly string MAP_FOLDER_LOCAL = Application.dataPath + "/Maps/Local/";

    public static void Init()
    {
        // Test if map folder exists
        if (!Directory.Exists(MAP_FOLDER_EDITOR))
        {
            // Create map folder
            Directory.CreateDirectory(MAP_FOLDER_EDITOR);
        }

        // Test if map folder exists
        if (!Directory.Exists(MAP_FOLDER_LOCAL))
        {
            // Create map folder
            Directory.CreateDirectory(MAP_FOLDER_LOCAL);
        }
    }

    // SAVE MAP
    public static void Save(string mapString)
    {
        int mapNumber = 1;
        while (File.Exists(MAP_FOLDER_EDITOR + "map_" + mapNumber + ".json"))
        {
            mapNumber++;
        }
        File.WriteAllText(MAP_FOLDER_EDITOR + "map_" + mapNumber + ".json", mapString);
    }

    // LOAD MAP
    public static string Load()
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(MAP_FOLDER_EDITOR);
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
