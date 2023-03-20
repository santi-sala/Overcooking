using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private const string PLAYER_PREFS_SFX_VOLUME = "SfxVolume";

    public static SoundManager Instance { get; private set; }

    [SerializeField] private SO_AudioClipReferences _audioClipReferencesSO;

    private float _volume;


    private void Awake()
    {
        Instance = this;

        _volume = PlayerPrefs.GetFloat(PLAYER_PREFS_SFX_VOLUME, 1f);
    }
    private void Start()
    {
        DeliveryManager.Instance.OnRecipeSuccess += DeliveryManager_Instance_OnRecipeSuccess;
        DeliveryManager.Instance.OnRecipeFailed += DeliveryManager_Instance_OnRecipeFailed;
        CuttingCounter.OnAnyCut += CuttingCounter_OnAnyCut;
        Player.Instance.OnPickUpSomething += PLayer_Instance_OnPickUpSomething;
        BaseCounter.OnAnyObjectPlacedhere += BaseCounter_OnAnyObjectPlacedhere;
        TrashCounter.OnAnyObjectTrashed += TrashCounter_OnAnyObjectTrashed;
    }

    private void TrashCounter_OnAnyObjectTrashed(object sender, System.EventArgs e)
    {
        TrashCounter trashCounter = sender as TrashCounter;
        PlaySound(_audioClipReferencesSO.trash, trashCounter.transform.position);
    }

    private void BaseCounter_OnAnyObjectPlacedhere(object sender, System.EventArgs e)
    {
        BaseCounter baseCounter = sender as BaseCounter;
        PlaySound(_audioClipReferencesSO.objectDrop, baseCounter.transform.position);
    }

    private void PLayer_Instance_OnPickUpSomething(object sender, System.EventArgs e)
    {
        PlaySound(_audioClipReferencesSO.objectPickup, Player.Instance.transform.position);
    }

    private void CuttingCounter_OnAnyCut(object sender, System.EventArgs e)
    {
        CuttingCounter cuttingCounter = sender as CuttingCounter;
        PlaySound(_audioClipReferencesSO.chop, cuttingCounter.transform.position);
    }

    private void DeliveryManager_Instance_OnRecipeFailed(object sender, System.EventArgs e)
    {
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaySound(_audioClipReferencesSO.deliveryFail, deliveryCounter.transform.position);
    }

    private void DeliveryManager_Instance_OnRecipeSuccess(object sender, System.EventArgs e)
    {
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaySound(_audioClipReferencesSO.deliverySuccess, deliveryCounter.transform.position);
    }

    public void PlaySound(AudioClip audioClip, Vector3 position, float volume = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volume);
    }
    public void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volumeMultiplier = 1f)
    {
        PlaySound(audioClipArray[Random.Range(0, audioClipArray.Length)], position, volumeMultiplier * _volume);
    }

    public void PlayFootStepsSound(Vector3 position, float volume)
    {
        PlaySound(_audioClipReferencesSO.footstep, position, volume);
    }

    public void PlayCountdownSound()
    {
        PlaySound(_audioClipReferencesSO.warning, Vector3.zero);
    }

    public void PlayWarningSound(Vector3 position)
    {
        PlaySound(_audioClipReferencesSO.warning, position);
    }

    public void AddVolumeSfx()
    {
        _volume += .1f;
        if (_volume > 1f)
        {
            _volume = 1f;
        }

        SavePlayerPrefSfxVolume();
        
    }

    public void RemoveVolumeSfx()
    {
        _volume -= .1f;
        if (_volume < 0f)
        {
            _volume = 0f;
        }

        SavePlayerPrefSfxVolume();
    }

    public float GetVolume()
    {
        return _volume;
    }

    private void SavePlayerPrefSfxVolume()
    {
        PlayerPrefs.SetFloat(PLAYER_PREFS_SFX_VOLUME, _volume);
        PlayerPrefs.Save();
    }
}
