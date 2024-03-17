using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OverlayTextManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string titleMessage;
    public string descriptionMessage;

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        Overlaybox.overlayBox.SetToolTipTitle(titleMessage, descriptionMessage);
    
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        Overlaybox.overlayBox.HideToolTip();
    }
}
