using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PerkUnlockButton : MonoBehaviour
{
    public PerkDataNode perkData; // Assign this via the Unity Editor
    private Button button; // The button component on this GameObjecT
    public PerkTreeManager perkTree;

    void UnlockSkill()
    {
        perkTree.UnlockSkill(perkData); // Unlock the skill associated with this button
    }
}
