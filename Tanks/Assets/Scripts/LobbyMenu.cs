using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyMenu : MonoBehaviour
{
    public void StartLocalGame()
    {
        SceneManager.LoadScene("LocalGameScene");
        Cursor.visible = false;
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
