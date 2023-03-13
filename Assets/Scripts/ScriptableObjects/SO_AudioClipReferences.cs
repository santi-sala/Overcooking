using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class SO_AudioClipReferences : ScriptableObject
{
    public AudioClip[] chop;
    public AudioClip[] deliveryFail;
    public AudioClip[] deliverySuccess;
    public AudioClip[] footstep;
    public AudioClip[] objectPickup;
    public AudioClip[] objectDrop;
    public AudioClip stoveSizzle;
    public AudioClip[] trash;
    public AudioClip[] warning;

}
