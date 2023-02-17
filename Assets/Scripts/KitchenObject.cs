using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{

    [SerializeField] private SO_KitchenObjects _kitchenObjectSO;

    public SO_KitchenObjects GetKitchenObjectSO()
    {
        return _kitchenObjectSO;
    }
}
