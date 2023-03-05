using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{

    [SerializeField] private SO_KitchenObjects _kitchenObjectSO;

    private IKitchenObjectParent _kitchenObjectParent;

    public SO_KitchenObjects GetKitchenObjectSO()
    {
        return _kitchenObjectSO;
    }

    public void SetKitchenObjectParent(IKitchenObjectParent kitchenObjectParent)
    {
        // Clearing kitchen object from old clear counter
        if (this._kitchenObjectParent != null)
        {
            this._kitchenObjectParent.ClearKitchenObject();
        }

        // Setting the new kitchen object to the new clear counter
        this._kitchenObjectParent = kitchenObjectParent;
        if (kitchenObjectParent.HasKitchenObject())
        {
            Debug.Log("IKitchenObjectParent has already a kitchen object!!!");
        }
        kitchenObjectParent.SetKitchenObject(this);

        // Teleporting the visual of the kitchen object to the new parent
        transform.parent = kitchenObjectParent.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }

    public IKitchenObjectParent GetKitchenObjectParent()
    {
        return _kitchenObjectParent;
    }

    public void DestroySelf()
    {
        _kitchenObjectParent.ClearKitchenObject();
        Destroy(gameObject);
    }

    public bool TryGetPlate(out PlateKitchenObject plateKitchenObject)
    {
        if (this is PlateKitchenObject)
        {
            plateKitchenObject = this as PlateKitchenObject;
            return true;
        }
        else
        {
            plateKitchenObject = null;
            return false;
        }
    }


    public static KitchenObject SpawnKitchenObject(SO_KitchenObjects kitchenObjectSO, IKitchenObjectParent kitchenObjectParent)
    {
        Transform kitchenObjectTranform = Instantiate(kitchenObjectSO.GetPrefab().transform);

        KitchenObject kitchenObject = kitchenObjectTranform.GetComponent<KitchenObject>();

        kitchenObject.SetKitchenObjectParent(kitchenObjectParent);

        return kitchenObject;
    }
}
