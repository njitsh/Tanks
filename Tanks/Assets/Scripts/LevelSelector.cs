using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class LevelSelector : MonoBehaviour
{
    public GameObject Level_Block;
    public List<GameObject> LevelBlockList;

    // Start is called before the first frame update
    void Start()
    {
        FileInfo[] mapFiles = SaveSystem.GetSavesFromFolder(0);
        int i = 0;
        foreach (FileInfo mapFile in mapFiles)
        {
            LevelBlockList.Add(Instantiate(Level_Block, transform));
            LevelBlockList[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().SetText(Path.GetFileNameWithoutExtension(mapFile.ToString()));
            Debug.Log(Path.GetFileNameWithoutExtension(mapFile.ToString()));
            i++;
        }
    }
    // Alive.GetComponent<TextMeshProUGUI>().SetText("Alive " + GameObject.Find("GameManager").GetComponent<GameManager>().alive);

    // Update is called once per frame
    void Update()
    {
        
    }
}
