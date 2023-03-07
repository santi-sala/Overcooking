using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PlateIconSingle : MonoBehaviour
{
    [SerializeField] private Image _image;
    public void SetKitchenObjectSO(SO_KitchenObjects kitchenObjectSO)
    {
        _image.sprite = kitchenObjectSO.GetSprite();
    }
}
