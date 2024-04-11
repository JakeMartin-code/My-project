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

    public EnemySpawnInfo[] enemyTypes; 
    public Transform playerTransform;
    public float spawnRate = 5f;
    public int maxEnemies = 10;
    public int squadSize = 4;
    private int lastPatrolAreaIndex = 0;

    private float nextSpawnTime = 0f;
    private List<GameObject> spawnedEnemies = new List<GameObject>();

    private PatrolArea[] patrolAreas;

    private void Start()
    {
        patrolAreas = FindObjectsOfType<PatrolArea>(); // Find all patrol areas in the scene
    }

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
        if (patrolAreas.Length > 0)
        {
            // Cycle through patrol areas sequentially
            PatrolArea selectedArea = patrolAreas[lastPatrolAreaIndex];
            Vector3 squadSpawnPosition = selectedArea.GetRandomPoint();

            for (int i = 0; i < squadSize; i++)
            {
                EnemySpawnInfo selectedEnemy = ChooseEnemyPrefab();
                Vector3 individualSpawnPosition = squadSpawnPosition + Random.insideUnitSphere * 2;
                NavMeshHit hit;
                if (NavMesh.SamplePosition(individualSpawnPosition, out hit, 2f, NavMesh.AllAreas))
                {
                    individualSpawnPosition = hit.position;
                }
                GameObject enemy = Instantiate(selectedEnemy.prefab, individualSpawnPosition, Quaternion.identity);
                enemy.GetComponent<EnemyManager>().SetPatrolArea(selectedArea); // Assign the PatrolArea to the enemy
                spawnedEnemies.Add(enemy);
            }

            // Move to the next patrol area index for the next spawn cycle
            lastPatrolAreaIndex = (lastPatrolAreaIndex + 1) % patrolAreas.Length;
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        int randomIndex = Random.Range(0, patrolAreas.Length);
        PatrolArea selectedArea = patrolAreas[randomIndex];
        return selectedArea.GetRandomPoint(); // Assumes PatrolArea has a GetRandomPoint method
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


}

