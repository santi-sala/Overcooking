using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgress
    {
    public static event EventHandler OnAnyCut;

    // The new here is because it inherits from BaseCounter which has the same function
    new public static void ResetStaticData()
    {
        OnAnyCut = null;
    }

    public event EventHandler<IHasProgress.OnprogressEventChangedArgs> OnProgressChanged;
    public event EventHandler OnCut;

    [SerializeField] private SO_CuttingRecipe[] _cuttingRecipeSOArray;

    private int _cuttingProgress;

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            // No kitchen object on the counter
            if (player.HasKitchenObject())
            {
                // Player has a kitchen object
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    // Player is carrying something that can be sliced!!
                    KitchenObject kitchenObject = player.GetKitchenObject();
                    kitchenObject.SetKitchenObjectParent(this);

                    InteractLogicPlaceObjectOnCounterServerRpc();
                }
            }
            else
            {
                // Player is NOT carrying a kitchen object
                Debug.Log("Get a kitxhen object!!");
            }
        }
        else
        {
            // There is a kitchen object
            if (player.HasKitchenObject())
            {
                // Player has a kistcheobject
                Debug.Log("Player is carrying a kitcken object");

                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    // PLayer is holding a plate                    
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        KitchenObject.DestroyKitchenObject(GetKitchenObject());
                    }
                }
            }
            else
           {
                // Player has NO kitcheobject
                GetKitchenObject().SetKitchenObjectParent(player);
                
                // Resetting the progressbar!!
                InteractLogicPlaceObjectOnCounterServerRpc();
            }
        }
    }
    [ServerRpc(RequireOwnership = false)]
    private void InteractLogicPlaceObjectOnCounterServerRpc()
    {
        InteractLogicPlaceObjectOnCounterClientRpc();
    }

    [ClientRpc]
    private void InteractLogicPlaceObjectOnCounterClientRpc()
    {
        _cuttingProgress = 0;

       
        OnProgressChanged?.Invoke(this, new IHasProgress.OnprogressEventChangedArgs
        {
            progressNormalized = 0f
        });;
    }

    public override void InteractAlternate(Player player)
    {
        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO()))
        {
            CutObjectServerRpc();
            TestCuttingProgressDonceServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void CutObjectServerRpc()
    {
        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO()))
        {
            CutObjectClientRpc();
        }
    }

    [ClientRpc]
    private void CutObjectClientRpc()
    {
        // There is a kitchen object AND can be cut... then cut it!!
        _cuttingProgress++;
        OnCut?.Invoke(this, EventArgs.Empty);

        //Debug.Log(OnAnyCut.GetInvocationList().Length);
        OnAnyCut?.Invoke(this, EventArgs.Empty);

        SO_CuttingRecipe cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

        OnProgressChanged?.Invoke(this, new IHasProgress.OnprogressEventChangedArgs
        {
            progressNormalized = (float)_cuttingProgress /  cuttingRecipeSO.cuttingProgressMax
        });
        
    }
    [ServerRpc(RequireOwnership = false)]
    private void TestCuttingProgressDonceServerRpc()
    {
        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO()))
        {
            SO_CuttingRecipe cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
            if (_cuttingProgress >= cuttingRecipeSO.cuttingProgressMax)
            {
                SO_KitchenObjects outputKitchenObjectSO = GetOutputFromInputKitchenObject(GetKitchenObject().GetKitchenObjectSO());

                KitchenObject.DestroyKitchenObject(GetKitchenObject());

                KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);
            }
        }
    }

    private SO_KitchenObjects GetOutputFromInputKitchenObject(SO_KitchenObjects inputKitchenObjectSO)
    {
        SO_CuttingRecipe cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
        if (cuttingRecipeSO != null)
        {
            return cuttingRecipeSO.output;
        }
        else
        {
            return null;
        }        
    }

    private bool HasRecipeWithInput(SO_KitchenObjects inputKitchenObjectSO)
    {
        SO_CuttingRecipe cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
        return cuttingRecipeSO != null;
    }

    private SO_CuttingRecipe GetCuttingRecipeSOWithInput(SO_KitchenObjects inputKitchenObjectSO)
    {
        foreach (SO_CuttingRecipe cuttingRecipeSO in _cuttingRecipeSOArray)
        {
            if (cuttingRecipeSO.input == inputKitchenObjectSO)
            {
                return cuttingRecipeSO;
            }
        }
        return null;
    }
}

