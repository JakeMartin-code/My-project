using UnityEngine;
using UnityEngine.UI;

public enum PerkName
{
    None,            // No effect
    IncreaseSpeed,
    Invisibility,
    Bunkerdown,
    AOE,
    SharpShooter,
    MarksmansFocus,
    LongshotMastery,
    ScopedAdvantage,
    VantagePoint,
    AssassinsViel,
    ToxicFumes,
    DetonationSurge,
    RadiationZone,
    DebilitatingShots,
    VigorousVandal,
    // Effect to increase speed
    // You can add more effects here as needed
}

public enum FilterTags
{
    stealth,
    damage,
    AOE
}




[CreateAssetMenu(fileName = "NewSkill", menuName = "Skills/SkillDataOld")]
public class SkillData : ScriptableObject
{
    public string skillName;
    public string description;
    public Image icon;
    public int skillLevel;
    public int maxSkillLevel;
    public PerkName effect;
    public FilterTags[] tags;
    public SkillData[] prerequisites; // List of prerequisites for this skill
    public Button skillButton;
    public float increaseSpeedMultiplier;
    public int tempHealth;

    // Other properties for effects, abilities, etc.
}
