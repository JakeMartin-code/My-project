using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GhostStrikeEffect", menuName = "PerkEffects/GhostStrikeEffect")]
public class GhostStrikeEffect : PerkEffect
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
