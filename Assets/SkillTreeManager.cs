using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SkillTreeManager : MonoBehaviour
{
    public List<SkillData> allSkills; // List of all SkillData Scriptable Objects
    public List<SkillData> unlockedSkills = new List<SkillData>(); // List of unlocked skills for the player
    public PlayerController playerController;
    public int skillPoints;
    public TextMeshProUGUI skillPointText;

    public void Start()
    {
        skillPointText.SetText("skill point " + skillPoints.ToString());
    }

    // Function to unlock a new skill for the player
    public void UnlockSkill(SkillData newSkill)
    {
        if(skillPoints >0)
        {
            if (!unlockedSkills.Contains(newSkill))
            {
                // Implement unlocking logic here (e.g., make the skill available to the player)
                unlockedSkills.Add(newSkill);
                Debug.Log("Unlocked skill: " + newSkill.skillName);
                skillPoints--;
                skillPointText.SetText("skill point " + skillPoints.ToString());
                LogUnlockedSkills(); // Log unlocked skills after each unlock operation
                ApplySkillEffects(newSkill);
            }
            else
            {
                Debug.Log("Skill already unlocked: " + newSkill.skillName);
            }
        }
        else
        {
            Debug.Log("you have no perks sonnn");
        }
    }

    // Debug function to log all unlocked skills
    public void LogUnlockedSkills()
    {
        Debug.Log("Unlocked Skills:");
        foreach (SkillData skill in unlockedSkills)
        {
            Debug.Log("- " + skill.skillName);
        }
    }

    public void ApplySkillEffects(SkillData skill)
    {
        switch (skill.effect)
        {
            case EffectType.IncreaseSpeed:
                ApplySpeedIncrease(skill);
                break;
            case EffectType.Invisibility:
                ApplyInvisibility(skill);
                break;
            case EffectType.Bunkerdown:
                ApplyBunkerdown(skill);
                break;



            default:
                break;
        }
    }

    public void ApplySpeedIncrease(SkillData skill)
    {
        Debug.Log("ApplySpeedIncrease called with skill: " + skill.skillName);
        if (skill.effect == EffectType.IncreaseSpeed)
        {
            float speedMultiplier = skill.increaseSpeedMultiplier;
            Debug.Log("Speed multiplier: " + speedMultiplier);
            playerController.IncreaseSpeed(speedMultiplier);
        }
    }

    public void ApplyInvisibility(SkillData skill)
    {
        if (skill.effect == EffectType.Invisibility)
        {
            
            playerController.UnlockInvisibility();
        }
    }

    public void ApplyBunkerdown(SkillData skill)
    {
        if (skill.effect == EffectType.Bunkerdown)
        {
            int tempHealth = skill.tempHealth;
            playerController.Bunkerdown(tempHealth);
        }
    }




    // Other functions for managing skill progression and prerequisites
}
