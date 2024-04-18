using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SmokeBombEffect", menuName = "PerkEffects/SmokeBombEffect")]
public class SmokeBombEffect : PerkEffect
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
