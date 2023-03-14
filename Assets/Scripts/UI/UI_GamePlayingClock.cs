using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_GamePlayingClock : MonoBehaviour
{
    [SerializeField] private Image _timerImage;

    private void Update()
    {
        _timerImage.fillAmount = GameManager.Instance.GetGamePlayingTimerNormalized();
    }
}
