using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class UI_TestingNetcode : MonoBehaviour
{
    [SerializeField] private Button _startHostBtn;
    [SerializeField] private Button _startClientBtn;

    private void Awake()
    {
        _startHostBtn.onClick.AddListener(() =>
        {
            Debug.Log("HOST STARTED");
            NetworkManager.Singleton.StartHost();
            Hide();
        });

        _startClientBtn.onClick.AddListener(() =>
        {
            Debug.Log("CLIENT STARTED");
            NetworkManager.Singleton.StartClient();
            Hide();
        });
    }

    private void Hide()
    {
        gameObject.SetActive(false);  
    }
}
