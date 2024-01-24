using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ButtonTrigger : MonoBehaviour
{
    public Button[] buttonsToChangeColor;
    public Color targetColor;

    private Dictionary<Button, Color> originalColors;

    public void ChangeButtonColors()
    {
        // Ensure original colors are stored before making any changes
        if (originalColors == null || originalColors.Count == 0)
        {
            StoreOriginalColors();
        }

        // Revert all buttons to their original colors
        foreach (var kvp in originalColors)
        {
            kvp.Key.image.color = kvp.Value;
        }

        // Change the color of each specified button
        foreach (Button button in buttonsToChangeColor)
        {
            button.image.color = targetColor;
        }
    }

    private void StoreOriginalColors()
    {
        // Initialize the dictionary to store original colors
        originalColors = new Dictionary<Button, Color>();

        // Find all buttons in the scene and store their original color
        Button[] allButtons = FindObjectsOfType<Button>();
        foreach (Button button in allButtons)
        {
            originalColors.Add(button, button.image.color);
        }
    }
}
