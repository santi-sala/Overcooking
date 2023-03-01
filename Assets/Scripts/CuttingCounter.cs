using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter
{
    [SerializeField] private SO_KitchenObjects _cutKitchenObjectSO;
    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            // No kitchen object on the counter
            Debug.Log("There is NO kitchen object on the counter!!");
            if (player.HasKitchenObject())
            {
                // Player has a kitchen object
                player.GetKitchenObject().SetKitchenObjectParent(this);
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
        if (HasKitchenObject())
        {
            // There is a kitchen object... then cut it!!
            GetKitchenObject().DestroySelf();
            KitchenObject.SpawnKitchenObject(_cutKitchenObjectSO, this);

        }
    }
}

