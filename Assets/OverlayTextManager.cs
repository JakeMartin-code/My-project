using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OverlayTextManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private string titleMessage;
    private string descriptionMessage;

    public SkillData SkillData;

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        titleMessage = SkillData.skillName;
        descriptionMessage = SkillData.description;
        Overlaybox.overlayBox.SetToolTipTitle(titleMessage, descriptionMessage);
    
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        Overlaybox.overlayBox.HideToolTip();
    }
}
