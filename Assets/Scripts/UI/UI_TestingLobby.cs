using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_TestingLobby : MonoBehaviour
{
    [SerializeField] private Button _btnCreateGame;
    [SerializeField] private Button _btnJoinGame;


    private void Awake()
    {
        _btnCreateGame.onClick.AddListener(() =>
        {
            KitchenGameMultiplayer.Instance.StartHost();

            Loader.LoadNetwork(Loader.Scene.CharacterSelectionScene);
        });

        _btnJoinGame.onClick.AddListener(() =>
        {
            KitchenGameMultiplayer.Instance.StartClient();
        });
    }
}
