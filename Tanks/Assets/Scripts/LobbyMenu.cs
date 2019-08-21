using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyMenu : MonoBehaviour
{
    void Update()
    {
        for (int i = 1; i <= 4; i++)
        {
            if (Input.GetButtonDown("J" + i + "B"))
            {
                //BackToMainMenu();
            }
        }
    }

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
