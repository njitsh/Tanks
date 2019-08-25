using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject optionsMenu;

    /*public int selectedButton = 0;

    private float nextButtonChange;
    private float buttonChangeRate = 0.1f;*/

    void Update()
    {
        for (int i = 1; i <= 4; i++)
        {
            /*if ((Input.GetAxisRaw("J" + i + "Vertical") >= 0.8 || Input.GetAxisRaw("J5Vertical") >= 0.8) && Time.time > nextButtonChange)
            {
                if (selectedButton > 0) selectedButton -= 1;
                else selectedButton = 4;
                nextButtonChange = Time.time + buttonChangeRate;
            }
            else if ((Input.GetAxisRaw("J" + i + "Vertical") <= -0.8 || Input.GetAxisRaw("J5Vertical") <= -0.8) && Time.time > nextButtonChange)
            {
                if (selectedButton < 4) selectedButton += 1;
                else selectedButton = 0;
                nextButtonChange = Time.time + buttonChangeRate;
            }*/
            if (optionsMenu.activeSelf && (Input.GetButtonDown("J" + i + "B") || Input.GetButton("J5B")))
            {
                optionsMenu.SetActive(false);
                mainMenu.SetActive(true);
                //selectedButton = 0;
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
