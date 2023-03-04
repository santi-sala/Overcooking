using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class SO_KitchenObjects : ScriptableObject
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private Sprite _sprite;
    [SerializeField] private string _objectName;

    public GameObject GetPrefab()
    {
        return _prefab;
    }

    public string GetObjectName()
    {
        return _objectName;
    }

}
