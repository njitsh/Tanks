using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = true;

    public GameObject pauseMenuUI;
    
    void Update()
    {
        for (int i = 1; i <= 4; i++)
        {
            if (Input.GetButtonDown("J" + i + "Start"))
            {
                if (GameIsPaused)
                {
                    Resume();
                }
                else
                {
                    Pause();
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape) && !Scoreboard.scoreboardIsEnabled)
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        Cursor.visible = false;
    }

    public void Pause() 
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        Cursor.visible = true;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        pauseMenuUI.SetActive(false);
        GameIsPaused = false;
        SceneManager.LoadScene("MenuScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
