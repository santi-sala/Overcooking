using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_GamePause : MonoBehaviour
{
    public static UI_GamePause Instance { get; private set; }

    [SerializeField] private Button _resumeButton;
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private Button _mainOptionsButton;

    private void Start()
    {
        GameManager.Instance.OnGamePaused += GameManager_Instance_OnGamePaused;
        GameManager.Instance.OnGameResume += GameManager_Instance_OnGameResume;

        Hide();
    }

    private void Awake()
    {
        Instance = this;

        _resumeButton.onClick.AddListener(() =>
        {
            GameManager.Instance.TogglePauseGame();
        });

        _mainMenuButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.MainMenuScene);
        });

        _mainOptionsButton.onClick.AddListener(() =>
        {
            UI_Options.Instance.Show();
            Hide();
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

    public void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
