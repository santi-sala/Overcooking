using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_DeliveryManagerSingle : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _recipeNameText;
    [SerializeField] private Transform _iconContainer;
    [SerializeField] private Transform _ingredientIconTemplate;

    private void Awake()
    {
        _ingredientIconTemplate.gameObject.SetActive(false);
    }

    public void SetRecipeSO(SO_Recipe recipeSO)
    {
        _recipeNameText.text = recipeSO.GetRecipeName();

        foreach (Transform child in _iconContainer)
        {
            if (child == _ingredientIconTemplate)
            {
                continue;
            }
            Destroy(child.gameObject);
        }

        foreach (SO_KitchenObjects kitchenObjectSO in recipeSO.GetKitchenObjectSOList())
        {
            Transform iconTransform = Instantiate(_ingredientIconTemplate, _iconContainer);
            iconTransform.gameObject.SetActive(true);
            iconTransform.GetComponent<Image>().sprite = kitchenObjectSO.GetSprite();
        }
    }
}
