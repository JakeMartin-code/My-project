using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShadowStepEffect", menuName = "PerkEffects/ShadowStepEffect")]
public class ShadowStepEffect : PerkEffect
{
    public override void ApplyEffect(GameObject player)
    {
        var playerMovement = player.GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.EnableCrouchInvisibilityPerk(); // Enable invisibility perk when this effect is applied
        }
    }

    public override void RemoveEffect(GameObject player)
    {
      
    }


}
