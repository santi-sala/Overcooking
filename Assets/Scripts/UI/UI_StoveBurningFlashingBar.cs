using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_StoveBurningFlashingBar : MonoBehaviour
{
    private const string IS_FLASHING = "IsFlashing";

    [SerializeField] private StoveCounter _stoveCounter;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        
        _animator.SetBool(IS_FLASHING, false);
    }
    private void Start()
    {
        _stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;

    }

    private void StoveCounter_OnProgressChanged(object sender, IHasProgress.OnprogressEventChangedArgs e)
    {
        float burnShowProgressAmount = 0.5f;
        bool show = _stoveCounter.IsFried() && e.progressNormalized >= burnShowProgressAmount;

        _animator.SetBool(IS_FLASHING, show);
    }

    
}
