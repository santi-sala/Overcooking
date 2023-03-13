using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterSound : MonoBehaviour
{
    [SerializeField] private StoveCounter _stoveCounter;
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        _stoveCounter.OnStateChange += StoveCounter_OnStateChange;
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
}
