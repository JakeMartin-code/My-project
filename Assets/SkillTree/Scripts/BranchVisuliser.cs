using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BranchVisuliser : MonoBehaviour
{
    private Image branchImage;
    [SerializeField] private float thickness = 2f; 

    void Awake()
    {
        branchImage = GetComponent<Image>();
        AdjustThickness();
    }

    public void AdjustThickness()
    {
        branchImage.rectTransform.sizeDelta = new Vector2(branchImage.rectTransform.sizeDelta.x, thickness);
    }
}
