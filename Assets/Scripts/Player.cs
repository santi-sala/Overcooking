using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent
{
    public static Player Instance { get; private set; } 

    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs :  EventArgs
    {
        public BaseCounter selectedCounterArg;
    }

    [SerializeField] private float _moveSpeed = 7f;
    [SerializeField] private float _rotateSpeed = 10f;
    [SerializeField] private GameInput _gameInput;
    [SerializeField] private LayerMask _countersLayerMask;
    [SerializeField] private Transform _KitchenObjectHoldPoint;


    private bool _isWalking;
    private Vector3 _lastInteractDirection;
    private BaseCounter _selectedCounter;
    private KitchenObject _kitchenObject;

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
        _gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
    }

    private void GameInput_OnInteractAlternateAction(object sender, EventArgs e)
    {
        if (_selectedCounter != null)
        {
            _selectedCounter.InteractAlternate(this);
        }
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {
        if (_selectedCounter != null)
        {
            _selectedCounter.Interact(this);
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

        // Check if theres a baseCounter(has to be in layer mask) in front of the player
        if (Physics.Raycast(transform.position, _lastInteractDirection, out raycastHit, interactDistance, _countersLayerMask))
        {
            // Check that the baseCounter hasd a clearCouonter component
            if(raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                if (baseCounter != _selectedCounter)
                {
                    SetSelectedCounter(baseCounter);
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
            canMove = moveDirection.x != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight,
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
                canMove = moveDirection.z != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight,
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

    private void SetSelectedCounter(BaseCounter selectedCounterParameter)
    {
        this._selectedCounter = selectedCounterParameter;

        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            selectedCounterArg = _selectedCounter,
        });
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return _KitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this._kitchenObject = kitchenObject;
    }

    public KitchenObject GetKitchenObject()
    {
        return _kitchenObject;
    }

    public void ClearKitchenObject()
    {
        _kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return _kitchenObject != null;
    }
}
