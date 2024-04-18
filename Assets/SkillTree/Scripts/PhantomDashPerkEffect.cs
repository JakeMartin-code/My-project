using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SlidePerkEffect", menuName = "PerkEffects/PhantomDashEffect")]
public class PhantomDashPerkEffect : PerkEffect
{
    public float dashSpeed;
    public float dashDuration;

    public override void ApplyEffect(GameObject player)
    {
        var playerMovement = player.GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.ActivateDashAbility(dashSpeed, dashDuration);
        }
    }

    public override void RemoveEffect(GameObject player)
    {

    }
}
