using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed = 7f;
    [SerializeField]
    private float _rotateSpeed = 10f;

    private bool isWalking;

    private void Update()
    {
        Vector2 inputVector = new Vector2(0, 0);
        if (Input.GetKey(KeyCode.W))
        {
            inputVector.y = +1;
        }
        
        if(Input.GetKey(KeyCode.S))
        {
            inputVector.y = -1;
        }
        
        if(Input.GetKey(KeyCode.A))
        {
            inputVector.x = -1;
        }
        
        if(Input.GetKey(KeyCode.D))
        {
            inputVector.x = +1;
        }

        inputVector = inputVector.normalized;

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
