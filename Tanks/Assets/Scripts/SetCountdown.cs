using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCountdown : MonoBehaviour
{
    public GameManager gameManager;

    public void SetCountDownNow()
    {
        PauseMenu.GameIsPaused = false;
    }

    public void CountDownDone()
    {
        GameManager.countDownDone = true;
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        GameManager.countDownDone = false;
        PauseMenu.GameIsPaused = true;
    }
}
