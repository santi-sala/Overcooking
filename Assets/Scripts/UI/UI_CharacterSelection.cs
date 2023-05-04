using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class UI_CharacterSelection : MonoBehaviour
{
    [SerializeField] private Button _btnMainMenu;
    [SerializeField] private Button _btnReady;

    private void Awake()
    {
        _btnMainMenu.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.Shutdown();
            Loader.Load(Loader.Scene.MainMenuScene);
        });

        _btnReady.onClick.AddListener(() =>
        {
            CharacterSelectionReady.Instance.SetPlayerReady();
        });
    }
}
