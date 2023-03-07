using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    public static DeliveryManager Instance { get; private set; }
    [SerializeField] private SO_RecipeList _recipeListSO;
    
    private List<SO_Recipe> _waitingRecipeSOList;
    private float _spawnRecipeTimer;
    private float _spawnTimerMax = 4f;
    private int _waitingRecipesMax = 4;

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
                SO_Recipe waitingRecipeSO = _recipeListSO.recipeSOList[Random.Range(0, _recipeListSO.recipeSOList.Count)];
                Debug.Log(waitingRecipeSO.GetRecipeName());
                _waitingRecipeSOList.Add(waitingRecipeSO);
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
                    Debug.Log("Succes delivery of a recipe");
                    _waitingRecipeSOList.RemoveAt(i);
                    return;
                }
            }
        }
        // Player did not deliver correct recipe;
        Debug.Log("Player did NOT deliver correct recipe");
    }
}
