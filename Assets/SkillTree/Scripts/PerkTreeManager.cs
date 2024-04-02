using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PerkTreeManager : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private int perkPoints = 5; 
    public Dictionary<string, bool> unlockedPerks = new Dictionary<string, bool>();
    private Dictionary<string, RectTransform> perkUITransforms = new Dictionary<string, RectTransform>();

    public List<PerkDataNode> allSkills;
    public GameObject branchPrefab;

    private void Start()
    {
        InitializePerkUIElements();
        CreateAllBranches();
        Debug.Log("Starting PerkTreeManager with " + allSkills.Count + " skills and branchPrefab: " + (branchPrefab != null));
    }

 

    void InitializePerkUIElements()
    {
        foreach (var skill in allSkills)
        {
          
            RectTransform uiElement = GameObject.Find(skill.perkID)?.GetComponent<RectTransform>();
            if (uiElement != null)
            {
                perkUITransforms[skill.perkID] = uiElement;
                Debug.Log("Assigned UI element for skill: " + skill.perkID);
            }
            else
            {
                Debug.LogError("UI element not found for skill: " + skill.perkID);
            }
        }
    }



    void CreateAllBranches()
    {
        foreach (var skill in allSkills)
        {
            if (skill.prerequisites != null)
            {
                foreach (var prerequisite in skill.prerequisites)
                {
                    CreateBranch(prerequisite, skill);
                }
            }
        }
    }

    void CreateBranch(PerkDataNode startSkill, PerkDataNode endSkill)
    {
        if (perkUITransforms.TryGetValue(startSkill.perkID, out RectTransform startRectTransform) &&
         perkUITransforms.TryGetValue(endSkill.perkID, out RectTransform endRectTransform))
        {
            GameObject branchInstance = Instantiate(branchPrefab, transform);
            Debug.Log("Creating branch between " + startSkill.perkID + " and " + endSkill.perkID);

            RectTransform branchRT = branchInstance.GetComponent<RectTransform>();

            Vector2 startPos = startRectTransform.position;
            Vector2 endPos = endRectTransform.position;
            Vector2 direction = endPos - startPos;
            float distance = Vector2.Distance(startPos, endPos);
            branchRT.sizeDelta = new Vector2(distance, branchRT.sizeDelta.y); 
            branchRT.position = startPos;
            branchRT.pivot = new Vector2(0, 0.5f);
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            branchRT.rotation = Quaternion.Euler(0, 0, angle);

            BranchVisuliser visualizer = branchInstance.GetComponent<BranchVisuliser>();
            if (visualizer != null)
            {
                visualizer.AdjustThickness(); 
            }
        }
        else
        {
            Debug.LogError("Failed to find RectTransforms for branch between " + startSkill.perkID + " and " + endSkill.perkID);
        }
    }

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
