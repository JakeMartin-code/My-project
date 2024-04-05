using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ConfirmRecomendedPerkOptions : MonoBehaviour
{
    [SerializeField] TMP_Dropdown playStyleDropdown;
    [SerializeField] TMP_Dropdown rangeDropdown;

    public PerkTreeManager perkTreeManager;

    public void OnConfirmPreferences()
    {
        string selectedPlayStyle = playStyleDropdown.options[playStyleDropdown.value].text;
        string selectedRange = rangeDropdown.options[rangeDropdown.value].text;

        perkTreeManager.RecommendPerksBasedOnUserInput(selectedPlayStyle, selectedRange);
    }
}
