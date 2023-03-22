using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField] private BaseCounter _baseCounter;
    [SerializeField] private GameObject[] _visualGameObjectArray;

    private void Start()
    {
        //Player.Instance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
    }

    private void Player_OnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e)
    {
        if (e.selectedCounterArg == _baseCounter)
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
        foreach (GameObject visuaGameObject in _visualGameObjectArray)
        {
            visuaGameObject.SetActive(true);
        }
    }

    private void HideObject()
    {
        foreach (GameObject visuaGameObject in _visualGameObjectArray)
        {
            visuaGameObject.SetActive(false);
        }
    }
}
