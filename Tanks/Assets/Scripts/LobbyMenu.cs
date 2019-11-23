using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyMenu : MonoBehaviour
{
    public GameObject LobbyM;
    public GameObject LevelSelectionM;
    public GameObject PTCA_Object;

    GameObject Player_To_Controller_Assigner;
    PlayerToControllerAssigner ptocAssigner;

    void Start()
    {
        Player_To_Controller_Assigner = GameObject.Find("Player_To_Controller_Assigner");
        ptocAssigner = Player_To_Controller_Assigner.GetComponent<PlayerToControllerAssigner>();
    }

    public void StartLocalGame()
    {
        PTCA_Object.SetActive(true);
        ControllerPlayerBinding.TempControllerBinding();
        SceneManager.LoadScene("LocalGameScene");
        Cursor.visible = false;
    }

    /*
    public void ToLevelSelect()
    {
        if (ptocAssigner.GetPlayerAmount() >= 1)
        {
            PTCA_Object.SetActive(false);
            LobbyM.SetActive(false);
            LevelSelectionM.SetActive(true);
        }
    }
    */

    public void BackToLobby()
    {
        PTCA_Object.SetActive(true);
        LobbyM.SetActive(true);
        LevelSelectionM.SetActive(false);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
