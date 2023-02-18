using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : MonoBehaviour
{
    [SerializeField] private SO_KitchenObjects _kitchenObjectSO;
    [SerializeField] private Transform _counterTopPoint;
    [SerializeField] private ClearCounter _secondCounter;
    [SerializeField] private bool _testing;


    private KitchenObject _kitchenObject;

    private void Update()
    {
        if (_testing && Input.GetKeyDown(KeyCode.T))
        {
            if (_kitchenObject != null)
            {
                _kitchenObject.SetClearCounter(_secondCounter);
            }
        }
    }
    public void Interact()
    {
        // Spawning a kitchen object in a clear counter
        if (_kitchenObject == null)
        {
            Transform kitchenObjectTranform =  Instantiate(_kitchenObjectSO.GetPrefab().transform, _counterTopPoint);
            
            // This will know that there is something on top of the counter therefore not spawning more.
            //_kitchenObject = kitchenObjectTranform.GetComponent<KitchenObject>();
            // This will know on which vcounter it the kitchen object is
            //_kitchenObject.SetClearCounter(this);
            //kitchenObjectTranform.localPosition = Vector3.zero;
            kitchenObjectTranform.GetComponent<KitchenObject>().SetClearCounter(this);

        }
        else
        {
            Debug.Log(_kitchenObject.GetClearCounter());
        }
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return _counterTopPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this._kitchenObject = kitchenObject;
    }

    public KitchenObject GetKitchenObject()
    {
        return _kitchenObject;
    }

    public void ClearKitchenObject() 
    {
        _kitchenObject = null; 
    }

    public bool HasKitchenObject()
    {
        return _kitchenObject != null;
    }
}
