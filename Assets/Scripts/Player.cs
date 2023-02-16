using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed = 7f;
    [SerializeField]
    private float _rotateSpeed = 10f;
    [SerializeField]
    private GameInput gameInput;

    private bool isWalking;

    private void Update()
    {

        Vector2 inputVector = gameInput.GetMovementVectorNormalized(); 
        Vector3 moveDirection = new Vector3(inputVector.x, 0f, inputVector.y);

        float moveDistance = _moveSpeed * Time.deltaTime;
        float playerRadius = 0.7f;
        float playerHeight = 2f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, 
            playerRadius,  moveDirection, moveDistance);

        if(!canMove)
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

        //isWalking = moveDirection != Vector3.zero;
        if (moveDirection == Vector3.zero)
        {
            isWalking = false;
        }
        else
        {
            isWalking = true;
        }

        transform.forward = Vector3.Slerp(transform.forward, moveDirection, Time.deltaTime * _rotateSpeed);


    }

    public bool IsWalking()
    {
        return isWalking;
    }
}
