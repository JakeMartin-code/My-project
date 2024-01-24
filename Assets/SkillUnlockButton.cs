using UnityEngine;

public class SkillUnlockButton : MonoBehaviour
{
    public SkillTreeManager skillTreeManager; // Reference to the SkillTreeManager
    public SkillData skillToUnlock; // Assign the specific skill to unlock in the Inspector for each button

    // Function called when the UI button is clicked
    public void UnlockSkill()
    {
        if (skillToUnlock != null)
        {
            skillTreeManager.UnlockSkill(skillToUnlock);
        }
        else
        {
            Debug.LogWarning("Assign SkillData object to unlock in the Inspector!");
        }
    }
}
