using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public WeaponBehavior weaponData;
    private UIManager uIManager;

    private void Start()
    {
        uIManager = GameObject.Find("UImanager").GetComponent<UIManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            WeaponManager weaponManager = other.GetComponent<WeaponManager>();
            if (weaponManager != null)
            {
                // Add weaponData to the player's inventory
                weaponManager.AddWeaponToInventory(weaponData);

                // Update the UI dropdowns
                UIManager uIManager = FindObjectOfType<UIManager>();
                if (uIManager != null)
                {
                    uIManager.UpdateWeaponDropdowns();
                }

                // Set the weapon as inactive in the scene, NOT the prefab
                this.gameObject.SetActive(false);
            }
        }
    }
}
