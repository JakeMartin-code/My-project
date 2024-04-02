using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DoubleDamagePerkEffect", menuName = "PerkEffects/DoubleDamagePerkEffect")]
public class SlidingDoubleDamage : PerkEffect
{
    public override void ApplyEffect(GameObject player)
    {
        var weapon = player.GetComponentInChildren<WeaponBehavior>(); 
        if (weapon != null)
        {
            weapon.doubleDamageWhileSliding = true; 
        }
        else
        {
            Debug.Log("no weapon fpound while sliding");
        }
    }
}
