using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public event EventHandler OnStateChange;
    public static GameManager Instance { get; private set; }
    
    private enum State
    {
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver,
    }

    private State _currentState;
    private float _waitingToStartTimer = 1f;
    private float _countdownToStartTimer = 3f;
    private float _gamePlayingToStartTimer = 10f;

    private void Awake()
    {
        Instance = this;
        _currentState = State.WaitingToStart;
    }

    private void Update()
    {
        switch (_currentState)
        {
            case State.WaitingToStart:
                _waitingToStartTimer -= Time.deltaTime;
                if (_waitingToStartTimer < 0f)
                {
                    _currentState = State.CountdownToStart;
                    OnStateChange?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.CountdownToStart:
                _countdownToStartTimer -= Time.deltaTime;
                if (_countdownToStartTimer < 0f)
                {
                    _currentState = State.GamePlaying;
                    OnStateChange?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GamePlaying:
                _gamePlayingToStartTimer -= Time.deltaTime;
                if (_gamePlayingToStartTimer < 0f)
                {
                    _currentState = State.GameOver;
                    OnStateChange?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GameOver:
                break;
        }
        Debug.Log(_currentState);
    }

    public bool IsGamePlaying()
    {
        return _currentState == State.GamePlaying;
    }

    public bool  IsCountdownToStartActive()
    {
        return _currentState == State.CountdownToStart;
    }

    public float GetCountdownToStartTimer()
    {
        return _countdownToStartTimer;
    }

}
