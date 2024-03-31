using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkTreeManager : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private int perkPoints = 5; 
    private Dictionary<string, bool> unlockedPerks = new Dictionary<string, bool>();


    public void UnlockSkill(PerkDataNode perk)
    {
        if (perkPoints >= perk.cost && ArePrerequisitesMet(perk))
        {
            perkPoints -= perk.cost;
            perk.perkEffect.ApplyEffect(player);
            unlockedPerks[perk.perkID] = true; 

          
        }
        else
        {
            Debug.Log("Not enough perk points or prerequisites not met for " + perk.perkName);
        }
    }


    private bool ArePrerequisitesMet(PerkDataNode perk)
    {
        foreach (var prerequisite in perk.prerequisites)
        {
            if (!unlockedPerks.TryGetValue(prerequisite.perkID, out bool isUnlocked) || !isUnlocked)
            {
                return false;
            }
        }
        return true;
    }
  
    private void UpdateSkillTreeUI(PerkDataNode perk)
    {
      
    }

    private void UpdateSkillPointsUI()
    {
        Debug.Log("Remaining Skill Points: " + perkPoints);
    }
}
