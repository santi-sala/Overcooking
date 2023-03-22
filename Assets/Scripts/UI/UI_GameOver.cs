using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_GameOver : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _recipesDeliveredText;
    [SerializeField] private Button _reloadCurrentSceneBtn;

    
    private void Start()
    {
        GameManager.Instance.OnStateChange += GameManager_Instance_OnStateChange;
        _reloadCurrentSceneBtn.onClick.AddListener(PlayAgain);        
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
        _reloadCurrentSceneBtn.Select();
    }

    private void PlayAgain()
    {
        Loader.Load(Loader.Scene.GameScene);
    }
}
