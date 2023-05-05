using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class UI_LobbyMessage : MonoBehaviour
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
        KitchenGameLobby.Instance.OnCreateLobbyStarted += KitchenGameLobby_Instance_OnCreateLobbyStarted;
        KitchenGameLobby.Instance.OnCreateLobbyFailed += KitchenGameLobby_Instance_OnCreateLobbyFailed;
        KitchenGameLobby.Instance.OnJoinStarted += KitchenGameLobby_Instance_OnJoinStarted;
        KitchenGameLobby.Instance.OnJoinFailed += KitchenGameLobby_Instance_OnJoinFailed;
        KitchenGameLobby.Instance.OnQuickJoinFailed += KitchenGameLobby_Instance_OnQuickJoinFailed;

        Hide();
    }

    private void KitchenGameLobby_Instance_OnQuickJoinFailed(object sender, System.EventArgs e)
    {
        ShowMessage("Could not find a lobby for quick join...");

    }

    private void KitchenGameLobby_Instance_OnJoinFailed(object sender, System.EventArgs e)
    {
        ShowMessage("Failed to join lobby!");

    }

    private void KitchenGameLobby_Instance_OnJoinStarted(object sender, System.EventArgs e)
    {
        ShowMessage("Joining Lobby...");

    }

    private void KitchenGameLobby_Instance_OnCreateLobbyFailed(object sender, System.EventArgs e)
    {
        ShowMessage("Failed to create a lobby!");
    }

    private void KitchenGameLobby_Instance_OnCreateLobbyStarted(object sender, System.EventArgs e)
    {
        ShowMessage("Creating Lobby...");
    }

    private void KitchenGameMultiplayerInstance_OnFailedToJoinGame(object sender, System.EventArgs e)
    {
        if (NetworkManager.Singleton.DisconnectReason == "")
        {
            ShowMessage("Failed to connect");
        }
        else
        {
            ShowMessage(NetworkManager.Singleton.DisconnectReason);
        }
    }

    private void ShowMessage(string message)
    {
        Show();
        _txtMessage.text = message;
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
