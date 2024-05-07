using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveProjectile : MonoBehaviour
{


    public float explosionRadius = 5f;
    public float explosionDamage = 100f;
    public LayerMask damageableLayers;

 

    void OnCollisionEnter(Collision collision)
    {
        Explode(collision.GetContact(0).point); // Get impact point
        Destroy(gameObject);
    }

    void Explode(Vector3 explosionPoint)
    {
        // Find objects within the explosion radius
        Collider[] affectedColliders = Physics.OverlapSphere(explosionPoint, explosionRadius);

        // Apply damage in a similar way to the raycast
        foreach (var hitCollider in affectedColliders)
        {
            if (hitCollider.TryGetComponent(out EnemyManager enemy))
            {
                // Calculate damage based on distance from the explosion point (optional)
                float distance = Vector3.Distance(explosionPoint, hitCollider.transform.position);
                float damage = CalculateDamageBasedOnDistance(distance, explosionDamage);

                enemy.TakeDamage(damage, explosionPoint, WeaponPlaystyle.areaOfEffect);
            }
        }

        // Add visual or audio effects here
    }

    // Helper function to calculate damage falloff (optional)
    private float CalculateDamageBasedOnDistance(float distance, float maxDamage)
    {
        if (distance > explosionRadius) return 0f;  // Outside the radius

        float damageFalloff = 1.0f - (distance / explosionRadius); // Linear falloff for simplicity
        return maxDamage * damageFalloff;
    }
}
