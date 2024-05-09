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
    public WeaponBehavior equippedWeapon; 

    private PlayerInputs controls;

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


        controls = new PlayerInputs();
        SetupControls();

    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void SetupControls()
    {
        controls.WeaponControlls.Fire.performed += _ => equippedWeapon.Fire();
        controls.WeaponControlls.Reload.performed += _ => equippedWeapon.Reload();
        controls.WeaponControlls.SwitchWeapon.performed += _ => SwitchWeapon(true);
    }


    public WeaponBehavior GetEquippedWeapon()
    {
        return equippedWeapon;
    }


    public void SwitchWeapon(bool next)
    {
       
        if (next)
        {
            currentSlotIndex = (currentSlotIndex + 1) % 3;
        }
        else
        {
            currentSlotIndex = (currentSlotIndex - 1 + 3) % 3;
        }

  
        if (equippedWeapon != null)
        {
            equippedWeapon.gameObject.SetActive(false);
        }


        switch (currentSlotIndex)
        {
            case 0:
                equippedWeapon = equippedPrimary;
                break;
            case 1:
                equippedWeapon = equippedSecondary;
                break;
            case 2:
                equippedWeapon = equippedHeavy;
                break;
        }


        if (equippedWeapon != null)
        {
            equippedWeapon.gameObject.SetActive(true);
        }
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

        if ((int)slot == currentSlotIndex)
        {
       
            if (equippedWeapon != null && equippedWeapon != weapon)
            {
                equippedWeapon.gameObject.SetActive(false);
            }

            equippedWeapon = weapon;
       
            equippedWeapon.gameObject.SetActive(true);
        }
    }


    public void AddWeaponToInventory(WeaponBehavior weapon)
    {
        weapon.gameObject.SetActive(false); 
        weapon.transform.SetParent(transform); 

        switch (weapon.weaponStats.weaponType) 
        {
            case WeaponType.Primary:
                primaryInventory.Add(weapon);
                break;
            case WeaponType.Secondary:
                secondaryInventory.Add(weapon);
                break;
            case WeaponType.Heavy:
                heavyInventory.Add(weapon);
                break;
            default:
                Debug.LogWarning("Unrecognized weapon type!");
                break;
        }
    }

}

public enum WeaponSlot
{
    Primary,
    Secondary,
    Heavy
}
