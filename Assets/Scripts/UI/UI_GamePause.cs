using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_GamePause : MonoBehaviour
{
    [SerializeField] private Button _resumeButton;
    [SerializeField] private Button _mainMenuButton;

    private void Start()
    {
        GameManager.Instance.OnGamePaused += GameManager_Instance_OnGamePaused;
        GameManager.Instance.OnGameResume += GameManager_Instance_OnGameResume;

        Hide();
    }

    private void Awake()
    {
        _resumeButton.onClick.AddListener(() =>
        {
            GameManager.Instance.TogglePauseGame();
        });

        _mainMenuButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.MainMenuScene);
        });
    }
    private void GameManager_Instance_OnGameResume(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void GameManager_Instance_OnGamePaused(object sender, System.EventArgs e)
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
