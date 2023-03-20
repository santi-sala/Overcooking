using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_DeliveryResult : MonoBehaviour
{
    private const string POPUP = "Popup";

    [SerializeField] private Image _backgroundImage;
    [SerializeField] private Image _iconImage;
    [SerializeField] private TextMeshProUGUI _messageImage;
    [SerializeField] private Color _succesColor;
    [SerializeField] private Color _failedColor;
    [SerializeField] private Sprite _succesSprite;
    [SerializeField] private Sprite _failedSprite;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        DeliveryManager.Instance.OnRecipeSuccess += DeliveryManager_Instance_OnRecipeSuccess;
        DeliveryManager.Instance.OnRecipeFailed += DeliveryManager_Instance_OnRecipeFailed;
        Hide();
    }

    private void DeliveryManager_Instance_OnRecipeFailed(object sender, System.EventArgs e)
    {
        Show();
        _backgroundImage.color = _failedColor;
        _iconImage.sprite = _failedSprite;
        _messageImage.text = "DELIVERY\nFAILED";
        _animator.SetTrigger(POPUP);
    }

    private void DeliveryManager_Instance_OnRecipeSuccess(object sender, System.EventArgs e)
    {
        Show();
        _backgroundImage.color = _succesColor;
        _iconImage.sprite = _succesSprite;
        _messageImage.text = "DELIVERY\nSUCCESS";
        _animator.SetTrigger(POPUP);
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
