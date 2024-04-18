using System.Collections;
using UnityEngine;

public class TimeSurvivalMission : MissionStep
{
    public float timeToSurvive = 5;
    private float timeSurvived = 0;
    public bool isMissionActive = true;
    public EnemySpawnInfo[] enemyTypes;
    public Transform playerTransform;
    private PlayerStats playerStats;
    public float spawnInterval = 1f;  // Time between spawns
    private float nextSpawnTime = 0f;

    private void Start()
    {
        // Dynamically find the player GameObject and get the PlayerStats component
        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            playerTransform = player.transform;
            playerStats = player.GetComponent<PlayerStats>();
        }
        else
        {
            Debug.LogError("Player GameObject not found in the scene.");
        }
    }

    public override float ProgressPercentage
    {
        get { return timeSurvived / timeToSurvive; }
    }

    private void Update()
    {
        if (!isMissionActive) return;

        timeSurvived += Time.deltaTime;
        if (timeSurvived >= timeToSurvive)
        {
            isMissionActive = false;
            FinishMission(true);
        }
        else
        {
            HandleEnemySpawning();
        }
        CheckFailureCondition();
    }

    private void HandleEnemySpawning()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnEnemy();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    private void SpawnEnemy()
    {
        EnemySpawnInfo selectedEnemyInfo = ChooseEnemyPrefab();
        Vector3 spawnPosition = GetRandomSpawnPositionAroundPlayer();
        GameObject enemy = Instantiate(selectedEnemyInfo.prefab, spawnPosition, Quaternion.identity);
        // Optionally, set unique enemy behavior here
    }

    private Vector3 GetRandomSpawnPositionAroundPlayer()
    {
        float distance = 20f;  // Distance from the player to spawn enemies
        Vector3 randomDirection = Random.insideUnitSphere * distance;
        randomDirection += playerTransform.position;
        randomDirection.y = 0;  // Assuming you want to spawn on the ground level
        return randomDirection;
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
    private void FinishMission(bool success)
    {
        isMissionActive = false;
        if (success)
        {
            Debug.Log("Mission completed successfully.");
            FinishQuestStep();
        }
        else
        {
            Debug.Log("Mission failed.");
            EventsManager.instance.missionEvent.FailMission(missionID);
        }
    }

    public override void CheckFailureCondition()
    {
        PlayerStats playerStats = playerTransform.GetComponent<PlayerStats>();
        if (playerStats != null && playerStats.currentHealth <= 0)
        {
            FinishMission(false);
        }
    }
}
