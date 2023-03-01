using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter
{
    [SerializeField] private SO_CuttingRecipe[] _cuttingRecipeSOArray;
    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            // No kitchen object on the counter
            Debug.Log("There is NO kitchen object on the counter!!");
            if (player.HasKitchenObject())
            {
                // Player has a kitchen object
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    // Player is carrying something that can be sliced!!
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                }
            }
            else
            {
                // Player is NOT carrying a kitchen object
                Debug.Log("Get a kitxhen object!!");
            }
        }
        else
        {
            // There is a kitchen object
            Debug.Log("There IS kitchen object on the counter!!");

            if (player.HasKitchenObject())
            {
                // Player has a kistcheobject
                Debug.Log("Player is carrying a citcken object");
            }
            else
            {
                // Player has NO kitcheobject
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }

    public override void InteractAlternate(Player player)
    {
        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO()))
        {
            // There is a kitchen object AND can be cut... then cut it!!
            SO_KitchenObjects outputKitchenObjectSO = GetOutputFromInputKitchenObject(GetKitchenObject().GetKitchenObjectSO());
            GetKitchenObject().DestroySelf();

            KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);

        }
    }

    private SO_KitchenObjects GetOutputFromInputKitchenObject(SO_KitchenObjects inputKitchenObjectSO)
    {
        foreach (SO_CuttingRecipe cuttingRecipeSO in _cuttingRecipeSOArray)
        {
            if (cuttingRecipeSO.input == inputKitchenObjectSO)
            {
                return cuttingRecipeSO.output;
            }
        }
        return null;
    }

    private bool HasRecipeWithInput(SO_KitchenObjects inputKitchenObjectSO)
    {
        foreach (SO_CuttingRecipe cuttingRecipeSO in _cuttingRecipeSOArray)
        {
            if (cuttingRecipeSO.input == inputKitchenObjectSO)
            {
                return true;
            }
        }
        return false;
    }
}

