using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scoreboard : MonoBehaviour
{
    public GameObject scoreboardUI;
    public GameObject gameManager;

    public GameObject scoreboardStat;
    public List<GameObject> scoreboardStatList;

    int statListSize = 100;

    public GameObject pauseMenuUI;

    public static bool scoreboardIsEnabled = false;

    public void ShowScoreboard()
    {
        pauseMenuUI.SetActive(false);
        scoreboardUI.SetActive(true);
        Time.timeScale = 0f;
        PauseMenu.GameIsPaused = true;
        Cursor.visible = true;
        scoreboardIsEnabled = true;
        SetScoreboard();
    }

    /*public void HideScoreboard()
    {
        scoreboardUI.SetActive(false);
        Time.timeScale = 1f;
        PauseMenu.GameIsPaused = false;
        Cursor.visible = false;
        scoreboardIsEnabled = false;
    }*/

    public void SetScoreboard()
    {
        scoreboardStatList.Clear();
        for (int i = 0; i < gameManager.GetComponent<GameManager>().playingPlayers; i++)
        {
            scoreboardStatList.Add(Instantiate(scoreboardStat, scoreboardUI.transform.GetChild(0)));
            scoreboardStatList[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText("Player " + (i + 1)); // Set player name
            scoreboardStatList[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().SetText("K/D " + gameManager.GetComponent<GameManager>().allplayers[i].GetComponent<PlayerController>().kills + "/" + gameManager.GetComponent<GameManager>().allplayers[i].GetComponent<PlayerController>().deaths); // Set kill amount
            scoreboardUI.transform.GetChild(0).GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, statListSize * (i + 1));
        }
    }

    public void Next()
    {
        scoreboardUI.SetActive(false);
        Time.timeScale = 1f;
        PauseMenu.GameIsPaused = false;
        scoreboardIsEnabled = false;
        SceneManager.LoadScene("MenuScene");
    }
}
