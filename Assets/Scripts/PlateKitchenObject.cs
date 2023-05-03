using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
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

    // This awake will run first the awake of KitchenObject and then the List.
    protected override void Awake()
    {
        base.Awake();
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
            AddIngredientServerRpc(KitchenGameMultiplayer.Instance.GetKitchenObjectSOIndex(kitchenObjectSO));
            
            return true;
        }
    }
    [ServerRpc(RequireOwnership = false)]
    private void AddIngredientServerRpc(int kitchenObjectSOIndex)
    {
        AddIngredientClientRpc(kitchenObjectSOIndex);
    }
    [ClientRpc]
    private void AddIngredientClientRpc(int kitchenObjectSOIndex)
    {
        SO_KitchenObjects kitchenObjectSO = KitchenGameMultiplayer.Instance.GetKitchenObjectSoFromIndex(kitchenObjectSOIndex);
        // Add ingredient in the list
        _kitchenObjectSOList.Add(kitchenObjectSO);
        //Debug.Log("Ingredient added to the plate");

        OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs
        {
            kitchenObjectSOArgs = kitchenObjectSO
        });
    }

    public List<SO_KitchenObjects> GetKitchenObjectSOList()
    {
        return _kitchenObjectSOList;
    }
}
