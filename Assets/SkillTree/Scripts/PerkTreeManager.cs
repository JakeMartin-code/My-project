using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class PerkTreeManager : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    public Dictionary<string, bool> unlockedPerks = new Dictionary<string, bool>();
    private Dictionary<string, RectTransform> perkUITransforms = new Dictionary<string, RectTransform>();

    public List<PerkDataNode> allSkills;
    public GameObject branchPrefab;

    public PlayerStats PlayerStats;

    private Button perkButton;
    private Image buttonImage;

    public Color defaultPerkColor = Color.white;
    public Color purchasedColor = Color.white;
    public Color highlightedPerkColor = Color.yellow;
    public Color lockedColor = Color.red;
    public Color decisionTreeColor = Color.white;
    public Color recommendNextPerkColor = Color.white;



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

                perkButton = uiElement.GetComponent<Button>();
                if (perkButton != null)
                {
                    buttonImage = perkButton.GetComponent<Image>();
                    Debug.Log("found buttons");
                    UpdatePerkButtonColor(skill);
                }
            }
            else
            {
                Debug.LogError("UI element not found for skill: " + skill.perkID);
            }
        }
    }

    private void UpdatePerkButtonColor(PerkDataNode perk)
    {
        // Attempt to retrieve the RectTransform for the given perk ID
        if (perkUITransforms.TryGetValue(perk.perkID, out RectTransform uiElement))
        {
            // Retrieve the Button component from the uiElement
            perkButton = uiElement.GetComponent<Button>();
            if (perkButton != null)
            {
                // Directly retrieve and update the Image component of the perk button
                buttonImage = perkButton.GetComponent<Image>();
                Color targetColor = defaultPerkColor; // Default assumption

                // Check if the perk has been purchased
                if (unlockedPerks.TryGetValue(perk.perkID, out bool isUnlocked) && isUnlocked)
                {
                    // Perk has been purchased
                    targetColor = purchasedColor;
                }
                else if (ArePrerequisitesMet(perk))
                {
                    // Perk is unlockable but not yet purchased
                    targetColor = defaultPerkColor;
                }
                else
                {
                    // Perk is locked
                    targetColor = lockedColor;
                }

                buttonImage.color = targetColor;
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
        if (PlayerStats.perkPoints >= perk.cost && ArePrerequisitesMet(perk))
        {
            PlayerStats.perkPoints -= perk.cost;
            perk.perkEffect.ApplyEffect(player);
            unlockedPerks[perk.perkID] = true;

            foreach (var skill in allSkills)
            {
                UpdatePerkButtonColor(skill);
            }
        }
        else
        {
            Debug.Log("Not enough perk points or prerequisites not met for " + perk.perkName);
        }
    }



    private bool ArePrerequisitesMet(PerkDataNode perk)
    {
        // Check if the perk has no prerequisites, if so, it can be unlocked.
        if (perk.prerequisites == null || perk.prerequisites.Count == 0)
        {
            return true;
        }

        foreach (var prerequisite in perk.prerequisites)
        {
            // If any one of the prerequisites is unlocked, return true.
            if (unlockedPerks.TryGetValue(prerequisite.perkID, out bool isUnlocked) && isUnlocked)
            {
                return true;
            }
        }

        // If none of the prerequisites are unlocked, return false.
        return false;

    }

    public void HighlightBuildPerks(PerkDataNode[] recommendedPerks)
    {
        // First, clear existing highlights
        ClearHighlighting();

        // Then, highlight recommended perks
        foreach (var perk in recommendedPerks)
        {
            if (perkUITransforms.TryGetValue(perk.perkID, out RectTransform uiElement))
            {
                var buttonImage = uiElement.GetComponent<Image>();
                if (buttonImage != null)
                {
                    buttonImage.color = highlightedPerkColor;
                }
            }
        }
    }

    public void ClearHighlighting()
    {
        foreach (var perk in allSkills)
        {
            if (perkUITransforms.TryGetValue(perk.perkID, out RectTransform uiElement))
            {
                var buttonImage = uiElement.GetComponent<Image>();
                if (buttonImage != null)
                {
                    // Reset to default color or based on whether it's unlocked
                    UpdatePerkButtonColor(perk);
                }
            }
        }
    }




    // decicion tree implementation
    public void RecommendNextPerk()
    {
        // Gather available perks based on playstyle
        List<PerkDataNode> availableCombatPerks = new List<PerkDataNode>();
        List<PerkDataNode> availableInteractionPerks = new List<PerkDataNode>();

        foreach (var skill in allSkills)
        {
            if (!unlockedPerks.ContainsKey(skill.perkID) || !unlockedPerks[skill.perkID]) // Check if not unlocked
            {
                if (ArePrerequisitesMet(skill)) // Check if prerequisites are met
                {
                    if (skill.playStyle == PerkPlayStyle.Combat)
                    {
                        availableCombatPerks.Add(skill);
                    }
                    else if (skill.playStyle == PerkPlayStyle.Interaction)
                    {
                        availableInteractionPerks.Add(skill);
                    }
                }
            }
        }
        
        // Determine the recommendation based on completion count and availability
        int killMissionsCompleted = MissionTracker.Instance.GetMissionTypeCompletions(MissionType.Kill);
        int interactMissionsCompleted = MissionTracker.Instance.GetMissionTypeCompletions(MissionType.interact);

        if (killMissionsCompleted > interactMissionsCompleted && availableCombatPerks.Count > 0)
        {
            HighlightPerksByPlayStyle(availableCombatPerks);
        }
        else if (availableInteractionPerks.Count > 0)
        {
            HighlightPerksByPlayStyle(availableInteractionPerks);
        }
        // You could add an else statement to handle the case where no perks are available
    }


    void HighlightPerksByPlayStyle(List<PerkDataNode> perksToHighlight)
    {
        ClearHighlighting(); // Clear existing highlights first

        foreach (var perk in perksToHighlight)
        {
            HighlightPerk(perk.perkID);
        }
    }

    void HighlightPerk(string perkID)
    {
        if (perkUITransforms.TryGetValue(perkID, out RectTransform uiElement))
        {
            var buttonImage = uiElement.GetComponent<Image>();
            if (buttonImage != null)
            {
                buttonImage.color = decisionTreeColor;
            }
        }
    }

    //recommender system
     public void RecommendPerksBasedOnUserInput(string playStyle, string range)
     {
         Debug.Log($"[PerkTreeManager] Starting recommendation process for PlayStyle: {playStyle}, Range: {range}. Total perks available: {allSkills.Count}");

         bool isPlayStyleBalanced = playStyle == "Balanced";
         bool isRangeBalanced = range == "Balanced";

         var filteredPerks = allSkills.Where(perk =>
             (!unlockedPerks.ContainsKey(perk.perkID) || !unlockedPerks[perk.perkID]) && ArePrerequisitesMet(perk)).ToList();
         Debug.Log($"[PerkTreeManager] After initial filtering (unlocked & prerequisites met): {filteredPerks.Count} perks remain. Perks: {string.Join(", ", filteredPerks.Select(perk => perk.perkID))}");

         var recommendationReasons = new List<string>(); // Collect reasons for debug output
         var recommendedPerks = new List<PerkDataNode>();

         foreach (var perk in filteredPerks)
         {
             string reason = ""; // To collect the reason for inclusion or exclusion
             if (!isPlayStyleBalanced && perk.playStyle.ToString() != playStyle)
             {
                 reason = $"Excluded: PlayStyle '{perk.playStyle}' does not match selected '{playStyle}'.";
             }
             else if (!isRangeBalanced && perk.engagementRange.ToString() != range)
             {
                 reason = $"Excluded: EngagementRange '{perk.engagementRange}' does not match selected '{range}'.";
             }
             else
             {
                 recommendedPerks.Add(perk);
                 reason = $"Included: Matches PlayStyle '{playStyle}' and Range '{range}'.";
             }
             recommendationReasons.Add($"{perk.perkID}: {reason}");
         }

         // Handle balanced separately to add reasons for inclusion or exclusion
         if (isPlayStyleBalanced)
         {
             recommendedPerks.AddRange(GetBalancedPerksByPlayStyle(filteredPerks, recommendationReasons));
         }
         if (isRangeBalanced)
         {
             recommendedPerks.AddRange(GetBalancedPerksByRange(filteredPerks, recommendationReasons));
         }

         Debug.Log($"[PerkTreeManager] After applying playStyle and range filters: {recommendedPerks.Count} perks to recommend. Reasons:\n{string.Join("\n", recommendationReasons)}");

         if (recommendedPerks.Count == 0)
         {
             Debug.LogWarning("[PerkTreeManager] No perks recommended. This may be due to all suitable perks being unlocked or not meeting prerequisites.");
         }

         HighlightRecommendedPerks(recommendedPerks);
     }
    

    private void HighlightRecommendedPerks(List<PerkDataNode> perks)
    {
        ClearHighlighting();
        foreach (var perk in perks)
        {
            HighlightRecommendedPerk(perk.perkID);
        }
    }

    private void HighlightRecommendedPerk(string perkID)
    {
        if (perkUITransforms.TryGetValue(perkID, out RectTransform uiElement))
        {
            var buttonImage = uiElement.GetComponent<Image>();
            if (buttonImage != null)
            {
                buttonImage.color = recommendNextPerkColor;
                Debug.Log($"[PerkTreeManager] Highlighting perk: {perkID}");
            }
            else
            {
                Debug.LogError($"[PerkTreeManager] Failed to find button image for perk: {perkID}");
            }
        }
        else
        {
            Debug.LogError($"[PerkTreeManager] Failed to find UI element for perk: {perkID}");
        }
    }


    // Adjust these methods to take a List<string> for collecting reasons
    List<PerkDataNode> GetBalancedPerksByPlayStyle(List<PerkDataNode> perks, List<string> reasons)
    {
        // Implement according to how you want to balance the perks
        var combatPerks = perks.Where(perk => perk.playStyle == PerkPlayStyle.Combat).Take(2);
        reasons.AddRange(combatPerks.Select(p => $"{p.perkID}: Included for balanced PlayStyle (Combat)."));
        var stealthPerks = perks.Where(perk => perk.playStyle == PerkPlayStyle.Stealth).Take(2);
        reasons.AddRange(stealthPerks.Select(p => $"{p.perkID}: Included for balanced PlayStyle (Stealth)."));

        return combatPerks.Concat(stealthPerks).ToList();
    }

    List<PerkDataNode> GetBalancedPerksByRange(List<PerkDataNode> perks, List<string> reasons)
    {
        // Mix close and far range perks
        var closePerks = perks.Where(perk => perk.engagementRange == EngagementRange.Close).Take(2);
        reasons.AddRange(closePerks.Select(p => $"{p.perkID}: Included for balanced Range (Close)."));
        var farPerks = perks.Where(perk => perk.engagementRange == EngagementRange.Far).Take(2);
        reasons.AddRange(farPerks.Select(p => $"{p.perkID}: Included for balanced Range (Far)."));

        return closePerks.Concat(farPerks).ToList();
    }
}
