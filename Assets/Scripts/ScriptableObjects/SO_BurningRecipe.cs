using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class SO_BurningRecipe : ScriptableObject
{
    public SO_KitchenObjects input;
    public SO_KitchenObjects output;
    public float burningTimerMax;
}
