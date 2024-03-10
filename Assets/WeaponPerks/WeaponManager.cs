using UnityEngine;
using System.Collections.Generic;

public class WeaponManager : MonoBehaviour
{
    public List<WeaponBehavior> primaryInventory = new List<WeaponBehavior>();
    public List<WeaponBehavior> secondaryInventory = new List<WeaponBehavior>();
    public List<WeaponBehavior> heavyInventory = new List<WeaponBehavior>();

    public WeaponBehavior equippedPrimary;
    public WeaponBehavior equippedSecondary;
    public WeaponBehavior equippedHeavy;
    public WeaponBehavior equippedWeapon; // Currently held weapon

    public int currentSlotIndex = 0;

    private void Awake()
    {
        EquipWeaponToSlot(primaryInventory[0], WeaponSlot.Primary);
        EquipWeaponToSlot(secondaryInventory[0], WeaponSlot.Secondary);
        EquipWeaponToSlot(heavyInventory[0], WeaponSlot.Heavy);

   
        equippedWeapon = primaryInventory[0];
        equippedWeapon.SetWeaponStats(primaryInventory[0].weaponStats);
        equippedSecondary.SetWeaponStats(secondaryInventory[0].weaponStats);
        equippedHeavy.SetWeaponStats(heavyInventory[0].weaponStats);

        Enemy.EnemyKilled += OnEnemyKilled;

    }

    public WeaponBehavior GetEquippedWeapon()
    {
        return equippedWeapon;
    }


    public void SwitchWeapon(bool next)
    {
        Debug.Log("Entered weapon switch function in manager");

        // Deactivate the current equipped weapon
        equippedWeapon.gameObject.SetActive(false);

        // Determine the index of the next weapon slot
        if (next)
        {
            currentSlotIndex = (currentSlotIndex + 1) % 3;
        }
        else
        {
            currentSlotIndex = (currentSlotIndex - 1 + 3) % 3;
        }

        // Update the equipped weapon based on the currentSlotIndex
        switch (currentSlotIndex)
        {
            case 0:
                equippedWeapon = primaryInventory[0];
                break;
            case 1:
                equippedWeapon = secondaryInventory[0];
                break;
            case 2:
                equippedWeapon = heavyInventory[0];
                break;
        }

        // Activate the newly equipped weapon
        equippedWeapon.gameObject.SetActive(true);
    }


    public void EquipWeaponToSlot(WeaponBehavior weapon, WeaponSlot slot)
    {
        switch (slot)
        {
            case WeaponSlot.Primary:
                equippedPrimary = weapon;
                break;
            case WeaponSlot.Secondary:
                equippedSecondary = weapon;
                break;
            case WeaponSlot.Heavy:
                equippedHeavy = weapon;
                break;
        }
    }

    private void OnEnemyKilled(Enemy enemy)
    {

        Debug.Log("Enemy killed: " + enemy.name);

        if (equippedWeapon != null)
        {
            // Access the equipped weapon responsible for the kill and apply perks/effects
            equippedWeapon.ApplyPerkEffects();
        }
    }
}

public enum WeaponSlot
{
    Primary,
    Secondary,
    Heavy
}
