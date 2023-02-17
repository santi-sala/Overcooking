using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : MonoBehaviour
{
    [SerializeField] private SO_KitchenObjects _kitchenObject;
    [SerializeField] private Transform _counterTopPoint;

    public void Interact()
    {
        Debug.Log("Interact");
        Transform kitchenObjectTranform =  Instantiate(_kitchenObject.GetPrefab().transform, _counterTopPoint);
        kitchenObjectTranform.localPosition = Vector3.zero;

        Debug.Log(kitchenObjectTranform.GetComponent<KitchenObject>().GetKitchenObjectSO().GetObjectName());
    }
}
