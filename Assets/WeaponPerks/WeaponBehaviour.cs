using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class WeaponBehavior : MonoBehaviour
{
    public WeaponData weaponStats;

    public int localreserveAmmo;
    public int localcurrentAmmoInMag;
    public int localmaxAmmoInMag;
    public bool isReloading = false;
    public TextMeshProUGUI localAmmoInMagUI;
    public TextMeshProUGUI localReserveUI;

    private void Start()
    {
       
        localAmmoInMagUI.SetText("" + localcurrentAmmoInMag.ToString());
        localReserveUI.SetText("" + localreserveAmmo.ToString());

    }

    public void SetWeaponStats(WeaponData newStats)
    {
       // weaponStats = newStats;
        localreserveAmmo = newStats.basereserveAmmo;
        localmaxAmmoInMag = newStats.basemaxAmmoInMag;
        localcurrentAmmoInMag = localmaxAmmoInMag;
    }

    private void Update()
    {
        
        localAmmoInMagUI.SetText("" + localcurrentAmmoInMag.ToString());
        localReserveUI.SetText("" + localreserveAmmo.ToString());
    }


    public void Fire()
    {
        if (localcurrentAmmoInMag > 0)
        {
            // Decrease ammo in the magazine
            localcurrentAmmoInMag--;
            // Cast a ray from the weapon's position along the aim direction
            RaycastHit hit;
            Debug.DrawLine(transform.position, transform.position + transform.forward * weaponStats.range, Color.green, 10f);

            if (Physics.Raycast(transform.position, transform.forward, out hit, weaponStats.range))
            {
                // Check if the ray hits a targets
                Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.red);

                // Perform actions on the hit object, such as dealing damage or triggering effects

                if (hit.collider.CompareTag("Enemy")) // Example: Assuming the target is tagged as "Enemy"
                {
                    // Check if the hit object has an Enemy component
                    Enemy enemy = hit.collider.GetComponent<Enemy>();
                    if (enemy != null)
                    {
                        // Deal damage to the enemy
                        enemy.TakeDamage(weaponStats.baseDamage);
                        Debug.Log("Hit enemy! Dealt damage: " + weaponStats.baseDamage);
                    }
                }
                else
                {
                    // If the hit object is not tagged as an enemy, log what it is
                    Debug.Log("Hit non-enemy object: " + hit.collider.name);
                    // You can perform other actions here, such as shooting the ground or triggering effects
                }

            }
            else
            {
                // If the ray doesn't hit anything, visualize the ray's path
                Debug.DrawRay(transform.position, transform.forward * weaponStats.range, Color.green);    
            }

            // If the magazine becomes empty after firing, and there's reserve ammo, reload
            if (localcurrentAmmoInMag == 0 && localreserveAmmo > 0)
            {
                isReloading = true;
                Reload();
            }
        }
        else
        {
            // Perform actions for out of ammo, e.g., play empty click sound
            Debug.Log("Out of ammo");
        }
    }

    public void Reload()
    {
        Debug.Log("Reloading");

        int ammoNeeded = localmaxAmmoInMag - localcurrentAmmoInMag;
        if (localreserveAmmo >= ammoNeeded)
        {
            StartCoroutine(ReloadCoroutine(ammoNeeded));
        }
        else if (localreserveAmmo > 0)
        {
            StartCoroutine(ReloadCoroutine(localreserveAmmo));
        }
        else
        {
            Debug.Log("Out of ammo!");
        }
    }


    private IEnumerator ReloadCoroutine(int ammoToReload)
    {
        // Add delay before reloading
        yield return new WaitForSeconds(weaponStats.baseReloadTime);

        // Refill the magazine from reserve ammo
        localreserveAmmo -= ammoToReload;
        localcurrentAmmoInMag += ammoToReload;
        if (localcurrentAmmoInMag > localmaxAmmoInMag)
        {
            localcurrentAmmoInMag = localmaxAmmoInMag;
        }

        isReloading = false;
    }


    public void ApplyPerkEffects()
    {
        Debug.Log("apply perk effects called");

        // Ensure weaponData is assigned
        if (weaponStats == null)
        {
            Debug.LogError("WeaponData is not assigned in WeaponBehavior.");
            return;
        }

        // Check for active perks in weaponData
        for (int i = 0; i < weaponStats.possiblePerks.Count; i++)
        {
            switch (weaponStats.possiblePerks[i])
            {
                case WeaponPerk.DamageBoostAfterKill:
                    StartCoroutine(DamageBoostCoroutine(weaponStats.perkDurations[i], weaponStats.perkValues[i])); // Use perk duration and boost amount from weaponData
                    break;
                    // Handle other perks similarly
            }
        }
    }

    

    private IEnumerator DamageBoostCoroutine(float boostDuration, float boostAmount)
    {
        float originalDamage = weaponStats.baseDamage; // Store the original base damage

        // Log the base damage before applying the boost
        Debug.Log("Base Damage before boost: " + originalDamage);

        // Apply damage boost for boostDuration seconds
        weaponStats.baseDamage *= (1 + boostAmount); // Increase damage by boostAmount (e.g., 10%)

        // Log the base damage after applying the boost
        Debug.Log("Base Damage after boost: " + weaponStats.baseDamage);

        yield return new WaitForSeconds(boostDuration);

        // Restore damage to its original value after the duration
        weaponStats.baseDamage = originalDamage;

        // Log the base damage after restoring to original value
        Debug.Log("Base Damage after restoration: " + weaponStats.baseDamage); 
    }
}
