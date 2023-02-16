using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField] private ClearCounter _clearCounter;
    [SerializeField] private GameObject _visualGameObject;

    private void Start()
    {
        Player.Instance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
    }

    private void Player_OnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e)
    {
        if (e.selectedCounterArg == _clearCounter)
        {
            ShowObject();
        }
        else
        {
            HideObject();
        }
    }

    private void ShowObject()
    {
        _visualGameObject.SetActive(true);
    }

    private void HideObject()
    {
        _visualGameObject.SetActive(false);
    }
}
