using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DoubleDamagePerkEffect", menuName = "PerkEffects/DoubleDamagePerkEffect")]
public class SlidingDoubleDamage : PerkEffect
{
    public override void ApplyEffect(GameObject player)
    {
        var weapon = player.GetComponentInChildren<WeaponBehavior>(); // Assuming WeaponBehavior is attached to a child of the player
        if (weapon != null)
        {
            weapon.doubleDamageWhileSliding = true; // Assuming this field is added to WeaponBehavior to control damage
        }
        else
        {
            Debug.Log("no weapon fpound while sliding");
        }
    }
}
