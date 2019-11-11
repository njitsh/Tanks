using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    public GameObject Level_Block;
    public List<GameObject> LevelBlockList;

    public string level_path;

    int blockSpacing = 110;
    // int maxFileNameLength = 10; TODO ADD MAX FILENAME LENGTH

    void Start()
    {
        FileInfo[] mapFiles = SaveSystem.GetSavesFromFolder(0);
        int i = 0;
        foreach (FileInfo mapFile in mapFiles)
        {
            LevelBlockList.Add(Instantiate(Level_Block, transform));
            string fileName = Path.GetFileNameWithoutExtension(mapFile.ToString());
            LevelBlockList[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText(fileName);
            LevelBlockList[i].transform.position -= new Vector3(0, blockSpacing * i, 0);
            LevelBlockList[i].GetComponent<Button>().onClick.AddListener(() => setSelectedLevelPath(mapFile.ToString()));
            i++;
        }
    }

    void setSelectedLevelPath(string filepath)
    {
        level_path = filepath;
        GameObject.Find("LobbyCanvas").GetComponent<LobbyMenu>().StartLocalGame();
    }
}
