using System;
using System.Collections;
using System.Collections.Generic;
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
        _spawnPlateTimer += Time.deltaTime;
        if (_spawnPlateTimer > _spwanPLateTimerMax)
        {
            _spawnPlateTimer = 0f;

            if (_platesSpawnedAmount < _platesSpawnedAmountMax)
            {
                _platesSpawnedAmount++;

                OnPlateSpawned?.Invoke(this, EventArgs.Empty);
            }

        }
    }

    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject())
        {
            // Player does not have a kitchen object
            if (_platesSpawnedAmount > 0)
            {
                // There is one plate in the the plate counter
                _platesSpawnedAmount--;


                KitchenObject.SpawnKitchenObject(_plateKitchenObjectSO, player);
                OnPlateRemoved?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
