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
    public float healthRegenDelay = 5f; // Delay in seconds before regeneration starts
    public float healthRegenRate = 1f;  // Rate of health regeneration per second
    private float lastDamageTime;
    private Coroutine regenCoroutine;

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
            
         }
    }

    void Update()
    {
        // Check if enough time has passed since the last damage was taken to start regeneration
        if (currentHealth < maxHealth && Time.time - lastDamageTime >= healthRegenDelay)
        {
            if (regenCoroutine == null) // Ensure that the coroutine is not already running
            {
                StartRegeneration();
            }
        }
    }

    public void StopRegeneration()
    {
        if (regenCoroutine != null)
        {
            StopCoroutine(regenCoroutine);
            regenCoroutine = null;
        }
    }

    public void StartRegeneration()
    {
        regenCoroutine = StartCoroutine(RegenerateHealth());
    }

    IEnumerator RegenerateHealth()
    {
        // Wait for the specified delay before starting regeneration
        yield return new WaitForSeconds(healthRegenDelay);

        // Continue regenerating health if not interrupted
        while (currentHealth < maxHealth)
        {
            if (Time.time - lastDamageTime >= healthRegenDelay)
            {
                currentHealth += (int)(healthRegenRate * Time.deltaTime * maxHealth);
                currentHealth = Mathf.Min(currentHealth, maxHealth);
                UpdateHealthBar();
                yield return null; // Wait until the next frame
            }
            else
            {
                break; // Stop regeneration if damaged recently
            }
        }
    }

    public void ResetPlayerStats()
    {
        currentXP = 0;
        playerLevel = 1;
        maxHealth = 100;
        currentHealth = maxHealth;
        perkPoints = 0;
        xpMultiplier = 1.2f; 
        StopRegeneration();
        UpdateHealthBar();
        UpdateXPBar();
        Debug.Log("Player stats have been reset.");
    }

}
