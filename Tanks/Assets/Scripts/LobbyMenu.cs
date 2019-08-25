using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyMenu : MonoBehaviour
{
    public void StartLocalGame()
    {
        GameObject Player_To_Controller_Assigner = GameObject.Find("Player_To_Controller_Assigner");
        PlayerToControllerAssigner ptocAssigner = Player_To_Controller_Assigner.GetComponent<PlayerToControllerAssigner>();
        if (ptocAssigner.GetPlayerAmount() > 0)
        {
            ControllerPlayerBinding.TempControllerBinding();
            SceneManager.LoadScene("LocalGameScene");
            Cursor.visible = false;
        }
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
