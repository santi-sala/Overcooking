using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    [SerializeField] private float _volume = 1f;
    private Player _player;
    private float _footstepTimer;
    private float _footstepTimerMax = 0.1f;


    private void Awake()
    {
        _player = GetComponent<Player>();
    }

    private void Update()
    {
        _footstepTimer -= Time.deltaTime;

        if (_footstepTimer < 0f)
        {
            _footstepTimer = _footstepTimerMax;

            if (_player.IsWalking())
            {
                SoundManager.Instance.PlayFootStepsSound(_player.transform.position, _volume);
            }            
        }
    }
}
