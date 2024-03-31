using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SlidePerkEffect", menuName = "PerkEffects/SlidePerkEffect")]
public class SlidePerkEffect : PerkEffect
{
    public float slideSpeed = 8f;
    public float slideDuration = 1f;

    public override void ApplyEffect(GameObject player)
    {
        var playerMovement = player.GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.ActivateSlideAbility(slideSpeed, slideDuration);
        }
    }
}