using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Options : MonoBehaviour
{
    public static UI_Options Instance { get; private set; }

    [SerializeField] private Button _addSfxButton;
    [SerializeField] private Button _removeSfxButton;
    [SerializeField] private Button _addMusicButton;
    [SerializeField] private Button _removeMusicButton;
    [SerializeField] private Button _backButton;
    [SerializeField] private TextMeshProUGUI _sfxText;
    [SerializeField] private TextMeshProUGUI _musicText;

    private void Awake()
    {
        Instance = this;

        _addSfxButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.AddVolumeSfx();
            UpdateVisualSfx();
        });

        _removeSfxButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.RemoveVolumeSfx();
            UpdateVisualSfx();
        });

        _addMusicButton.onClick.AddListener(() =>
        {
            MusicManager.Instance.AddVolumeMusic();
            UpdateVisualMusic();
        });

        _removeMusicButton.onClick.AddListener(() =>
        {
            MusicManager.Instance.RemoveVolumeMusic();
            UpdateVisualMusic();
        });

        _backButton.onClick.AddListener(() =>
        {
            UI_GamePause.Instance.Show();
            Hide();
        });
    }

    private void Start()
    {
        GameManager.Instance.OnGameResume += GameManager_Instance_OnGameResume;

        UpdateVisualSfx();
        UpdateVisualMusic();

        Hide();
    }

    private void GameManager_Instance_OnGameResume(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void UpdateVisualSfx()
    {
        _sfxText.text = "SFX volume: " + Mathf.Round( SoundManager.Instance.GetVolume() * 10).ToString();
    }

    private void UpdateVisualMusic()
    {
        _musicText.text = "Music volume: " + Mathf.Round(MusicManager.Instance.GetVolume() * 10).ToString();
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
