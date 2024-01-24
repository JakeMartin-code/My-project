using UnityEngine;

public enum EffectType
{
    None,            // No effect
    IncreaseSpeed,
    Invisibility,
    Bunkerdown,
    AOE
    // Effect to increase speed
    // You can add more effects here as needed
}


[CreateAssetMenu(fileName = "NewSkill", menuName = "Skills/SkillData")]
public class SkillData : ScriptableObject
{
    public string skillName;
    public string description;
    public Sprite icon;
    public int skillLevel;
    public int maxSkillLevel;
    public EffectType effect;
    public SkillData[] prerequisites; // List of prerequisites for this skill
    public float increaseSpeedMultiplier;
    public int tempHealth;

    // Other properties for effects, abilities, etc.
}
