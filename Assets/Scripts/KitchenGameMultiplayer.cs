using System;
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

    public void StartHost()
    {
        NetworkManager.Singleton.ConnectionApprovalCallback += NetworkManager_Singleton_ConnectionApprovalCallback;
        NetworkManager.Singleton.StartHost();
    }

    private void NetworkManager_Singleton_ConnectionApprovalCallback(NetworkManager.ConnectionApprovalRequest connectionApprovalRequest, NetworkManager.ConnectionApprovalResponse connectionApprovalResponse)
    {
        if (GameManager.Instance.IsWaitingToStart())
        {
            connectionApprovalResponse.Approved = true;
            connectionApprovalResponse.CreatePlayerObject = true;
        }
        else
        {
            connectionApprovalResponse.Approved = false;
        }
    }

    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
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

    public  int GetKitchenObjectSOIndex(SO_KitchenObjects kitchenObjectSO)
    {
        return _kitchenObjectSOList.kitchenObjectSOList.IndexOf(kitchenObjectSO);
    }

    public SO_KitchenObjects GetKitchenObjectSoFromIndex(int kitchenObjectSOIndex)
    {
        return _kitchenObjectSOList.kitchenObjectSOList[kitchenObjectSOIndex];
    }

    public void DestroyKitchenObject (KitchenObject kitchenObject)
    {
        DestroyObjectServerRpc(kitchenObject.NetworkObject);
    }

    [ServerRpc(RequireOwnership = false)]
    private void DestroyObjectServerRpc(NetworkObjectReference kitchenNetworkObjectReference)
    {
        kitchenNetworkObjectReference.TryGet(out NetworkObject kitchenNetworkObject);
        KitchenObject kitchenObject = kitchenNetworkObject.GetComponent<KitchenObject>();

        ClearKitchenObjectOnParentClientRpc(kitchenNetworkObjectReference);

        kitchenObject.DestroySelf();
    }

    [ClientRpc]
    private void ClearKitchenObjectOnParentClientRpc(NetworkObjectReference kitchenNetworkObjectReference)
    {
        kitchenNetworkObjectReference.TryGet(out NetworkObject kitchenNetworkObject);
        KitchenObject kitchenObject = kitchenNetworkObject.GetComponent<KitchenObject>();

        kitchenObject.ClearKitchenObjectOnParent();
    }
    
}
