using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public enum DataType
{
    Skill,
    BuildProfile,
    Weapon
}

public class OverlayTextManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
   
    public PerkDataNode perkData;
    public BuildProfile BuildProfile;
    public WeaponData weaponData;

    public DataType CurrentDataType;

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        switch (CurrentDataType)
        {
            case DataType.Skill:
                ShowSkillTooltip();
                break;
            case DataType.BuildProfile:
                ShowBuildProfileTooltip();
                break;
            case DataType.Weapon:
                ShowWeaponTooltip();
                break;
            default:
                break;
        }
    }

    private void ShowSkillTooltip()
    {
        if (perkData != null)
        {
            string titleMessage = perkData.perkName;
            string descriptionMessage = perkData.description;
            Overlaybox.overlayBox.SetToolTipTitle(titleMessage, descriptionMessage);
        }
    }

    private void ShowBuildProfileTooltip()
    {
        if (BuildProfile != null)
        {
            string titleMessage = BuildProfile.profileName;
            string descriptionMessage = BuildProfile.profileDescription;
            Overlaybox.overlayBox.SetToolTipTitle(titleMessage, descriptionMessage);
        }
    }

    private void ShowWeaponTooltip()
    {
        if (weaponData != null)
        {
            string titleMessage = weaponData.name;
            string descriptionMessage = weaponData.baseDamage.ToString();
            Overlaybox.overlayBox.SetToolTipTitle(titleMessage, descriptionMessage);
        }
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        Overlaybox.overlayBox.HideToolTip();
    }
}

