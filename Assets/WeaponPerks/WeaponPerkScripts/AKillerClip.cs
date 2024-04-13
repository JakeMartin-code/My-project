using System.Collections;
using UnityEngine;

public class AKillerClip : MonoBehaviour
{

    public float damageMultiplier = 1.25f;
    public float activationWindow = 3.6f; // seconds after a kill to reload and activate
    public float duration = 5.5f; // seconds

    private bool canActivate = false;
    private Coroutine damageBoostCoroutine;

    public  void ActivatePerk(WeaponBehavior weapon)
    {
        if (canActivate)
        {
            if (damageBoostCoroutine != null)
            {
                weapon.StopCoroutine(damageBoostCoroutine);
            }
            damageBoostCoroutine = weapon.StartCoroutine(DamageBoost(weapon));
        }
    }

    public void OnEnemyDefeated()
    {
        canActivate = true;
        // Start a coroutine that waits for activationWindow duration before setting canActivate to false
        // This coroutine should be stopped and started fresh with every enemy kill
    }

    private IEnumerator DamageBoost(WeaponBehavior weapon)
    {
        float originalDamage = weapon.weaponStats.baseDamage;
        weapon.weaponStats.baseDamage *= damageMultiplier;

        yield return new WaitForSeconds(duration);

        weapon.weaponStats.baseDamage = originalDamage;
        canActivate = false;
    }
}
