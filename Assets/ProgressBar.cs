using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

[ExecuteInEditMode()]
public class ProgressBar : MonoBehaviour
{
    // Start is called before the first frame update

    public int maximum;
    public int current;
    public float minimum;    
    public Image mask;

    void Start()
    {
        GetCurrentFill();
    }

    // Update is called once per frame
    void Update()
    {
        GetCurrentFill();
    }

   public void GetCurrentFill()
    {
        float currentOffset = current - minimum;
        float maximumOffset = maximum - minimum;
        float fillAmount = currentOffset / maximumOffset;
        mask.fillAmount = fillAmount;
    }
}
