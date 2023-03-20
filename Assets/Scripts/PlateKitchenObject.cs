using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;
    public class OnIngredientAddedEventArgs : EventArgs
    {
        public SO_KitchenObjects kitchenObjectSOArgs;
    }

    [SerializeField] private List<SO_KitchenObjects> _validKitchenObjectSOIngredientList;
    
    private List<SO_KitchenObjects> _kitchenObjectSOList;

    private void Awake()
    {
        _kitchenObjectSOList = new List<SO_KitchenObjects>();
    }
    public bool TryAddIngredient(SO_KitchenObjects kitchenObjectSO)
    {
        if (!_validKitchenObjectSOIngredientList.Contains(kitchenObjectSO))
        {
            // Not a valid ingredient
            //Debug.Log("Not a valid ingrdient");
            return false;
        }
        if (_kitchenObjectSOList.Contains(kitchenObjectSO))
        {
            // Ingredient already exists in the plate (No duplicates!!)
            //Debug.Log("Ingredient already in plate");
            return false;
        }
        else
        {
            // Add ingredient in the list
            _kitchenObjectSOList.Add(kitchenObjectSO);
            //Debug.Log("Ingredient added to the plate");

            OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs
            {
                kitchenObjectSOArgs = kitchenObjectSO
            });
            return true;
        }
    }

    public List<SO_KitchenObjects> GetKitchenObjectSOList()
    {
        return _kitchenObjectSOList;
    }
}
