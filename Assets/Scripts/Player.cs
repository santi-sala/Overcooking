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

        transform.position += moveDirection * _moveSpeed * Time.deltaTime;

    }

    public bool IsWalking()
    {
        return isWalking;
    }
}
