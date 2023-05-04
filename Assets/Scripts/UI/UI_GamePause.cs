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
            Hide();
            UI_Options.Instance.Show(Show);
        });
    }

    private void Start()
    {
        GameManager.Instance.OnLocalGamePaused += GameManager_Instance_OnLocalGamePaused;
        GameManager.Instance.OnLocalGameResume += GameManager_Instance_OnLocalGameResume;

        Hide();
    }

    private void GameManager_Instance_OnLocalGameResume(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void GameManager_Instance_OnLocalGamePaused(object sender, System.EventArgs e)
    {
        Show();
    }

    public void Show()
    {
        gameObject.SetActive(true);

        // This will make it highlight so its easier to see in gamepads
        _resumeButton.Select();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
