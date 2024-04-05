using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 20f;
    public int damage = 10;
    public float maxLifetime = 5f; // Time before the projectile gets automatically recycled

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        // Reset the projectile's lifetime each time it's enabled
        Invoke(nameof(DestroyProjectile), maxLifetime);
    }

    public void Launch(Vector3 direction)
    {
        rb.velocity = direction.normalized * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Assuming the player has a script with a TakeDamage method
            other.GetComponent<PlayerStats>().TakeDamage(damage);
        }
        DestroyProjectile();
    }

    private void DestroyProjectile()
    {
        // Instead of destroying, deactivate it to return it to the pool
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }
}
