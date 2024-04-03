using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TestNodeEffect3", menuName = "PerkEffects/TestNodeEffect3")]
public class TestNodeEffect3 : PerkEffect
{
   
    public override void ApplyEffect(GameObject player)
    {
        Debug.Log("text node 3 perk");
    }
}
