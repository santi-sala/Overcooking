using System;
using System.Collections;
using System.Collections.Generic;
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


    private State _currentState;
    private float _fryingTimer;
    private SO_FryingRecipe _fryingRecipeS0;
    private float _burningTimer;
    private SO_BurningRecipe _burningRecipeS0;


    private void Start()
    {
        _currentState = State.Idle;
    }
    private void Update()
    {
        if (HasKitchenObject())
        {
            switch (_currentState)
            {
                case State.Idle:
                    break;
                case State.Frying:
                    _fryingTimer += Time.deltaTime;

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnprogressEventChangedArgs
                    {
                        progressNormalized = _fryingTimer / _fryingRecipeS0.fryingTimerMax
                    });

                    if (_fryingTimer > _fryingRecipeS0.fryingTimerMax)
                    {
                        // Fried
                        GetKitchenObject().DestroySelf();

                        KitchenObject.SpawnKitchenObject(_fryingRecipeS0.output, this);

                        Debug.Log("object fried!!)");

                        _currentState = State.Fried;
                        _burningTimer = 0f;
                        _burningRecipeS0 = GetBurningRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

                        OnStateChange?.Invoke(this, new OnStateChangedEventArgs
                        {
                            currentState = _currentState,
                        });
                    }
                    break;
                case State.Fried:
                    _burningTimer += Time.deltaTime;

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnprogressEventChangedArgs
                    {
                        progressNormalized = _burningTimer / _burningRecipeS0.burningTimerMax
                    }) ;

                    if (_burningTimer > _burningRecipeS0.burningTimerMax)
                    {
                        // Fried
                        GetKitchenObject().DestroySelf();

                        KitchenObject.SpawnKitchenObject(_burningRecipeS0.output, this);

                        Debug.Log("object burned!!)");
                        _currentState = State.Burned;

                        OnStateChange?.Invoke(this, new OnStateChangedEventArgs
                        {
                            currentState = _currentState,
                        });

                        OnProgressChanged?.Invoke(this, new IHasProgress.OnprogressEventChangedArgs
                        {
                            progressNormalized = 0f
                        }) ;
                    }
                    break;
                case State.Burned:
                    break;               
            }
        }       
            Debug.Log(_currentState);
          
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
                    player.GetKitchenObject().SetKitchenObjectParent(this);

                    _fryingRecipeS0 = GetfryingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

                    _currentState = State.Frying;
                    _fryingTimer = 0f;

                    OnStateChange?.Invoke(this, new OnStateChangedEventArgs
                    {
                        currentState = _currentState,
                    });

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnprogressEventChangedArgs
                    {
                        progressNormalized = _fryingTimer / _fryingRecipeS0.fryingTimerMax
                    });
                }
            }
            else
            {
                // Player is NOT carrying a kitchen object
                Debug.Log("Get a kitchen object!!");
            }
        }
        else
        {
            // There is a kitchen object
            Debug.Log("There IS kitchen object on the counter!!");

            if (player.HasKitchenObject())
            {
                // Player has a kitcheobject
                Debug.Log("Player is carrying a citcken object");

                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    // PLayer is holding a plate                    
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf();
                    }
                }

                _currentState = State.Idle;

                OnStateChange?.Invoke(this, new OnStateChangedEventArgs
                {
                    currentState = _currentState,
                });

                OnProgressChanged?.Invoke(this, new IHasProgress.OnprogressEventChangedArgs
                {
                    progressNormalized = 0f
                });
            }
            else
            {
                // Player has NO kitcheobject
                GetKitchenObject().SetKitchenObjectParent(player);

                _currentState = State.Idle;

                OnStateChange?.Invoke(this, new OnStateChangedEventArgs
                {
                    currentState = _currentState,
                });

                OnProgressChanged?.Invoke(this, new IHasProgress.OnprogressEventChangedArgs
                {
                    progressNormalized = 0f
                });
            }
        }
    }

    private bool HasRecipeWithInput(SO_KitchenObjects inputKitchenObjectSO)
    {
        SO_FryingRecipe fryingRecipeSO = GetfryingRecipeSOWithInput(inputKitchenObjectSO);
        return fryingRecipeSO != null;
    }

    private SO_KitchenObjects GetOutputFromInputKitchenObject(SO_KitchenObjects inputKitchenObjectSO)
    {
        SO_FryingRecipe fryingRecipeSO = GetfryingRecipeSOWithInput(inputKitchenObjectSO);
        if (fryingRecipeSO != null)
        {
            return fryingRecipeSO.output;
        }
        else
        {
            return null;
        }
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
}
