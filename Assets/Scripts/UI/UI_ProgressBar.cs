using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ProgressBar : MonoBehaviour
{
    [SerializeField] private GameObject _hasProgressGameObject;
    [SerializeField] private Image _barImage;

    private IHasProgress _hasProgress;
    private void Start()
    {
        _hasProgress = _hasProgressGameObject.GetComponent<IHasProgress>();
        if (_hasProgress == null)
        {
            Debug.LogError("GameObject " + _hasProgressGameObject.name + " does not have a component that implements IHasProgress");
        }
        _hasProgress.OnProgressChanged += HasProgress_OnProgressChanged;

        _barImage.fillAmount = 0f;

        Hide();
    }

    private void HasProgress_OnProgressChanged(object sender, IHasProgress.OnprogressEventChangedArgs e)
    {
        _barImage.fillAmount = e.progressNormalized;

        if (e.progressNormalized == 0f || e.progressNormalized == 1f)
        {
            //Debug.Log("Hide " + e.progressNormalized);
          Hide();
        }
        else
        {
            //Debug.Log("Show " + e.progressNormalized);
            Show();
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject?.SetActive(false);
    }
}
