using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuildDropdownScript : MonoBehaviour
{
    public TMP_Dropdown buildDropsownMenu;
    public TextMeshProUGUI buildNameText;

    public PerkTreeManager perkManager;

    public BuildProfile[] buildProfiles;

    public void DropDownValue()
    {
        int pickedEntryIndex = buildDropsownMenu.value;
        // Check if the picked index corresponds to a build profile
        if (pickedEntryIndex > 0 && pickedEntryIndex <= buildProfiles.Length)
        {
            // Adjust for 0-index and consider "No Build" as the first option
            BuildProfile selectedProfile = buildProfiles[pickedEntryIndex - 1];
            perkManager.HighlightBuildPerks(selectedProfile.recommendedPerks);
        }
        else
        {
            perkManager.ClearHighlighting(); // Clear any existing highlighting
        }
    }

}
