using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Tutorial : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _keyboardMoveUpText;
    [SerializeField] private TextMeshProUGUI _keyboardMoveRightText;
    [SerializeField] private TextMeshProUGUI _keyboardMoveDownText;
    [SerializeField] private TextMeshProUGUI _keyboardMoveLeftText;
    [SerializeField] private TextMeshProUGUI _keyboardInteractText;
    [SerializeField] private TextMeshProUGUI _keyboardInteractAlternateText;
    [SerializeField] private TextMeshProUGUI _keyboardPauseText;

    [SerializeField] private TextMeshProUGUI _gamepadInteractText;
    [SerializeField] private TextMeshProUGUI _gamepadInteractAlternateText;
    [SerializeField] private TextMeshProUGUI _gamepadPauseText;

    private void Start()
    {
        GameInput.Instance.OnBindingRebind += GameInput_Instance_OnBindingRebind;
        GameManager.Instance.OnStateChange += GameManager_Instance_OnStateChange;

        UpdateVisual();

        Show();
    }

    private void GameManager_Instance_OnStateChange(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsCountdownToStartActive())
        {
            Hide();
        }
    }

    private void GameInput_Instance_OnBindingRebind(object sender, System.EventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        _keyboardMoveUpText.text = GameInput.Instance.GetBindingText(GameInput.Bindings.Move_Up);
        _keyboardMoveDownText.text = GameInput.Instance.GetBindingText(GameInput.Bindings.Move_Down);
        _keyboardMoveLeftText.text = GameInput.Instance.GetBindingText(GameInput.Bindings.Move_Left);
        _keyboardMoveRightText.text = GameInput.Instance.GetBindingText(GameInput.Bindings.Move_Right);
        _keyboardInteractText.text = GameInput.Instance.GetBindingText(GameInput.Bindings.Interact);
        _keyboardInteractAlternateText.text = GameInput.Instance.GetBindingText(GameInput.Bindings.Interact_Alternate);
        _keyboardPauseText.text = GameInput.Instance.GetBindingText(GameInput.Bindings.Pause);

        // Gamepad
        _gamepadInteractText.text = GameInput.Instance.GetBindingText(GameInput.Bindings.Gamepad_Interact);
        _gamepadInteractAlternateText.text = GameInput.Instance.GetBindingText(GameInput.Bindings.Gamepad_InteractAlternate);
        _gamepadPauseText.text = GameInput.Instance.GetBindingText(GameInput.Bindings.Gamepad_Pause);
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
