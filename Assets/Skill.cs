using UnityEngine;

public class Skill : MonoBehaviour
{
    // This script might handle other player-related functionalities or interactions
    // For example, it could manage player stats, health, movement, etc.
    // It might also react to the availability of specific skills or their effects.

    // Example method handling an action related to a specific skill
    public void UseSkill(SkillData skillData)
    {
        // Perform actions related to the skill being used
        Debug.Log("Player using skill: " + skillData.skillName);
        // Implement logic specific to this skill's usage
    }
}
