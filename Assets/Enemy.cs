using UnityEngine;
using UnityEngine.AI;
using System;
using System.Collections.Generic;


public enum EnemyState
{
    Patrolling,
    Hostile,

}

public class Enemy : MonoBehaviour
{
    public static event Action<Enemy> EnemyKilled;

    public static int enemyCount = 0; 
    public int maxEnemies = 10; 
    public EnemyManager enemyManager;
    public WeaponManager weaponManager;

    public Transform[] patrolPoints;
    public float detectionRange = 5.0f;
    public float stoppingDistance = 1.5f;
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
    public PlayerMovement playerController;


    public GameObject[] potentialWeapons; // Array of potential weapon prefabs

    private bool hasDroppedItem = false;
    public float dropChance = 0.5f;

    private void Start()
    {
        currentHealth = startingHealth;
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform; 
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
               PlayerMovement player = col.GetComponent<PlayerMovement>();
                if (player != null)
                {
                    // Deal damage to the player
                    player.TakeDamage(damageAmount);
                   
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
                enemyManager.RespawnEnemy(); 
            }

            
            EnemyKilled?.Invoke(this);

            RewardXP();
            DropItem();
            Destroy(gameObject); 
        }
    }



    private void DropItem()
    {
        if (UnityEngine.Random.value < dropChance && !hasDroppedItem)
        {
          
            GameObject selectedWeaponPrefab = GetRandomWeaponPrefab();

            if (selectedWeaponPrefab != null)
            {
                
                Instantiate(selectedWeaponPrefab, transform.position, Quaternion.identity);
                hasDroppedItem = true;
            }
        }
    }

    private GameObject GetRandomWeaponPrefab()
    {

        if (potentialWeapons.Length == 0)
        {
          
            return null;
        }

        
        int randomIndex = UnityEngine.Random.Range(0, potentialWeapons.Length);
        return potentialWeapons[randomIndex];
    }


    private void RewardXP()
    {
        /*
        PlayerMovement player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        if (player != null)
        {
            player.GainXP(xpReward);
            Debug.Log("Player gained " + xpReward + " XP from defeating this enemy!");
        }
        */
        EventsManager.instance.ExperienceGained(xpReward);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }

}
