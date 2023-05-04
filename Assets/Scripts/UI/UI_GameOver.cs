using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class UI_GameOver : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _recipesDeliveredText;
    [SerializeField] private Button _playAgainButton;

    private void Awake()
    {
        _playAgainButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.Shutdown();
            Loader.Load(Loader.Scene.GameScene);
        });
    }

    private void Start()
    {
        GameManager.Instance.OnStateChange += GameManager_Instance_OnStateChange;
             
        Hide();
    }

    private void GameManager_Instance_OnStateChange(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsGameOver())
        {
            Show();
            _recipesDeliveredText.text = DeliveryManager.Instance.GetSuccesfulRecipesAmount().ToString();
        }
        else
        {
            Hide();
        }
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Show()
    {
        gameObject.SetActive(true);
        _playAgainButton.Select();
    }

    /*
    private void PlayAgain()
    {
        NetworkManager.Singleton.Shutdown();
        Loader.Load(Loader.Scene.GameScene);
    }
    */
}
