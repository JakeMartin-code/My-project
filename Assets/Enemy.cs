using UnityEngine;
using UnityEngine.AI;
using System;
using System.Collections.Generic;


public enum EnemyState
{
    Patrolling,
    Hostile,
    // Add more states as needed
}

public class Enemy : MonoBehaviour
{
    public static event Action<Enemy> EnemyKilled; // Event to notify when enemy is killed

    public static int enemyCount = 0; // Static variable to keep track of enemy count
    public int maxEnemies = 10; // Maximum number of enemies allowed
    public EnemyManager enemyManager;
    public WeaponManager weaponManager;

    public Transform[] patrolPoints;
    public float detectionRange = 5.0f;
    public float stoppingDistance = 1.5f; // Distance to stop from the player
    public int xpReward = 10;
    public int startingHealth = 100;
   

    public float currentHealth;

    private EnemyState currentState = EnemyState.Patrolling;
    private NavMeshAgent agent;
    private int currentPatrolIndex = 0;
    [SerializeField]
    private Transform player;

    public float meleeRange = 2.0f;
    public int damageAmount = 10;
    public PlayerController playerController;

    public GameObject[] potentialWeapons; // Array of potential weapon prefabs

    private bool hasDroppedItem = false;
    public float dropChance = 0.5f;

    private void Start()
    {
        currentHealth = startingHealth;
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform; // Assuming the player has a "Player" tag
        //playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        SetDestination();
        enemyManager = GameObject.FindGameObjectWithTag("EnemyManager").GetComponent<EnemyManager>();
        enemyCount++;
    }

    private void Update()
    {
        switch (currentState)
        {
            case EnemyState.Patrolling:
                if (!agent.pathPending && agent.remainingDistance < 0.1f)
                {
                    SetNextPatrolPoint();
                }
                CheckForPlayer();
                break;
            case EnemyState.Hostile:
                if (player != null)
                {
                    ChasePlayer();
                    AttackPlayer();
                }
                break;
            // Handle other states here
            default:
                break;
        }
    }

    private void SetDestination()
    {
        if (patrolPoints.Length > 0)
        {
            agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        }
    }

    private void SetNextPatrolPoint()
    {
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        SetDestination();
    }

    private void CheckForPlayer()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRange);
        foreach (Collider col in hitColliders)
        {
            if (col.CompareTag("Player"))
            {
                if(playerController.isInvisible == false)
                {
                    currentState = EnemyState.Hostile;
                    agent.isStopped = true; // Stop patrolling
                    break;
                }
            }
        }
    }

    private void ChasePlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer > stoppingDistance)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
        }
        else
        {
            agent.isStopped = true;
        }
    }

    private void AttackPlayer()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, meleeRange);
        foreach (Collider col in hitColliders)
        {
            if (col.CompareTag("Player"))
            {
                PlayerController player = col.GetComponent<PlayerController>();
                if (player != null)
                {
                    // Deal damage to the player
                    player.TakeDamage(damageAmount);
                    Debug.Log("Player took damage from enemy: " + damageAmount);
                }
            }
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            enemyCount--;
            if (enemyManager != null)
            {
                enemyManager.RespawnEnemy(); // Respawn a new enemy
            }

            // Notify subscribers that the enemy is killed, passing the enemy itself
            EnemyKilled?.Invoke(this);

            RewardXP();
            DropItem();
            Destroy(gameObject); // Destroy the enemy when health reaches 0
        }
    }



    private void DropItem()
    {
        if (UnityEngine.Random.value < dropChance && !hasDroppedItem)
        {
            // Randomly select a weapon prefab from the array
            GameObject selectedWeaponPrefab = GetRandomWeaponPrefab();

            if (selectedWeaponPrefab != null)
            {
                // Instantiate the selected weapon prefab at the enemy's position
                Instantiate(selectedWeaponPrefab, transform.position, Quaternion.identity);
                hasDroppedItem = true;
            }
        }
    }

    private GameObject GetRandomWeaponPrefab()
    {
        // Check if there are potential weapon prefabs in the array
        if (potentialWeapons.Length == 0)
        {
            Debug.LogWarning("No potential weapon prefabs assigned.");
            return null;
        }

        // Randomly select a weapon prefab from the array
        int randomIndex = UnityEngine.Random.Range(0, potentialWeapons.Length);
        return potentialWeapons[randomIndex];
    }


    private void RewardXP()
    {
        PlayerController player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        if (player != null)
        {
            player.GainXP(xpReward);
            Debug.Log("Player gained " + xpReward + " XP from defeating this enemy!");
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }

}
