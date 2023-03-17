using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeCompleted;
    public event EventHandler OnRecipeFailed;
    public event EventHandler OnRecipeSuccess;

    public static DeliveryManager Instance { get; private set; }
    [SerializeField] private SO_RecipeList _recipeListSO;
    
    private List<SO_Recipe> _waitingRecipeSOList;
    private float _spawnRecipeTimer;
    private float _spawnTimerMax = 4f;
    private int _waitingRecipesMax = 4;
    private int _succesfulRecipesAmount;

    private void Awake()
    {
        Instance = this;
        _waitingRecipeSOList = new List<SO_Recipe>();
    }
    private void Update()
    {
        _spawnRecipeTimer -= Time.deltaTime;
        if (_spawnRecipeTimer <= 0f)
        {
            _spawnRecipeTimer = 0f;
            _spawnRecipeTimer = _spawnTimerMax;

            if (_waitingRecipeSOList.Count < _waitingRecipesMax)
            {
                SO_Recipe waitingRecipeSO = _recipeListSO.recipeSOList[UnityEngine.Random.Range(0, _recipeListSO.recipeSOList.Count)];
                //Debug.Log(waitingRecipeSO.GetRecipeName());
                _waitingRecipeSOList.Add(waitingRecipeSO);

                OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
            }
        }


    }
    public void DevliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        for (int i = 0; i < _waitingRecipeSOList.Count; i++)
        {
            SO_Recipe waitingRecipeSO = _waitingRecipeSOList[i];

            if (waitingRecipeSO.GetKitchenObjectSOList().Count == plateKitchenObject.GetKitchenObjectSOList().Count)
            {
                bool plateContentMatchesRecipe = true;
                // Has the same number of ingredients
                foreach (SO_KitchenObjects recipekitchenObjectSO in waitingRecipeSO.GetKitchenObjectSOList())
                {
                    bool ingrdientFound = false; 
                    // Cycling through all ingredients in the RECIPE
                    foreach (SO_KitchenObjects platekitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList())
                    {
                        // Cycling through all ingredients in the PLATE
                        if (platekitchenObjectSO == recipekitchenObjectSO) 
                        {
                            //Ingredient matchtes!
                            ingrdientFound = true;
                            break;
                        }
                    }
                    if (!ingrdientFound)
                    {
                        // An ingredient in this recipe was NOT FOUND
                        plateContentMatchesRecipe = false;
                    }
                }
                if (plateContentMatchesRecipe)
                {
                    // Player deleivers the correct recipe
                    //Debug.Log("Succes delivery of a recipe");
                    _waitingRecipeSOList.RemoveAt(i);

                    _succesfulRecipesAmount++;

                    OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
                    OnRecipeSuccess?.Invoke(this, EventArgs.Empty);
                    return;
                }
            }
        }
        // Player did not deliver correct recipe;
        //Debug.Log("Player did NOT deliver correct recipe");
        OnRecipeFailed?.Invoke(this, EventArgs.Empty);
    }

    public List<SO_Recipe> GetWaitingRecipeSOList()
    {
        return _waitingRecipeSOList;
    }

    public int GetSuccesfulRecipesAmount()
    {
        return _succesfulRecipesAmount;
    }
}
