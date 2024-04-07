using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public struct EnemySpawnInfo
    {
        public GameObject prefab;
        public float spawnWeight;
    }

    public EnemySpawnInfo[] enemyTypes; // Array to hold different enemy types and weights
    public Transform playerTransform;
    public float spawnRate = 5f;
    public float spawnDistance = 20f;
    public int maxEnemies = 10; // Adjust based on gameplay needs
    public int squadSize = 4; // Target size of each spawned squad

    private float nextSpawnTime = 0f;
    private List<GameObject> spawnedEnemies = new List<GameObject>();

    private void Update()
    {
        spawnedEnemies.RemoveAll(item => item == null);

        if (Time.time >= nextSpawnTime && spawnedEnemies.Count + squadSize <= maxEnemies)
        {
            SpawnSquad();
            nextSpawnTime = Time.time + 1f / spawnRate;
        }
    }

    private void SpawnSquad()
    {
        Vector3 spawnPosition = GetSpawnPosition(playerTransform.position, spawnDistance, 30);

        for (int i = 0; i < squadSize; i++)
        {
            EnemySpawnInfo selectedEnemy = ChooseEnemyPrefab();
            GameObject enemy = Instantiate(selectedEnemy.prefab, spawnPosition + Random.insideUnitSphere * 2, Quaternion.identity);
            spawnedEnemies.Add(enemy);
        }
    }

    private EnemySpawnInfo ChooseEnemyPrefab()
    {
        float totalWeight = 0f;
        foreach (var enemy in enemyTypes)
        {
            totalWeight += enemy.spawnWeight;
        }

        float choice = Random.Range(0, totalWeight);
        float cumulativeWeight = 0f;

        foreach (var enemy in enemyTypes)
        {
            cumulativeWeight += enemy.spawnWeight;
            if (choice <= cumulativeWeight)
            {
                return enemy;
            }
        }

        return enemyTypes[0]; // Fallback, though choice should always fall within totalWeight range
    }

    private Vector3 GetSpawnPosition(Vector3 centralPoint, float distance, int attempts)
    {
        for (int i = 0; i < attempts; i++)
        {
            Vector3 randomDirection = Random.insideUnitSphere * distance + centralPoint;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDirection, out hit, distance, NavMesh.AllAreas))
            {
                return hit.position;
            }
        }
        return centralPoint; // Fallback to central point if no valid position found
    }
}

