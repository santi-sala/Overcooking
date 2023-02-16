using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    public event EventHandler OnInteractAction;
    private PLayerInputActions _playerInputActions;

    private void Awake()
    {
        _playerInputActions = new PLayerInputActions();
        _playerInputActions.Player.Enable();

        _playerInputActions.Player.Interact.performed += Interact_performed;
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
}
