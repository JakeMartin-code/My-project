using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class WeaponBehavior : MonoBehaviour
{
    public WeaponData weaponStats;

    public int localreserveAmmo;
    public int localcurrentAmmoInMag;
    public int localmaxAmmoInMag;
    public bool isReloading = false;
    public TextMeshProUGUI localAmmoInMagUI;
    public TextMeshProUGUI localReserveUI;
    private float nextFireTime = 0f;
    public bool doubleDamageWhileSliding = false;

    // public List<WeaponPerk> activePerks = new List<WeaponPerk>();


    private void Start()
    {

        localAmmoInMagUI = GameObject.Find("mag").GetComponent<TextMeshProUGUI>();
        localReserveUI = GameObject.Find("Reserves").GetComponent<TextMeshProUGUI>();
        localAmmoInMagUI.SetText("" + localcurrentAmmoInMag.ToString());
        localReserveUI.SetText("" + localreserveAmmo.ToString());
        //EnemyManager.EnemyKilled += OnEnemyDefeated;
    }


    public void SetWeaponStats(WeaponData newStats)
    {

        localreserveAmmo = newStats.basereserveAmmo;
        localmaxAmmoInMag = newStats.basemaxAmmoInMag;
        localcurrentAmmoInMag = localmaxAmmoInMag;
    }

    /*
    public void ActivatePerk()
    {
        foreach (var perk in activePerks)
        {
            perk.ActivatePerk(this);
        }
    }
    */
    private void Update()
    {

        localAmmoInMagUI.SetText("" + localcurrentAmmoInMag.ToString());
        localReserveUI.SetText("" + localreserveAmmo.ToString());

        if (!isReloading && localcurrentAmmoInMag > 0)
        {
            if (weaponStats.isFullAuto && Input.GetButton("Fire1") && Time.time >= nextFireTime)
            {
                Fire();
            }
            else if (!weaponStats.isFullAuto && Input.GetButtonDown("Fire1") && Time.time >= nextFireTime)
            {
                Fire();
            }
        }
    }


    public void Fire()
    {

        if (CanFire() && localcurrentAmmoInMag > 0)
        {
            nextFireTime = Time.time + 1f / weaponStats.fireRate;


            localcurrentAmmoInMag--;

            RaycastHit hit;


            if (Physics.Raycast(transform.position, transform.forward, out hit, weaponStats.range))
            {


                if (hit.collider.CompareTag("Enemy"))
                {

                    EnemyManager enemy = hit.collider.GetComponent<EnemyManager>();
                    if (enemy != null)
                    {
                        float damage = weaponStats.baseDamage;
                        if (doubleDamageWhileSliding && GetComponentInParent<PlayerMovement>().isSliding) // Check if double damage should be applied
                        {
                            damage *= 2; // Double the damage
                        }
                        enemy.TakeDamage(damage, hit.point, weaponStats.weaponPlaystyle);

                    }
                }
            }
            else
            {

                Debug.DrawRay(transform.position, transform.forward * weaponStats.range, Color.green);
            }


            if (localcurrentAmmoInMag == 0 && localreserveAmmo > 0)
            {
                isReloading = true;
                Reload();
            }
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

        // ActivatePerk();

    }

    private bool CanFire()
    {
        return Time.time >= nextFireTime;
    }


    private IEnumerator ReloadCoroutine(int ammoToReload)
    {

        yield return new WaitForSeconds(weaponStats.baseReloadTime);


        localreserveAmmo -= ammoToReload;
        localcurrentAmmoInMag += ammoToReload;
        if (localcurrentAmmoInMag > localmaxAmmoInMag)
        {
            localcurrentAmmoInMag = localmaxAmmoInMag;
        }

        isReloading = false;
    }
    /*

    public void OnEnemyDefeated(EnemyManager enemy)
    {
        foreach (var perk in activePerks.OfType<AKillerClip>())
        {
            perk.OnEnemyDefeated();
        }
    }
    */
}




    /*

    private IEnumerator DamageBoostCoroutine(float boostDuration, int boostAmount)
    {
        int originalDamage = weaponStats.baseDamage;

    
        weaponStats.baseDamage *= (1 + boostAmount); 

        yield return new WaitForSeconds(boostDuration);

        
        weaponStats.baseDamage = originalDamage;

      
    }

*/
    



