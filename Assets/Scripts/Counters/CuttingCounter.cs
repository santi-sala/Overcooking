using System;
using System.Collections;
using System.Collections.Generic;
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
            //Debug.Log("There is NO kitchen object on the counter!!");
            if (player.HasKitchenObject())
            {
                // Player has a kitchen object
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    // Player is carrying something that can be sliced!!
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    _cuttingProgress = 0;

                    SO_CuttingRecipe cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnprogressEventChangedArgs
                    {
                        progressNormalized = (float)_cuttingProgress / cuttingRecipeSO.cuttingProgressMax
                    });
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
            //Debug.Log("There IS kitchen object on the counter!!");

            if (player.HasKitchenObject())
            {
                // Player has a kistcheobject
                Debug.Log("Player is carrying a citcken object");

                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    // PLayer is holding a plate                    
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf();
                    }
                }
            }
            else
            {
                // Player has NO kitcheobject
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }

    public override void InteractAlternate(Player player)
    {
        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO()))
        {
            // There is a kitchen object AND can be cut... then cut it!!
            _cuttingProgress++;
            OnCut?.Invoke(this, EventArgs.Empty);

            //Debug.Log(OnAnyCut.GetInvocationList().Length);
            OnAnyCut?.Invoke(this, EventArgs.Empty);

            SO_CuttingRecipe cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

            OnProgressChanged?.Invoke(this, new IHasProgress.OnprogressEventChangedArgs
            {
                progressNormalized = (float)_cuttingProgress / cuttingRecipeSO.cuttingProgressMax
            });

            if (_cuttingProgress >= cuttingRecipeSO.cuttingProgressMax)
            {
                SO_KitchenObjects outputKitchenObjectSO = GetOutputFromInputKitchenObject(GetKitchenObject().GetKitchenObjectSO());
                GetKitchenObject().DestroySelf();

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

