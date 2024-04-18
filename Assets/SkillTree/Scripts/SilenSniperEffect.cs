using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SilenSniperEffect", menuName = "PerkEffects/SilenSniperEffect")]
public class SilenSniperEffect : PerkEffect
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
