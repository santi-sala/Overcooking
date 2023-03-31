using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class KitchenGameMultiplayer : NetworkBehaviour
{
    public static KitchenGameMultiplayer Instance { get; private set; }

    [SerializeField] private SO_KitchenObjectList _kitchenObjectSOList;

    private void Awake()
    {
        Instance = this;
    }

    public void SpawnKitchenObject(SO_KitchenObjects kitchenObjectSO, IKitchenObjectParent kitchenObjectParent)
    {
        SpawnKitchenObjectServerRpc(GetKitchenObjectSOIndex(kitchenObjectSO), kitchenObjectParent.GetNetworkObject());    
    }
    
    [ServerRpc(RequireOwnership = false)]
    private void SpawnKitchenObjectServerRpc(int kitchenObjectSOIndex, NetworkObjectReference kitchenObjectParentNetworkObjectReference)
    {
        SO_KitchenObjects kitchenObjectSO = GetKitchenObjectSoFromIndex(kitchenObjectSOIndex);
        Transform kitchenObjectTranform = Instantiate(kitchenObjectSO.GetPrefab().transform);

        NetworkObject kitchenObjectNetworkObject = kitchenObjectTranform.GetComponent<NetworkObject>();
        kitchenObjectNetworkObject.Spawn(true);

        KitchenObject kitchenObject = kitchenObjectTranform.GetComponent<KitchenObject>();

        kitchenObjectParentNetworkObjectReference.TryGet(out NetworkObject kitchenObjectParentNetworkObject);
        IKitchenObjectParent kitchenObjectParent = kitchenObjectParentNetworkObject.GetComponent<IKitchenObjectParent>();
        kitchenObject.SetKitchenObjectParent(kitchenObjectParent);
    }

    private  int GetKitchenObjectSOIndex(SO_KitchenObjects kitchenObjectSO)
    {
        return _kitchenObjectSOList.kitchenObjectSOList.IndexOf(kitchenObjectSO);
    }

    private SO_KitchenObjects GetKitchenObjectSoFromIndex(int kitchenObjectSOIndex)
    {
        return _kitchenObjectSOList.kitchenObjectSOList[kitchenObjectSOIndex];
    }
    
}
