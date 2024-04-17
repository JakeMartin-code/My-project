using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShadowDashEffect", menuName = "PerkEffects/ShadowDashEffect")]
public class ShadowDashEffect : PerkEffect
{
    public float invisibilityDuration = 5.0f; // Duration of invisibility after dash

    public override void ApplyEffect(GameObject player)
    {
        var playerMovement = player.GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.StartDashInvisibility(invisibilityDuration);
            playerMovement.invisibilityDashPerkActive = true;
        }
    }
}
