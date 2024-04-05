using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
    public static ProjectilePool Instance;

    [SerializeField] private GameObject projectilePrefab;
    private Queue<GameObject> projectiles = new Queue<GameObject>();

    private void Awake()
    {
        Instance = this;
    }

    public GameObject GetProjectile()
    {
        if (projectiles.Count == 0)
            AddProjectiles(1);

        return projectiles.Dequeue();
    }

    private void AddProjectiles(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var projectile = Instantiate(projectilePrefab, transform);
            projectile.SetActive(false);
            projectiles.Enqueue(projectile);
        }
    }

    public void ReturnProjectile(GameObject projectile)
    {
        projectile.SetActive(false);
        projectiles.Enqueue(projectile);
    }
}