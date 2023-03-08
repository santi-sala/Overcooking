using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_DeliveryManager : MonoBehaviour
{
    

    [SerializeField] private Transform _container;
    [SerializeField] private Transform _recipeTemplate;

    private void Awake()
    {
        _recipeTemplate.gameObject.SetActive(false);        
    }
    private void Start()
    {
        DeliveryManager.Instance.OnRecipeSpawned += DeliveryManager_Instance_OnRecipeSpawned;
        DeliveryManager.Instance.OnRecipeCompleted += DeliveryManager_Instance_OnRecipeCompleted;

        UpdateVisual();
    }

    private void DeliveryManager_Instance_OnRecipeSpawned(object sender, System.EventArgs e)
    {
        UpdateVisual();
    }

    private void DeliveryManager_Instance_OnRecipeCompleted(object sender, System.EventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        foreach (Transform child in _container)
        {
            if (child == _recipeTemplate)
            {
                continue;
            }
            Destroy(child.gameObject);
        }

        foreach (SO_Recipe recipeSO in DeliveryManager.Instance.GetWaitingRecipeSOList())
        {
            Transform recipeTransform = Instantiate(_recipeTemplate, _container);
            recipeTransform.gameObject.SetActive(true);
            recipeTransform.GetComponent<UI_DeliveryManagerSingle>().SetRecipeSO(recipeSO);
        } 
    }
}
