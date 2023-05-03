using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_WaitingForPlayers : MonoBehaviour
{

    private void Start()
    {
        GameManager.Instance.OnLocalPlayerReadyChanged += GameManager_Instance_OnLocalPlayerReadyChanged;
        GameManager.Instance.OnStateChange += GameManager_Instance_OnStateChange;
        Hide();
    }

    private void GameManager_Instance_OnStateChange(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsCountdownToStartActive())
        {
            Hide();
        }
    }

    private void GameManager_Instance_OnLocalPlayerReadyChanged(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsLocalPLayerReady())
        {
            Show();
        }
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
