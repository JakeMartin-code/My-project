using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public float xpMultiplier = 1.2f;
    public int perkPoints = 0;
    public int xpToNextLevel;

    public int currentXP = 0;
    public int playerLevel = 1;

    public int maxHealth = 100;
    public int currentHealth;

    public Slider xpBarSlider;
    public Slider healthBar;

    void OnEnable()
    {
      
        EventsManager.instance.onXPGained += GainXP;
    }

    void OnDisable()
    {
     
        EventsManager.instance.onXPGained -= GainXP;
    }

    public void Start()
    {
        currentHealth = maxHealth;
        UpdateXPBar();
        UpdateHealthBar();
    }

    public void GainXP(int amount)
    {
        currentXP += amount;
        Debug.Log("Player gained " + amount + " XP!");
        Debug.Log("XP Required for Next Level: " + CalculateXPToNextLevel());
        UpdateXPBar();
        while (currentXP >= CalculateXPToNextLevel())
        {
            LevelUp();
        }
    }

    int CalculateXPToNextLevel()
    {

        return Mathf.FloorToInt(Mathf.Pow(playerLevel, xpMultiplier) * 100);
    }

    void LevelUp()
    {

        playerLevel++;
        currentXP -= xpToNextLevel;
        perkPoints++;
        UpdateXPBar();
        EventsManager.instance.LevelUp(playerLevel);

    }

    void UpdateXPBar()
    {

        xpToNextLevel = CalculateXPToNextLevel();
        xpBarSlider.maxValue = xpToNextLevel;
        xpBarSlider.value = Mathf.Max(currentXP, 0);

    }

    void UpdateHealthBar()
    {
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
    }

    public void TakeDamage(int damage)
    {
         currentHealth -= damage;
         UpdateHealthBar();
         if (currentHealth <= 0)
         {
            EventsManager.instance.PlayerDied();
            Debug.Log("Player died!");
         }
    }
}
