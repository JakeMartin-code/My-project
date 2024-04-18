using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TestNodeEffect2", menuName = "PerkEffects/TestNodeEffect2")]
public class TestNodeEffect2 : PerkEffect
{
   
    public override void ApplyEffect(GameObject player)
    {
        Debug.Log("text node 2 perk");
    }

    public override void RemoveEffect(GameObject player)
    {

    }
}
