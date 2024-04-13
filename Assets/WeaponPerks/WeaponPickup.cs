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
            Camera mainCamera = Camera.main; // Find the main camera

            if (weaponManager != null && mainCamera != null)
            {
                // Add the weapon to the player's inventory
                weaponManager.AddWeaponToInventory(weaponData);

                // Set the weapon's parent to be the camera to simulate a first-person view
                transform.SetParent(mainCamera.transform);
                // Adjust the weapon's position and rotation to your liking here
                transform.localPosition = new Vector3(0.325f, -0.372f, 0.4530005f);
                transform.localRotation = Quaternion.identity;

                // Update the UI dropdowns
                if (uIManager != null)
                {
                    uIManager.UpdateWeaponDropdowns();
                }

                // Set the weapon GameObject as inactive in the scene, so it's not visible or interactable
                gameObject.SetActive(false);

                weaponData.SetWeaponStats(weaponData.weaponStats);

                // Equip the weapon if it should be immediately active

            }
        }
    }

}
