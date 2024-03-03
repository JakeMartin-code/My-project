using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public Transform[] spawnPoints; // Array of spawn points for enemies
    public GameObject enemyPrefab; // Prefab of the enemy to spawn

    public int currentEnemyCount = 0; // Current enemy count

    public int maxEnemies = 10; // Maximum number of enemies allowed

    private void Update()
    {
        if (currentEnemyCount < maxEnemies)
        {
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        // Randomly select a spawn point
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // Instantiate the enemy at the spawn point
        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

        currentEnemyCount++;
    }

    public void RespawnEnemy()
    {
        if (currentEnemyCount < maxEnemies)
        {
            SpawnEnemy();
        }
    }
}
