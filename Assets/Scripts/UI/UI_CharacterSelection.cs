using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class UI_CharacterSelection : MonoBehaviour
{
    [SerializeField] private Button _btnMainMenu;
    [SerializeField] private Button _btnReady;
    [SerializeField] private TextMeshProUGUI _txtLobbyName;
    [SerializeField] private TextMeshProUGUI _txtLobbyCode;

    private void Awake()
    {
        _btnMainMenu.onClick.AddListener(() =>
        {
            KitchenGameLobby.Instance.LeaveLobby();
            NetworkManager.Singleton.Shutdown();
            Loader.Load(Loader.Scene.MainMenuScene);
        });

        _btnReady.onClick.AddListener(() =>
        {
            CharacterSelectionReady.Instance.SetPlayerReady();
        });
    }

    private void Start()
    {
       Lobby lobby = KitchenGameLobby.Instance.GetLobby();

        _txtLobbyName.text = "Lobby Name: " + lobby.Name;
        _txtLobbyCode.text = "Lobby Code: " + lobby.LobbyCode;
    }
}
