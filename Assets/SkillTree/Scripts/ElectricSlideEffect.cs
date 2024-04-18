using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ElectricSlideEffect", menuName = "PerkEffects/ElectricSlideEffect")]
public class ElectricSlideEffect : PerkEffect
{
    public override void ApplyEffect(GameObject player)
    {
        Debug.Log("appiled");
    }

    public override void RemoveEffect(GameObject player)
    {
        Debug.Log("removed");
    }

}
