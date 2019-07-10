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
        ControllerPlayerBinding.TempControllerBinding();
        SceneManager.LoadScene("LocalGameScene");
        Cursor.visible = false;
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
