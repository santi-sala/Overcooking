using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_CharacvterColorSelection : MonoBehaviour
{
    [SerializeField] private int _colorId;
    [SerializeField] private Image _image;
    [SerializeField] private GameObject _selelectedGameObject;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            KitchenGameMultiplayer.Instance.ChangePlayerColor(_colorId);
        });
    }

    private void Start()
    {
        KitchenGameMultiplayer.Instance.OnPlayerDataNetworkListChanged += KitchenGameMultiplayer_Instance_OnPlayerDataNetworkListChanged;
       _image.color = KitchenGameMultiplayer.Instance.GetPlayerColor(_colorId);
        UpdateIsSelected();
    }

    private void KitchenGameMultiplayer_Instance_OnPlayerDataNetworkListChanged(object sender, System.EventArgs e)
    {
        UpdateIsSelected();
    }

    private void UpdateIsSelected()
    {
        if (KitchenGameMultiplayer.Instance.GetplayerData().colorId == _colorId)
        {
            _selelectedGameObject.SetActive(true);
        }
        else
        {
            _selelectedGameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        KitchenGameMultiplayer.Instance.OnPlayerDataNetworkListChanged -= KitchenGameMultiplayer_Instance_OnPlayerDataNetworkListChanged;

    }
}
