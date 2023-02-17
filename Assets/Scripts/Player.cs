using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; } 

    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs :  EventArgs
    {
        public ClearCounter selectedCounterArg;
    }

    [SerializeField] private float _moveSpeed = 7f;
    [SerializeField] private float _rotateSpeed = 10f;
    [SerializeField] private GameInput _gameInput;
    [SerializeField] private LayerMask _countersLayerMask;


    private bool _isWalking;
    private Vector3 _lastInteractDirection;
    private ClearCounter _selectedCounter;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one player instance!!!!");
        }
        Instance = this;
    }

    private void Start()
    {
        _gameInput.OnInteractAction += GameInput_OnInteractAction;
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        if (_selectedCounter != null)
        {
            _selectedCounter.Interact();
        }
    }

    private void Update()
    {
        HandlePlayerMovement();
        HandlePlayerInteractions();

    }
    
    public bool IsWalking()
    {
        return _isWalking;
    }

    private Vector3 GetPlayerMovementDirection()
    {
        Vector2 inputVector = _gameInput.GetMovementVectorNormalized();
        Vector3 moveDirection = new Vector3(inputVector.x, 0f, inputVector.y);
        return moveDirection;
    }
    
    private void HandlePlayerInteractions()
    {
        Vector3 moveDirection = GetPlayerMovementDirection();

        if (moveDirection != Vector3.zero)
        {
            _lastInteractDirection = moveDirection;
        }


        float interactDistance = 2f;
        RaycastHit raycastHit;

        // Check if theres a counter(has to be in layer mask) in front of the player
        if (Physics.Raycast(transform.position, _lastInteractDirection, out raycastHit, interactDistance, _countersLayerMask))
        {
            // Check that the counter hasd a clearCouonter component
            if(raycastHit.transform.TryGetComponent(out ClearCounter clearCounter))
            {
                if (clearCounter != _selectedCounter)
                {
                    SetSelectedCounter(clearCounter);
                }
            }
            else
            {
                SetSelectedCounter(null);
            }
        }
        else
        {
            SetSelectedCounter(null);
        }

        //Debug.Log(_selectedCounter);
    }

    private void HandlePlayerMovement()
    {
        Vector3 moveDirection = GetPlayerMovementDirection();

        float moveDistance = _moveSpeed * Time.deltaTime;
        float playerRadius = 0.7f;
        float playerHeight = 2f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight,
            playerRadius, moveDirection, moveDistance);

        if (!canMove)
        {
            // Trying to move diagonally

            // Attemp to move on the x axis 
            Vector3 moveDirection_X = new Vector3(moveDirection.x, 0, 0).normalized;
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight,
            playerRadius, moveDirection_X, moveDistance);

            if (canMove)
            {
                // Move on the X axis
                moveDirection = moveDirection_X;
            }
            else
            {
                // X axis not possible. Attemp to move on the Z axis
                Vector3 moveDirection_Z = new Vector3(0, 0, moveDirection.z).normalized;
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight,
                playerRadius, moveDirection_Z, moveDistance);

                if (canMove)
                {
                    // Move to Z axis.
                    moveDirection = moveDirection_Z;
                }
                else
                {
                    //Cannot move diagonally to any direction
                }
            }

        }
        if (canMove)
        {
            transform.position += moveDirection * moveDistance;
        }

        //_isWalking = moveDirection != Vector3.zero;
        if (moveDirection == Vector3.zero)
        {
            _isWalking = false;
        }
        else
        {
            _isWalking = true;
        }

        transform.forward = Vector3.Slerp(transform.forward, moveDirection, Time.deltaTime * _rotateSpeed);
    }

    private void SetSelectedCounter(ClearCounter selectedCounterParameter)
    {
        this._selectedCounter = selectedCounterParameter;

        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            selectedCounterArg = _selectedCounter,
        });
    }


}
