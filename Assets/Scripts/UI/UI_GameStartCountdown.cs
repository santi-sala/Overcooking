using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_GameStartCountdown : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _countdownText;

    private void Start()
    {
        GameManager.Instance.OnStateChange += GameManager_Instance_OnStateChange;
        Hide();
    }

    private void GameManager_Instance_OnStateChange(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsCountdownToStartActive())
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Update()
    {
        _countdownText.text = MathF.Ceiling(GameManager.Instance.GetCountdownToStartTimer()).ToString();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
}
