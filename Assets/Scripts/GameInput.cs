using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    private const string PLAYER_PREFS_BINDINGS = "InputBindings";
    public static GameInput Instance { get; private set; }

    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;
    public event EventHandler OnPauseEvent;
    public event EventHandler OnBindingRebind;

    public enum Bindings
    {
        Move_Up,
        Move_Down,
        Move_Left, 
        Move_Right, 
        Interact, 
        Interact_Alternate, 
        Pause,
        Gamepad_Interact,
        Gamepad_InteractAlternate,
        Gamepad_Pause,


    }

    private PLayerInputActions _playerInputActions;

    private void Awake()
    {
        Instance = this;

        _playerInputActions = new PLayerInputActions();

        // Checking if there is any save keybindings in player prefs
        if (PlayerPrefs.HasKey(PLAYER_PREFS_BINDINGS))
        {
            _playerInputActions.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFS_BINDINGS));
        }

        _playerInputActions.Player.Enable();

        _playerInputActions.Player.Interact.performed += Interact_performed;
        _playerInputActions.Player.InteractAlternate.performed += InteractAlternate_performed;
        _playerInputActions.Player.Pause.performed += Pause_performed;

        

        //Debug.Log(GetBindingText(Bindings.Pause));
    }

    private void OnDestroy()
    {
        // Unsubscribing to events
        _playerInputActions.Player.Interact.performed -= Interact_performed;
        _playerInputActions.Player.InteractAlternate.performed -= InteractAlternate_performed;
        _playerInputActions.Player.Pause.performed -= Pause_performed;

        // This will clean up the this object created on awake freeing up memory!!
        _playerInputActions.Dispose();
    }

    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPauseEvent?.Invoke(this, EventArgs.Empty);
    }

    private void InteractAlternate_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
        /*
        if (OnInteractAction != null)
        {
            OnInteractAction(this, EventArgs.Empty);
        }
        */
    }

    public Vector2 GetMovementVectorNormalized()
    {        
        Vector2 inputVector = _playerInputActions.Player.Move.ReadValue<Vector2>();

        /*
        if (Input.GetKey(KeyCode.W))
        {
            inputVector.y = +1;
        }

        if (Input.GetKey(KeyCode.S))
        {
            inputVector.y = -1;
        }

        if (Input.GetKey(KeyCode.A))
        {
            inputVector.x = -1;
        }

        if (Input.GetKey(KeyCode.D))
        {
            inputVector.x = +1;
        }
        */

        inputVector = inputVector.normalized;

        return inputVector;
    }

    public  string GetBindingText(Bindings binding)
    {
        switch (binding)
        {
            default:
            case Bindings.Move_Up:
                return _playerInputActions.Player.Move.bindings[1].ToDisplayString();

            case Bindings.Move_Down:
                return _playerInputActions.Player.Move.bindings[2].ToDisplayString();

            case Bindings.Move_Left:
                return _playerInputActions.Player.Move.bindings[3].ToDisplayString();
            
            case Bindings.Move_Right:
                return _playerInputActions.Player.Move.bindings[4].ToDisplayString();

            case Bindings.Interact:
                return _playerInputActions.Player.Interact.bindings[0].ToDisplayString();
                
            case Bindings.Interact_Alternate:
                return _playerInputActions.Player.InteractAlternate.bindings[0].ToDisplayString();

            case Bindings.Pause:
                if (_playerInputActions.Player.Pause.bindings[0].ToDisplayString() == "Escape")
                {
                    return "Esc";
                }
                else
                {
                    return _playerInputActions.Player.Pause.bindings[0].ToDisplayString();
                }
            case Bindings.Gamepad_Interact:
                return _playerInputActions.Player.Interact.bindings[1].ToDisplayString();
            case Bindings.Gamepad_InteractAlternate:
                return _playerInputActions.Player.InteractAlternate.bindings[1].ToDisplayString();
            case Bindings.Gamepad_Pause:
                return _playerInputActions.Player.Pause.bindings[1].ToDisplayString();

        }

    }
    public void RebindBinding(Bindings binding, Action onActionRebound)
    {
        _playerInputActions.Player.Disable();

        InputAction _inputAction;
        int _inputIndex;

        switch (binding)
        {
            default:
            case Bindings.Move_Up:
                _inputAction = _playerInputActions.Player.Move;
                _inputIndex = 1;
                break;
            case Bindings.Move_Down:
                _inputAction = _playerInputActions.Player.Move;
                _inputIndex = 2;
                break;
            case Bindings.Move_Left:
                _inputAction = _playerInputActions.Player.Move;
                _inputIndex = 3;
                break;
            case Bindings.Move_Right:
                _inputAction = _playerInputActions.Player.Move;
                _inputIndex = 4;
                break;
            case Bindings.Interact:
                _inputAction = _playerInputActions.Player.Interact;
                _inputIndex = 0;
                break;
            case Bindings.Interact_Alternate:
                _inputAction = _playerInputActions.Player.InteractAlternate;
                _inputIndex = 0;
                break;
            case Bindings.Pause:
                _inputAction = _playerInputActions.Player.Pause;
                _inputIndex = 0;
                break;
            case Bindings.Gamepad_Interact:
                _inputAction = _playerInputActions.Player.Interact;
                _inputIndex = 1;
                break;
            case Bindings.Gamepad_InteractAlternate:
                _inputAction = _playerInputActions.Player.InteractAlternate;
                _inputIndex = 1;
                break;
            case Bindings.Gamepad_Pause:
                _inputAction = _playerInputActions.Player.Pause;
                _inputIndex = 1;
                break;
        }
        _inputAction.PerformInteractiveRebinding(_inputIndex).OnComplete(callback =>
        {
            //Debug.Log(callback.action.bindings[1].path);
            //Debug.Log(callback.action.bindings[1].overridePath);
            callback.Dispose();
            _playerInputActions.Player.Enable();

            // This is a DELEGATE which is will fire/invoke a LAMBDA function in UI_Options.RebindKeyBinding(), like en event
            onActionRebound();

            // Saving the new rebinded keys to player prefs
            PlayerPrefs.SetString(PLAYER_PREFS_BINDINGS, _playerInputActions.SaveBindingOverridesAsJson());
            PlayerPrefs.Save();

            OnBindingRebind?.Invoke(this, EventArgs.Empty);
        }).Start();
    }
}
