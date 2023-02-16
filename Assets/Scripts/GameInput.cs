using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    private PLayerInputActions _playerInputActions;

    private void Awake()
    {
        _playerInputActions = new PLayerInputActions();
        _playerInputActions.Player.Enable();
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
