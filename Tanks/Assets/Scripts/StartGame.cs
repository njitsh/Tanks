using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
using System;

public class StartGame : MonoBehaviour
{

    public Button StartButton;
   
    // Start is called before the first frame update
    void Start()
    {
        if (StartButton != null)
        {
            StartButton.onClick.AddListener(LoadMainScene);
        }
        
    }
    void LoadMainScene()
    {
        SceneManager.LoadScene("MainGameScene");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
