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
    int i;

    // int maxFileNameLength = 10; TODO ADD MAX FILENAME LENGTH

    void Start()
    {
        LevelBlockList.Clear();
        listLevels(SaveSystem.GetSavesFromFolder(0));
        listLevels(SaveSystem.GetSavesFromFolder(1));
    }

    void setSelectedLevelPath(string filepath)
    {
        level_path = filepath;
        GameObject.Find("LobbyCanvas").GetComponent<LobbyMenu>().StartLocalGame();
    }

    void listLevels(List<FileInfo> mapFiles)
    {
        foreach (FileInfo mapFile in mapFiles)
        {
            LevelBlockList.Add(Instantiate(Level_Block, transform.GetChild(0).GetChild(0)));
            string fileName = Path.GetFileNameWithoutExtension(mapFile.ToString());
            LevelBlockList[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText(fileName);
            LevelBlockList[i].transform.position -= new Vector3(0, blockSpacing * i + 60, 0);
            transform.GetChild(0).GetChild(0).GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (blockSpacing + 20) * i + 10);
            LevelBlockList[i].GetComponent<Button>().onClick.AddListener(() => setSelectedLevelPath(mapFile.ToString()));
            i++;
        }
    }
}
