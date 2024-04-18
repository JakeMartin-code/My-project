using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TestNodeEffect", menuName = "PerkEffects/TestNodeEffect")]
public class TestNodeEffect : PerkEffect
{
   
    public override void ApplyEffect(GameObject player)
    {
        Debug.Log("text ndoe perk");
    }

    public override void RemoveEffect(GameObject player)
    {

    }
}
