using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LingeringPantomEffect", menuName = "PerkEffects/LingeringPantomEffect")]
public class LingeringPantomEffect : PerkEffect
{
    public int dashDamage;

    public override void ApplyEffect(GameObject player)
    {
        var playerMovement = player.GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.ActivateDamageDashAbility(dashDamage);
        }
    }
}
