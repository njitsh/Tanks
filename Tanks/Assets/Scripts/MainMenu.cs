using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject optionsMenu;

    void Update()
    {
        for (int i = 1; i <= 4; i++)
        {
            if (Input.GetButtonDown("J" + i + "B") && optionsMenu.activeSelf)
            {
                optionsMenu.SetActive(false);
                mainMenu.SetActive(true);
            }
        }
    }

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
