using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
using System;

public class StartGame : MonoBehaviour
{

    public Button NewSceneButton;
    // Start is called before the first frame update
    void Start()
    {

    }
    public void StartMainScene()
    {
      
        SceneManager.LoadScene("MainGameScene");
     
       
    }
    public void StartEditorScene()
    {
        SceneManager.LoadScene("MapEditorScene");

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
