using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DeathlyDartEffect", menuName = "PerkEffects/DeathlyDartEffect")]
public class DeathlyDartEffect : PerkEffect
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
