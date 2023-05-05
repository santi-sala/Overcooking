using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_LobbyCreate : MonoBehaviour
{
    [SerializeField] private Button _btnClose;
    [SerializeField] private Button _btnPrivateLobby;
    [SerializeField] private Button _btnPublicLobby;
    [SerializeField] private TMP_InputField _inputFieldLobbyName;

    private void Awake()
    {
        _btnClose.onClick.AddListener(() =>
        {
            Hide();
        });
        _btnPublicLobby.onClick.AddListener(() =>
        {
            KitchenGameLobby.Instance.CreateLobby(_inputFieldLobbyName.text, false);
        });
        _btnPrivateLobby.onClick.AddListener(() =>
        {
            KitchenGameLobby.Instance.CreateLobby(_inputFieldLobbyName.text, true);
        });
    }

    private void Start()
    {
        Hide();
    }

    public void Show()
    {
        gameObject.SetActive(true);

        _btnPublicLobby.Select();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
