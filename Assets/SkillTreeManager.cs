using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public enum SkillTag
{
    Stealth,
    AOE,
    // Add additional tags as needed
}

public class SkillTreeManager : MonoBehaviour
{

    //Build profiles
    public BuildProfile[] allBuildProfiles; 
    public BuildProfile currentBuildProfile;
    public TextMeshProUGUI buildTitleText;

    public List<SkillData> allSkills; 
    public List<SkillData> unlockedSkills = new List<SkillData>(); 

    public PlayerMovement playerController;
    public int skillPoints;
    public TextMeshProUGUI skillPointText;

    //guidance variables
    public Button stealthButton; 
    public List<Button> stealthSkillButtons; 
    public List<Button> allSkillButtons; 
    private bool isStealthTagActive = false;
    public Color tagActiveColor = Color.blue; 
    public Color defaultButtonColor = Color.white; 


    public void Start()
    {
        skillPointText.SetText("skill point " + skillPoints.ToString());

        stealthButton.onClick.AddListener(ToggleStealthTag);
    }

    public void ToggleStealthTag()
    {
      
        isStealthTagActive = !isStealthTagActive;


        Image stealthButtonImage = stealthButton.GetComponent<Image>();
        if (isStealthTagActive)
        {
            stealthButtonImage.color = tagActiveColor;
        }
        else
        {
            stealthButtonImage.color = defaultButtonColor;
        }


        foreach (Button button in stealthSkillButtons)
        {
            var buttonImage = button.GetComponent<Image>();
            if (isStealthTagActive)
            {
        
                buttonImage.color = tagActiveColor;
            }
            else
            {
           
                buttonImage.color = defaultButtonColor;
            }
        }
    }

    public void ToggleAOETag()
    {

        isStealthTagActive = !isStealthTagActive;


        Image stealthButtonImage = stealthButton.GetComponent<Image>();
        if (isStealthTagActive)
        {
            stealthButtonImage.color = tagActiveColor;
        }
        else
        {
            stealthButtonImage.color = defaultButtonColor;
        }

        foreach (Button button in stealthSkillButtons)
        {
            var buttonImage = button.GetComponent<Image>();
            if (isStealthTagActive)
            {
    
                buttonImage.color = tagActiveColor;
            }
            else
            {
          
                buttonImage.color = defaultButtonColor;
            }
        }
    }

    public void SelectBuildProfile(string profileName)
    {
 
        currentBuildProfile = null;


        foreach (BuildProfile profile in allBuildProfiles)
        {
            if (profile.profileName == profileName)
            {
                currentBuildProfile = profile;
                buildTitleText.text = profileName;
                break; 
            }
        }

    
        if (currentBuildProfile == null)
        {
            Debug.LogError("Build Profile not found: " + profileName);
            return;
        }

        HighlightRecommendedSkills();
    }

    private void HighlightRecommendedSkills()
    {

        if (currentBuildProfile == null) return;

        foreach (Button button in allSkillButtons)
        {
            SkillData skillData = button.GetComponent<SkillUnlockButton>().skillToUnlock; 

            bool isSkillRecommended = false;
            // Manually check if the skill is recommended
            foreach (SkillData recommendedSkill in currentBuildProfile.recommendedSkills)
            {
                if (recommendedSkill == skillData)
                {
                    isSkillRecommended = true;
                    break; 
                }
            }

            Image buttonImage = button.GetComponent<Image>();

        
            if (isSkillRecommended)
            {
                
                buttonImage.color = Color.green;
            }
            else
            {
               
                buttonImage.color = Color.white;
            }
        }
    }


  
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
            case PerkName.IncreaseSpeed:
                ApplySpeedIncrease(skill);
                break;
            case PerkName.Invisibility:
                ApplyInvisibility(skill);
                break;
            case PerkName.Bunkerdown:
               
                break;

            default:
                break;
        }
    }

    public void ApplySpeedIncrease(SkillData skill)
    {
        Debug.Log("ApplySpeedIncrease called with skill: " + skill.skillName);
        if (skill.effect == PerkName.IncreaseSpeed)
        {
            float speedMultiplier = skill.increaseSpeedMultiplier;
            Debug.Log("Speed multiplier: " + speedMultiplier);
            playerController.IncreaseSpeed(speedMultiplier);
        }
    }

    public void ApplyInvisibility(SkillData skill)
    {
        if (skill.effect == PerkName.Invisibility)
        {
            
            playerController.UnlockInvisibility();
        }
    }

   




    // Other functions for managing skill progression and prerequisites
}
