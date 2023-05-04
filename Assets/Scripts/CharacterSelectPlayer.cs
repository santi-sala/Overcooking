using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectPlayer : MonoBehaviour
{
    [SerializeField] private int _playerIndex;
    [SerializeField] private GameObject _readyPlayer;
    [SerializeField] private PlayerVisual _playerVisual;

    

    private void Start()
    {
        KitchenGameMultiplayer.Instance.OnPlayerDataNetworkListChanged += KitchenGameMultiplayer_Instance_OnPlayerDataNetworkListChanged;

        CharacterSelectionReady.Instance.OnPlayerReadyChanged += CharacterSelectionReady_Instance_OnPlayerReadyChanged;

        UpdatePlayer();
    }

    private void CharacterSelectionReady_Instance_OnPlayerReadyChanged(object sender, System.EventArgs e)
    {
        UpdatePlayer();
    }

    private void KitchenGameMultiplayer_Instance_OnPlayerDataNetworkListChanged(object sender, System.EventArgs e)
    {
        UpdatePlayer();
    }

    private void UpdatePlayer()
    {
        if (KitchenGameMultiplayer.Instance.IsPlayerIndexConnected(_playerIndex))
        {
            Show();

            PlayerData playerData = KitchenGameMultiplayer.Instance.GetPlayerDataFromPlayerIndex(_playerIndex);
            _readyPlayer.SetActive(CharacterSelectionReady.Instance.IsPlayerReady(playerData.clientId));
            _playerVisual.SetPlayerColor(KitchenGameMultiplayer.Instance.GetPlayerColor(_playerIndex));
        }
        else
        {
            Hide();
        }
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
