using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterSound : MonoBehaviour
{
    [SerializeField] private StoveCounter _stoveCounter;
    private AudioSource _audioSource;

    private bool _playWarningSound;
    private float _warningSoundTimer;
    private float _warningSoundTimerMax = 0.2f;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        _stoveCounter.OnStateChange += StoveCounter_OnStateChange;
        _stoveCounter.OnProgressChanged += _stoveCounter_OnProgressChanged;
    }

    private void _stoveCounter_OnProgressChanged(object sender, IHasProgress.OnprogressEventChangedArgs e)
    {
        float burnShowProgressAmount = 0.5f;
        _playWarningSound = _stoveCounter.IsFried() && e.progressNormalized >= burnShowProgressAmount;
    }

    private void StoveCounter_OnStateChange(object sender, StoveCounter.OnStateChangedEventArgs e)
    {
        bool playSound = e.currentState == StoveCounter.State.Frying || e.currentState == StoveCounter.State.Fried;

        if (playSound)
        {
            _audioSource.Play();

        }
        else
        {
            _audioSource.Pause();
        }
    }

    private void Update()
    {
        if (_playWarningSound)
        {
            _warningSoundTimer -= Time.deltaTime;
            if (_warningSoundTimer <= 0f)
            {
                _warningSoundTimer = _warningSoundTimerMax;

                SoundManager.Instance.PlayWarningSound(_stoveCounter.transform.position);
            }
        }
    }
}
