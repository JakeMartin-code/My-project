using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
        Debug.Log("ON START Current Ammo In Mag : " + localcurrentAmmoInMag);
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
        Debug.Log("ON UPDATE Current Ammo In Mag : " + localcurrentAmmoInMag);
        localAmmoInMagUI.SetText("" + localcurrentAmmoInMag.ToString());
        localReserveUI.SetText("" + localreserveAmmo.ToString());
    }


    public void Fire()
    {
        // Check if there's ammo in the magazine to fire
        Debug.Log("Current Ammo In Mag: " + localcurrentAmmoInMag);

        if (localcurrentAmmoInMag > 0)
        {
            // Decrease ammo in the magazine
            localcurrentAmmoInMag--;
            Debug.Log(localcurrentAmmoInMag);
            Debug.Log("firing");

            // Cast a ray from the weapon's position along the aim direction
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, weaponStats.range))
            {
                // Check if the ray hits a target
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




    // Function to apply perk effects
    public void ApplyPerkEffect()
    {
        switch (weaponStats.perkType)
        {
            case WeaponPerk.DamageBoostAfterKill:
                // Example: Increase damage temporarily after a kill
                StartCoroutine(DamageBoostCoroutine());
                break;
                // Add more perk cases as needed
        }
    }

    private IEnumerator DamageBoostCoroutine()
    {
        // Apply damage boost logic using weaponStats.perkValue
        yield return null;
    }
}
