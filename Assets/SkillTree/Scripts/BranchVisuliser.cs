using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BranchVisuliser : MonoBehaviour
{
    public Image branchImage;
    [SerializeField] private float thickness = 2f;
    [SerializeField] private Color defaultColor = Color.white;
    [SerializeField] private Color combatColour = Color.red;

    void Awake()
    {
      
     
        AdjustThickness();
    }

    public void AdjustThickness()
    {
        branchImage = GetComponent<Image>();
        if (branchImage == null)
        {
            Debug.LogError("Image component not found", this);
        }
        branchImage.rectTransform.sizeDelta = new Vector2(branchImage.rectTransform.sizeDelta.x, thickness);
     
    }

    public void ChangeColor(Color newColor)
    {
        branchImage.color = newColor;
       
    }

    public void OnButtonPress()
    {
        ChangeColor(combatColour);
    }
}
