using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class UI_Lobby : MonoBehaviour
{
    [SerializeField] private Button _btnMainMenu;
    [SerializeField] private Button _btnCreateLobby;
    [SerializeField] private Button _btnQuickJoin;
    [SerializeField] private Button _btnJoinCode;
    [SerializeField] private TMP_InputField _inputFieldJoinCode;
    [SerializeField] private TMP_InputField _inputFieldPlayerName;
    [SerializeField] private UI_LobbyCreate _createLobby;
    [SerializeField] private Transform _lobbyContainer;
    [SerializeField] private Transform _lobbyTemplate;


    private void Awake()
    {
        _btnMainMenu.onClick.AddListener(() =>
        {
            KitchenGameLobby.Instance.LeaveLobby();
            Loader.Load(Loader.Scene.MainMenuScene);
        });

        _btnCreateLobby.onClick.AddListener(() =>
        {
            _createLobby.Show();
        });

        _btnQuickJoin.onClick.AddListener(() =>
        {
            KitchenGameLobby.Instance.QuickJoin();
        });

        _btnJoinCode.onClick.AddListener(() =>
        {
            KitchenGameLobby.Instance.JoinWithCode(_inputFieldJoinCode.text);
        });

        _lobbyTemplate.gameObject.SetActive(false);
    }

    private void Start()
    {
        _inputFieldPlayerName.text = KitchenGameMultiplayer.Instance.GetPlayerName();
        _inputFieldPlayerName.onValueChanged.AddListener((string newPlayerName) =>
        {
            KitchenGameMultiplayer.Instance.SetPlayername(newPlayerName);
        });
        KitchenGameLobby.Instance.OnLobbyListChanged += KitchenGameLobby_Instance_OnLobbyListChanged;
        UpdateLobbyList(new List<Lobby>());
    }

    private void KitchenGameLobby_Instance_OnLobbyListChanged(object sender, KitchenGameLobby.OnLobbyListChangedEventArgs e)
    {
        UpdateLobbyList(e.lobbyList);
    }

    private void UpdateLobbyList(List<Lobby> lobbyList)
    {
        foreach (Transform child in _lobbyContainer)
        {
            if (child == _lobbyTemplate)
            {
                continue;
            }
            Destroy(child.gameObject);
        }

        foreach (Lobby lobby in lobbyList)
        {
            Transform lobbyTransform = Instantiate(_lobbyTemplate, _lobbyContainer);
            lobbyTransform.gameObject.SetActive(true);
            lobbyTransform.GetComponent<UI_LobbyListSingle>().SetLobby(lobby);
        }
    }
}
