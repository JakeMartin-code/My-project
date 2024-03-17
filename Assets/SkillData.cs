using UnityEngine;

public enum EffectType
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

public enum Playstyle
{
    Stealth,
    LongRange,
    CloseRange,
    InstantDamage
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
    public Playstyle playstyle;
    public SkillData[] prerequisites; // List of prerequisites for this skill
    public float increaseSpeedMultiplier;
    public int tempHealth;

    // Other properties for effects, abilities, etc.
}
