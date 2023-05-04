using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class UI_ConnectionResponseMessage : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI _txtMessage;
    [SerializeField] private Button _btnClose;

    private void Awake()
    {
        _btnClose.onClick.AddListener(() =>
        {
            Hide();
        });
    }
    private void Start()
    {
        KitchenGameMultiplayer.Instance.OnFailedToJoinGame += KitchenGameMultiplayerInstance_OnFailedToJoinGame;
        Hide();
    }

    private void KitchenGameMultiplayerInstance_OnFailedToJoinGame(object sender, System.EventArgs e)
    {
        Show();
        _txtMessage.text = NetworkManager.Singleton.DisconnectReason;

        if (_txtMessage.text == "")
        {
            _txtMessage.text = "Failed to connect!!";
        }
    }


    private void Hide()
    {
        gameObject.SetActive(false);
    }
    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        KitchenGameMultiplayer.Instance.OnFailedToJoinGame -= KitchenGameMultiplayerInstance_OnFailedToJoinGame;
    }
}
