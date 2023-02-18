using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{

    [SerializeField] private SO_KitchenObjects _kitchenObjectSO;

    private ClearCounter _clearCounter;

    public SO_KitchenObjects GetKitchenObjectSO()
    {
        return _kitchenObjectSO;
    }

    public void SetClearCounter(ClearCounter clearCounter)
    {
        // Clearing kitchen object from old clear counter
        if (this._clearCounter != null)
        {
            this._clearCounter.ClearKitchenObject();
        }

        // Setting the new kitchen object to the new clear counter
        this._clearCounter = clearCounter;
        if (clearCounter.HasKitchenObject())
        {
            Debug.Log("Counter has already a kitchen object object!!!");
        }
        clearCounter.SetKitchenObject(this);

        // Teleporting the visual of the kitchen object to the new parent
        transform.parent = clearCounter.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }

    public ClearCounter GetClearCounter()
    {
        return _clearCounter;
    }
}
