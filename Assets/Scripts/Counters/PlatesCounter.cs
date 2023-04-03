using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlatesCounter : BaseCounter
{
    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemoved;

    [SerializeField] private SO_KitchenObjects _plateKitchenObjectSO;
    [SerializeField] private float _spwanPLateTimerMax = 4f;
    [SerializeField] private int _platesSpawnedAmountMax = 4;


    private int _platesSpawnedAmount;
    private float _spawnPlateTimer;

    private void Update()
    {
        if (!IsServer)
        {
            return;
        }
        _spawnPlateTimer += Time.deltaTime;
        if (_spawnPlateTimer > _spwanPLateTimerMax)
        {
            _spawnPlateTimer = 0f;

            if (GameManager.Instance.IsGamePlaying() && _platesSpawnedAmount < _platesSpawnedAmountMax)
            {
                SpawnPlateServerRpc();
            }

        }
    }
    [ServerRpc]
    private void SpawnPlateServerRpc()
    {
        SpawnPlateClientRpc();
    }

    [ClientRpc]
    private void SpawnPlateClientRpc()
    {
        _platesSpawnedAmount++;

        OnPlateSpawned?.Invoke(this, EventArgs.Empty);
    }

    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject())
        {
            // Player does not have a kitchen object
            if (_platesSpawnedAmount > 0)
            {             
                KitchenObject.SpawnKitchenObject(_plateKitchenObjectSO, player);
                InteractLogicServerRpc();                
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void InteractLogicServerRpc()
    {
        InteractLogicClientRpc();
    }

    [ClientRpc]
    private void InteractLogicClientRpc()
    {
        _platesSpawnedAmount--;
        OnPlateRemoved?.Invoke(this, EventArgs.Empty);
    }
}
