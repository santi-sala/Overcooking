using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class SO_Recipe : ScriptableObject
{
   [SerializeField] private List<SO_KitchenObjects> _kitchenObjectSOList;
   [SerializeField] private String _recipeName;

    public String GetRecipeName()
    {
        return _recipeName;
    }

    public List<SO_KitchenObjects> GetKitchenObjectSOList()
    {
        return _kitchenObjectSOList;
    }
}

