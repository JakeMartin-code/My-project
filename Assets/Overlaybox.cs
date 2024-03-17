using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Overlaybox : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;


    public static Overlaybox overlayBox;


    private void Awake()
    {
        if(overlayBox != null && overlayBox != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            overlayBox = this;
        }
    }

    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Input.mousePosition;
    }

    public void SetToolTipTitle(string title, string description)
    {
        gameObject.SetActive(true);
        titleText.text = title;
        descriptionText.text = description;

    }


    public void HideToolTip()
    {
        gameObject.SetActive(false);
        titleText.text = string.Empty;
        descriptionText.text = string.Empty;

    }
}
