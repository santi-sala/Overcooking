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

    [SerializeField] private Button _moveUpButton;
    [SerializeField] private Button _moveDownButton;
    [SerializeField] private Button _moveLeftButton;
    [SerializeField] private Button _moveRightButton;
    [SerializeField] private Button _moveInteractButton;
    [SerializeField] private Button _moveInteractAlternateButton;
    [SerializeField] private Button _movePauseButton;

    [SerializeField] private TextMeshProUGUI _moveUpText;
    [SerializeField] private TextMeshProUGUI _moveDownText;
    [SerializeField] private TextMeshProUGUI _moveLeftText;
    [SerializeField] private TextMeshProUGUI _moveRightText;
    [SerializeField] private TextMeshProUGUI _moveInteractText;
    [SerializeField] private TextMeshProUGUI _moveInteractAlternateText;
    [SerializeField] private TextMeshProUGUI _movePauseText;

    [SerializeField] private Transform _pressToRebindKeyTransform;




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

        //Listening to change for Keybindings
        _moveUpButton.onClick.AddListener(() =>
        {
            RebindKeyBinding(GameInput.Bindings.Move_Up);
        });
        _moveDownButton.onClick.AddListener(() =>
        {
            RebindKeyBinding(GameInput.Bindings.Move_Down);
        });
        _moveLeftButton.onClick.AddListener(() =>
        {
            RebindKeyBinding(GameInput.Bindings.Move_Left);
        });
        _moveRightButton.onClick.AddListener(() =>
        {
            RebindKeyBinding(GameInput.Bindings.Move_Right);
        });
        _moveInteractButton.onClick.AddListener(() =>
        {
            RebindKeyBinding(GameInput.Bindings.Interact);
        });
        _moveInteractAlternateButton.onClick.AddListener(() =>
        {
            RebindKeyBinding(GameInput.Bindings.Interact_Alternate);
        });
        _movePauseButton.onClick.AddListener(() =>
        {
            RebindKeyBinding(GameInput.Bindings.Pause);
        });
    }

    private void Start()
    {
        GameManager.Instance.OnGameResume += GameManager_Instance_OnGameResume;

        UpdateVisualSfx();
        UpdateVisualMusic();
        UpdateVisualKeyBinding();

        HidePressToRebindKey();
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

    private void UpdateVisualKeyBinding()
    {
        _moveUpText.text = GameInput.Instance.GetBindingText(GameInput.Bindings.Move_Up);
        _moveDownText.text = GameInput.Instance.GetBindingText(GameInput.Bindings.Move_Down);
        _moveLeftText.text = GameInput.Instance.GetBindingText(GameInput.Bindings.Move_Left);
        _moveRightText.text = GameInput.Instance.GetBindingText(GameInput.Bindings.Move_Right);
        _moveInteractText.text = GameInput.Instance.GetBindingText(GameInput.Bindings.Interact);
        _moveInteractAlternateText.text = GameInput.Instance.GetBindingText(GameInput.Bindings.Interact_Alternate);
        _movePauseText.text = GameInput.Instance.GetBindingText(GameInput.Bindings.Pause);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);       
    }

    private void ShowPressToRebindKey()
    {
        _pressToRebindKeyTransform.gameObject.SetActive(true);
    }

    private void HidePressToRebindKey()
    {
        _pressToRebindKeyTransform.gameObject.SetActive(false);
    }

    private void RebindKeyBinding(GameInput.Bindings binding)
    {
        ShowPressToRebindKey();
        GameInput.Instance.RebindBinding(binding,() => {
            HidePressToRebindKey();
            UpdateVisualKeyBinding();
            });
    }
}
