using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetStaticDataManager : MonoBehaviour
{
    // This script will reset any STATIC EVENT that we have in any script so we dont have any duplications causing erros.
    // This script should only exist in the game manager as the issue with static events will start once we destroy and load scenes!!
    // Check object lifetime and statics
    private void Awake()
    {
        // Clearing the OnAnyObjectPlacedHere STATIC EVENT from BaseCounter class
        BaseCounter.ResetStaticData();

        // Clearing the OnAnyCut STATIC EVENT from CuttingCounter class
        CuttingCounter.ResetStaticData();

        // Clearing the OnAnyObjectTrashed STATIC EVENT from TrashCounter class
        TrashCounter.ResetStaticData();

        // Clearing the OnAnyPlayerSpawned STATIC EVENT from TrashCounter class
        Player.ResetStaticData();
    }
}