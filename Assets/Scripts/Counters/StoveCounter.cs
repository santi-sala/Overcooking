using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class StoveCounter : BaseCounter, IHasProgress
{
    public event EventHandler<IHasProgress.OnprogressEventChangedArgs> OnProgressChanged;
    public event EventHandler<OnStateChangedEventArgs> OnStateChange;
    public class OnStateChangedEventArgs : EventArgs
    {
        public State currentState;
    }
    public enum State
    {
        Idle,
        Frying,
        Fried,
        Burned,
    }

    [SerializeField] private SO_FryingRecipe[] _fryingRecipeSOArray;
    [SerializeField] private SO_BurningRecipe[] _burningRecipeSOArray;


    private NetworkVariable<State> _currentState = new NetworkVariable<State>(State.Idle);
    private NetworkVariable<float> _fryingTimer = new NetworkVariable<float>(0f);
    private SO_FryingRecipe _fryingRecipeS0;
    private NetworkVariable<float> _burningTimer = new NetworkVariable<float>(0f);
    private SO_BurningRecipe _burningRecipeS0;

    public override void OnNetworkSpawn()
    {
        _fryingTimer.OnValueChanged += FryingTimer_OnValueChanged;
        _burningTimer.OnValueChanged += BurningTimer_OnValueChanged;
        _currentState.OnValueChanged += CurrentState_OnValueChanged;
    }

    private void CurrentState_OnValueChanged(State previousState, State newState)
    {
        OnStateChange?.Invoke(this, new OnStateChangedEventArgs
        {
            currentState = _currentState.Value
        });

        if (_currentState.Value == State.Burned || _currentState.Value == State.Idle)
        {
            OnProgressChanged?.Invoke(this, new IHasProgress.OnprogressEventChangedArgs
            {
                progressNormalized = 0f
            });
        }
    }

    private void FryingTimer_OnValueChanged(float previousValue, float newValue)
    {
        float fryingTimerMax = _fryingRecipeS0 != null ? _fryingRecipeS0.fryingTimerMax : 1f;

        OnProgressChanged?.Invoke(this, new IHasProgress.OnprogressEventChangedArgs
        {
            progressNormalized = _fryingTimer.Value / fryingTimerMax
        });
    }

    private void BurningTimer_OnValueChanged(float previousValue, float newValue)
    {
        float burningTimerMax = _burningRecipeS0 != null ? _burningRecipeS0.burningTimerMax : 1f;

        OnProgressChanged?.Invoke(this, new IHasProgress.OnprogressEventChangedArgs
        {
            progressNormalized = _burningTimer.Value / burningTimerMax
        }) ;
    }

    private void Update()
    {
        if(!IsServer)
        {
            return;
        }
        if (HasKitchenObject())
        {
            switch (_currentState.Value)
            {
                case State.Idle:
                    break;
                case State.Frying:
                    _fryingTimer.Value += Time.deltaTime;

                    if (_fryingTimer.Value > _fryingRecipeS0.fryingTimerMax)
                    {
                        // Fried
                        KitchenObject.DestroyKitchenObject(GetKitchenObject());

                        KitchenObject.SpawnKitchenObject(_fryingRecipeS0.output, this);

                        //Debug.Log("object fried!!)");

                        _currentState.Value = State.Fried;
                        _burningTimer.Value = 0f;
                        SetBurningRecipeSOClientRpc(KitchenGameMultiplayer.Instance.GetKitchenObjectSOIndex(GetKitchenObject().GetKitchenObjectSO()));
                    }
                    break;
                case State.Fried:
                    _burningTimer.Value += Time.deltaTime;

                    if (_burningTimer.Value > _burningRecipeS0.burningTimerMax)
                    {
                        // Fried
                        KitchenObject.DestroyKitchenObject(GetKitchenObject());

                        KitchenObject.SpawnKitchenObject(_burningRecipeS0.output, this);

                        Debug.Log("object burned!!)");
                        _currentState.Value = State.Burned;
                    }
                    break;
                case State.Burned:
                    break;               
            }
        }
        else
        {
            SetStateIdleServerRpc();
        }
            //Debug.Log(_currentState);
          
    }
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
                    // Player is carrying something that can be fried
                    KitchenObject kitchenObject = player.GetKitchenObject();
                    kitchenObject.SetKitchenObjectParent(this);

                    InteractLogicPLaceObjectOnCounterServerRpc(KitchenGameMultiplayer.Instance.GetKitchenObjectSOIndex(kitchenObject.GetKitchenObjectSO()));                    
                }
            }
            else
            {
                // Player is NOT carrying a kitchen object
                //Debug.Log("Get a kitchen object!!");
            }
        }
        else
        {
            // There is a kitchen object
            //Debug.Log("There IS kitchen object on the counter!!");

            if (player.HasKitchenObject())
            {
                // Player has a kitcheobject

                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    // PLayer is holding a plate                    
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        KitchenObject.DestroyKitchenObject(GetKitchenObject());

                        SetStateIdleServerRpc();
                    }
                }
            }
            else
            {
                // Player has NO kitcheobject
                GetKitchenObject().SetKitchenObjectParent(player);

                SetStateIdleServerRpc();
            }
        }
    }
    [ServerRpc(RequireOwnership = false)]
    private void SetStateIdleServerRpc()
    {
        
        //_currentState.Value = State.Idle;
        //**************HEREERER*********************
        //HideProgressBarClientRpc();

        _currentState.Value = State.Idle;
    }
    /*
    [ClientRpc]
    private void HideProgressBarClientRpc()
    {
        OnProgressChanged?.Invoke(this, new IHasProgress.OnprogressEventChangedArgs
        {
            progressNormalized = 0f
        }); ;
    }
    */

    [ServerRpc(RequireOwnership = false)]
    private void InteractLogicPLaceObjectOnCounterServerRpc(int kitchenObjectSOIndex)
    {
        _fryingTimer.Value = 0f;
        _currentState.Value = State.Frying;

        SetFryingRecipeSOClientRpc(kitchenObjectSOIndex);
    }

    [ClientRpc]
    private void SetFryingRecipeSOClientRpc(int kitchenObjectSOIndex)
    {
        SO_KitchenObjects kitchenObjectSO = KitchenGameMultiplayer.Instance.GetKitchenObjectSoFromIndex(kitchenObjectSOIndex);
        _fryingRecipeS0 = GetfryingRecipeSOWithInput(kitchenObjectSO);
    }

    [ClientRpc]
    private void SetBurningRecipeSOClientRpc(int kitchenObjectSOIndex)
    {
        SO_KitchenObjects kitchenObjectSO = KitchenGameMultiplayer.Instance.GetKitchenObjectSoFromIndex(kitchenObjectSOIndex);
        _burningRecipeS0 = GetBurningRecipeSOWithInput(kitchenObjectSO);
    }

    private bool HasRecipeWithInput(SO_KitchenObjects inputKitchenObjectSO)
    {
        SO_FryingRecipe fryingRecipeSO = GetfryingRecipeSOWithInput(inputKitchenObjectSO);
        return fryingRecipeSO != null;
    }

    private SO_FryingRecipe GetfryingRecipeSOWithInput(SO_KitchenObjects inputKitchenObjectSO)
    {
        foreach (SO_FryingRecipe fryingRecipeSO in _fryingRecipeSOArray)
        {
            if (fryingRecipeSO.input == inputKitchenObjectSO)
            {
                return fryingRecipeSO;
            }
        }
        return null;
    }

    private SO_BurningRecipe GetBurningRecipeSOWithInput(SO_KitchenObjects inputKitchenObjectSO)
    {
        foreach (SO_BurningRecipe burningRecipeSO in _burningRecipeSOArray)
        {
            if (burningRecipeSO.input == inputKitchenObjectSO)
            {
                return burningRecipeSO;
            }
        }
        return null;
    }

    public bool IsFried()
    {
        return _currentState.Value == State.Fried;
    }
}
