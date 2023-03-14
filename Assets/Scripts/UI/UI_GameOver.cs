using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_GameOver : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _recipesDeliveredText;

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
    }
}
