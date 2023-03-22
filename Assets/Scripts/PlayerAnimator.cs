using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerAnimator : NetworkBehaviour
{
    
    private Animator _animator;

    [SerializeField]
    private Player _player;

    private const string IS_WALKING = "IsWalking";

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!IsOwner)
        {
            return;
        }
        _animator.SetBool(IS_WALKING, _player.IsWalking());
    }
}
