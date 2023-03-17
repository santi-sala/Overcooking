using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private const string PLAYER_PREFS_MUSIC_VOLUME = "MusicVolume";
    public static MusicManager Instance { get; private set; }

    private AudioSource _audioSource;
    private float _volume;

    private void Awake()
    {
        Instance = this;

        _audioSource = GetComponent<AudioSource>();

        _volume = PlayerPrefs.GetFloat(PLAYER_PREFS_MUSIC_VOLUME, .3f);
        _audioSource.volume = _volume;
    }
    public void AddVolumeMusic()
    {
        _volume += .1f;
        if (_volume > 1f)
        {
            _volume = 1f;
        }

        _audioSource.volume = _volume;

        SavePlayerPrefSfxVolume();
    }

    public void RemoveVolumeMusic()
    {
        _volume -= .1f;
        if (_volume < 0f)
        {
            _volume = 0f;
        }

        _audioSource.volume = _volume;

        SavePlayerPrefSfxVolume();
    }

    public float GetVolume()
    {
        return _volume;
    }

    private void SavePlayerPrefSfxVolume()
    {
        PlayerPrefs.SetFloat(PLAYER_PREFS_MUSIC_VOLUME, _volume);
        PlayerPrefs.Save();
    }
}
