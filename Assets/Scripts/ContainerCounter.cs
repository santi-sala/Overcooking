using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
    public event EventHandler OnPlayerGrabbedObject;

    [SerializeField] private SO_KitchenObjects _kitchenObjectSO;

    public override void Interact(Player player)
    {        
        if (!player.HasKitchenObject())
        {
            // Player does not ahve a kitchen object
            Transform kitchenObjectTranform = Instantiate(_kitchenObjectSO.GetPrefab().transform);
            kitchenObjectTranform.GetComponent<KitchenObject>().SetKitchenObjectParent(player);
            
            OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
        }
    }
}
