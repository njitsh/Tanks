using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void LocalGameLobby()
    {
        SceneManager.LoadScene("LocalLobbyScene");
    }

    public void OnlineGame()
    {
        SceneManager.LoadScene("OnlineGameScene");
    }

    public void MapEditor()
    {
        SceneManager.LoadScene("MapEditorScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
