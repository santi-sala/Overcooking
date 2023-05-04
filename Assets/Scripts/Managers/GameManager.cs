using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance { get; private set; }

    public event EventHandler OnStateChange;
    public event EventHandler OnLocalGamePaused;
    public event EventHandler OnLocalGameResume;
    public event EventHandler OnMultiplayerGamePause;
    public event EventHandler OnMultiplayerGameResume;
    public event EventHandler OnLocalPlayerReadyChanged;

    private enum State
    {
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver,
    }

    [SerializeField] private Transform _playerPrefab;

    private NetworkVariable<State> _currentState = new NetworkVariable<State>(State.WaitingToStart);
    private bool _isLocalPLayerReady;
    //****************************************************************************
    //Change after finish development
    private NetworkVariable<float> _countdownToStartTimer = new NetworkVariable<float>(3f); 
    private float _gamePlayingTimerMax = 90f;
    //****************************************************************************
    private NetworkVariable<float> _gamePlayingTimer = new NetworkVariable<float>(0.0f);
    private bool _isLocalGamePaused = false;
    private NetworkVariable<bool> _isGamePaused = new NetworkVariable<bool>(false);
    private Dictionary<ulong, bool> _playerReadyDictionary;
    private Dictionary<ulong, bool> _playerPauseDictionary;
    private bool _autoTestGamePausedState;

    private void Awake()
    {
        Instance = this;
        //_currentState = State.WaitingToStart;

        _playerReadyDictionary = new Dictionary<ulong, bool>();
        _playerPauseDictionary = new Dictionary<ulong, bool>();
    }

    private void Start()
    {
        GameInput.Instance.OnPauseEvent += GameInput_Instance_OnPauseEvent;
        GameInput.Instance.OnInteractAction += GameInput_Instance_OnInteractAction;

        /*
        //***********************************************
        // Remove after development is finished!!
        _currentState = State.CountdownToStart;
        OnStateChange?.Invoke(this, EventArgs.Empty);
        //***********************************************
        */
    }

    public override void OnNetworkSpawn()
    {
        _currentState.OnValueChanged += CurrentState_OnvalueChanged;
        _isGamePaused.OnValueChanged += IsGamePaused_OnValueChanged;

        // Handling client disconnections
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_Singleton_OnClientDisconnectCallback;
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += NetworkManager_Singleton_SceneManager_OnLoadEventCompleted;
        }
    }

    private void NetworkManager_Singleton_SceneManager_OnLoadEventCompleted(string sceneName, UnityEngine.SceneManagement.LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            Transform playerTransform = Instantiate(_playerPrefab);
            playerTransform.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, true);
        }
    }

    private void NetworkManager_Singleton_OnClientDisconnectCallback(ulong clientId)
    {
        _autoTestGamePausedState = true;
    }

    private void IsGamePaused_OnValueChanged(bool previousValue, bool newValue)
    {
        if (_isGamePaused.Value)
        {
            Time.timeScale = 0f;

            OnMultiplayerGamePause?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            Time.timeScale = 1f;

            OnMultiplayerGameResume?.Invoke(this, EventArgs.Empty);

        }
    }

    private void CurrentState_OnvalueChanged(State previousValue, State newValue)
    {
        OnStateChange?.Invoke(this, EventArgs.Empty);
    }

    private void GameInput_Instance_OnInteractAction(object sender, EventArgs e)
    {
        if (_currentState.Value == State.WaitingToStart)
        {
            _isLocalPLayerReady = true;

            OnLocalPlayerReadyChanged?.Invoke(this, EventArgs.Empty);
            SetPlayerReadyServerRpc();

            /*
            _currentState = State.CountdownToStart;
            OnStateChange?.Invoke(this, EventArgs.Empty);
            */
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerReadyServerRpc(ServerRpcParams serverRpcParams = default)
    {
        _playerReadyDictionary[serverRpcParams.Receive.SenderClientId] = true;

        bool allClientsReady = true;
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            if (!_playerReadyDictionary.ContainsKey(clientId) || !_playerReadyDictionary[clientId])
            {
                //This player is not ready
                allClientsReady = false;
                break;
            }
        }

        if (allClientsReady)
        {
            _currentState.Value = State.CountdownToStart;
        }
    }

    private void GameInput_Instance_OnPauseEvent(object sender, EventArgs e)
    {
        TogglePauseGame();
    }
    

    private void Update()
    {
        if(!IsServer)
        {
            return;
        }
        switch (_currentState.Value)
        {
            case State.WaitingToStart:
                break;
            case State.CountdownToStart:
                _countdownToStartTimer.Value -= Time.deltaTime;
                if (_countdownToStartTimer.Value < 0f)
                {
                    _currentState.Value = State.GamePlaying;
                    _gamePlayingTimer.Value = _gamePlayingTimerMax;
                    
                }
                break;
            case State.GamePlaying:
                _gamePlayingTimer.Value -= Time.deltaTime;
                if (_gamePlayingTimer.Value < 0f)
                {
                    _currentState.Value = State.GameOver;                    
                }
                break;
            case State.GameOver:
                break;
        }
        //Debug.Log(_currentState);
    }

    private void LateUpdate()
    {
        if (_autoTestGamePausedState)
        {
            _autoTestGamePausedState = false;

            TestGamePauseState();
        }
    }

    public bool IsLocalPLayerReady()
    {
        return _isLocalPLayerReady;
    }

    public bool IsWaitingToStart()
    {
        return _currentState.Value == State.WaitingToStart;
    }

    public bool IsGamePlaying()
    {
        return _currentState.Value == State.GamePlaying;
    }

    public bool  IsCountdownToStartActive()
    {
        return _currentState.Value == State.CountdownToStart;
    }

    public float GetCountdownToStartTimer()
    {
        return _countdownToStartTimer.Value;
    }

    public bool IsGameOver()
    {
        return _currentState.Value == State.GameOver;
    }

    public float GetGamePlayingTimerNormalized()
    {
        return 1 - (_gamePlayingTimer.Value / _gamePlayingTimerMax);
    }

    public void TogglePauseGame()
    {
        _isLocalGamePaused = !_isLocalGamePaused;

        if (_isLocalGamePaused)
        {
            PauseGameServerRpc();
            OnLocalGamePaused?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            UnPauseGameServerRpc();
            OnLocalGameResume?.Invoke(this, EventArgs.Empty);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void PauseGameServerRpc(ServerRpcParams serverRpcParams = default)
    {
        _playerPauseDictionary[serverRpcParams.Receive.SenderClientId] = true;

        TestGamePauseState();
    }
    [ServerRpc(RequireOwnership = false)]
    private void UnPauseGameServerRpc(ServerRpcParams serverRpcParams = default)
    {
        _playerPauseDictionary[serverRpcParams.Receive.SenderClientId] = false;

        TestGamePauseState();
    }

    private void TestGamePauseState()
    {
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            if (_playerPauseDictionary.ContainsKey(clientId) && _playerPauseDictionary[clientId])
            {
                // Player is paused
                _isGamePaused.Value = true;
                return;
            }

            // Alla players arfe unpasued
            _isGamePaused.Value = false;
        }
    }

}
