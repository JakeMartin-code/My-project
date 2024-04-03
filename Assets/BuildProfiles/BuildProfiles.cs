using UnityEngine;

[CreateAssetMenu(fileName = "NewBuildProfile", menuName = "Builds/BuildProfile")]
public class BuildProfile : ScriptableObject
{
    public string profileName;
    public string profileDescription;
    public SkillData[] recommendedSkills; // List of skills recommended for this build old version
    public PerkDataNode[] recommendedPerks;
}
