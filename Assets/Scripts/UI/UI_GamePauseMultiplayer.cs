using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_GamePauseMultiplayer : MonoBehaviour
{

    private void Start()
    {
        GameManager.Instance.OnMultiplayerGamePause += GameManager_Instance_OnMultiplayerGamePause;
        GameManager.Instance.OnMultiplayerGameResume += GameManagerInstance_OnMultiplayerGameResume;

        Hide();
    }

    private void GameManagerInstance_OnMultiplayerGameResume(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void GameManager_Instance_OnMultiplayerGamePause(object sender, System.EventArgs e)
    {
        Show();
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
