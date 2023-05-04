using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_TestingCharacterSelection : MonoBehaviour
{
    [SerializeField] private Button _btnReady;

    private void Awake()
    {
        _btnReady.onClick.AddListener(() =>
        {            
           CharacterSelectionReady.Instance.SetPlayerReady();
        });
    }

    
}
