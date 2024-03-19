using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public enum DataType
{
    Skill,
    BuildProfile
}

public class OverlayTextManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private string titleMessage;
    private string descriptionMessage;

    

    public SkillData SkillData;
    public BuildProfile BuildProfile;

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
            default:
                break;
        }
    }

    private void ShowSkillTooltip()
    {
        if (SkillData != null)
        {
            string titleMessage = SkillData.skillName;
            string descriptionMessage = SkillData.description;
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

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        Overlaybox.overlayBox.HideToolTip();
    }
}




    /*
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        titleMessage = SkillData.skillName;
        descriptionMessage = SkillData.description;
        Overlaybox.overlayBox.SetToolTipTitle(titleMessage, descriptionMessage);
    }



    public void OnPointerExit(PointerEventData pointerEventData)
    {
        Overlaybox.overlayBox.HideToolTip();
    }    */
