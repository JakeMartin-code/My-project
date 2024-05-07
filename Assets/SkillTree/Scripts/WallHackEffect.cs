using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WallHackEffect", menuName = "PerkEffects/WallHackEffect")]
public class WallHackEffect : PerkEffect
{
    public Material wallhackMaterial; // Assign this material in the Unity editor

    public override void ApplyEffect(GameObject player)
    {
        EnemyManager[] enemies = FindObjectsOfType<EnemyManager>();
        foreach (var enemy in enemies)
        {
            if (enemy.IsOccluded(player.transform))
            {
                enemy.EnableWallhack(wallhackMaterial);
            }
        }
        Debug.Log("Wallhack applied to occluded enemies");
    }

    public override void RemoveEffect(GameObject player)
    {
        EnemyManager[] enemies = FindObjectsOfType<EnemyManager>();
        foreach (var enemy in enemies)
        {
            enemy.DisableWallhack();
        }
        Debug.Log("Wallhack removed");
    }
}
