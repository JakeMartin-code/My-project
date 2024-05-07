using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System.Collections;
using Unity.VisualScripting;


public abstract class EnemyManager : MonoBehaviour
{
    public static event Action<EnemyManager> EnemyKilled;


    public enum State
    {
        Patrolling,
        Hostile,
        Attacking
    }

    [Header("Patrol")]
    // public Transform[] patrolPoints;
    // protected int currentPatrolIndex = 0;

    private PatrolArea patrolArea;

    [Header("Combat")]
    public float detectionRange = 10f;
    public float attackCooldown = 2f;
    protected float attackTimer;

    [Header("Health")]
    public float maxHealth = 100;
    protected float currentHealth;

    [Header("UI")]
    public GameObject healthBarUIPrefab;
    protected Slider healthBarSlider;
    public GameObject damageTextPrefab;
    public Canvas worldSpaceCanvas;


    protected State currentState;
    protected NavMeshAgent agent;
    protected Transform player;
    public int xpReward = 10;

    public EnemySpawnInfo spawnInfo;
    private bool hasDroppedItem = false;

    private Material originalMaterial;
    private Renderer enemyRenderer;


    protected virtual void Start()
    {
        currentHealth = maxHealth;
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        worldSpaceCanvas = GameObject.FindGameObjectWithTag("WorldSpaceCanvas").GetComponent<Canvas>();
        InitializeHealthBar();
        TransitionToState(State.Patrolling);

        enemyRenderer = GetComponent<Renderer>();
        originalMaterial = enemyRenderer.material;

    }

    protected virtual void Update()
    {
        switch (currentState)
        {
            case State.Patrolling:
                PatrolBehavior();
                if (IsPlayerDetected())
                {
                    TransitionToState(State.Hostile);
                }
                break;
            case State.Hostile:
                HostileBehavior();
                break;
            case State.Attacking:
                AttackBehavior();
                break;
        }
        if (currentState != State.Patrolling)
        {
            CheckAttackTimer();
        }

        if (healthBarSlider != null)
        {
            healthBarSlider.transform.LookAt(Camera.main.transform);
            healthBarSlider.transform.Rotate(0, 180, 0); 
        }
    }

    protected bool IsPlayerDetected()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;
        if (distanceToPlayer <= detectionRange)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, directionToPlayer.normalized, out hit, detectionRange))
            {
                if (hit.collider.gameObject.CompareTag("Player"))
                {
                    // Player is within range and in line of sight
                    return true;
                }
            }
        }
        return false;
    }

    public void SetPatrolArea(PatrolArea area)
    {
        patrolArea = area;
    }


    protected void PatrolBehavior()
    {
        if (!agent.pathPending && agent.remainingDistance < 1f)
        {
            if (patrolArea != null)
            {
                Vector3 patrolPoint = patrolArea.GetRandomPoint();
                agent.SetDestination(patrolPoint);
            }
        }
    }

    protected abstract void HostileBehavior();

    protected abstract void AttackBehavior();

    protected void CheckAttackTimer()
    {
        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }
    }


  
    
    private Vector3 GetRandomPatrolPoint()
    {
        Vector3 centralPoint = player.position; // Using player's position as the central point
        float maxDistance = 30f; // Maximum distance from the player for patrol points
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * maxDistance;
        randomDirection += centralPoint;

        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, maxDistance, NavMesh.AllAreas);
        return hit.position;
    }
    

    protected void SetNextPatrolPoint()
    {
       // currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
       // agent.destination = patrolPoints[currentPatrolIndex].position;
    }

    protected void TransitionToState(State newState)
    {
        currentState = newState;
        OnStateChange(newState);
    }

    protected virtual void OnStateChange(State newState)
    {
        
    }

    public void TakeDamage(float damage, Vector3 spawnPosition, WeaponPlaystyle weaponPlaystyle)
    {
        Debug.Log("taking damage");
        currentHealth -= damage;
        UpdateHealthBar();
        if (currentHealth <= 0)
        {
            float distance = Vector3.Distance(player.position, transform.position);
            EnemyKilled?.Invoke(this);
            RewardXP();
            DropItem();
            Destroy(gameObject);
            MissionTracker.Instance.RecordKill(weaponPlaystyle, distance);
           
        }
        
        ShowDamage(damage, spawnPosition);
        
    }

    public void SetSpawnInfo(EnemySpawnInfo info)
    {
        spawnInfo = info;
    }

    private void DropItem()
    {
        if (UnityEngine.Random.value < spawnInfo.dropChance && !hasDroppedItem)
        {
            GameObject selectedWeaponPrefab = GetRandomWeaponPrefab();
            if (selectedWeaponPrefab != null)
            {
                // Instantiate the weapon prefab at the enemy's position
                GameObject weaponToDrop = Instantiate(selectedWeaponPrefab, transform.position, Quaternion.identity);
                // Attempt to get the WeaponPickup script attached to the weapon
                WeaponPickup weaponPickup = weaponToDrop.GetComponent<WeaponPickup>();

                // If the WeaponPickup script is found, assign the weapon's data to it
                if (weaponPickup != null)
                {
                    weaponPickup.weaponData = weaponToDrop.GetComponent<WeaponBehavior>();

                }
                else
                {
                    // If no WeaponPickup script is found, log an error or take corrective action
                    Debug.LogError("WeaponPickup script not found on the weapon prefab.");
                }

                hasDroppedItem = true;
            }
        }
    }

    private GameObject GetRandomWeaponPrefab()
    {
        if (spawnInfo.potentialWeapons.Length == 0)
        {
            return null;
        }

        int randomIndex = UnityEngine.Random.Range(0, spawnInfo.potentialWeapons.Length);
        return spawnInfo.potentialWeapons[randomIndex];
    }

    private void InitializeHealthBar()
    {
        GameObject healthBarUI = Instantiate(healthBarUIPrefab, transform.position + Vector3.up * 2, Quaternion.identity, transform); 
        healthBarSlider = healthBarUI.GetComponentInChildren<Slider>();
        healthBarSlider.maxValue = maxHealth;
        healthBarSlider.value = currentHealth;
    }

    private void UpdateHealthBar()
    {
        healthBarSlider.value = currentHealth;
    }

    protected void ShowDamage(float damage, Vector3 spawnPosition)
    {
        GameObject dmgTextObj = Instantiate(damageTextPrefab, spawnPosition, Quaternion.identity, worldSpaceCanvas.transform);
        dmgTextObj.GetComponent<DamageText>().SpawnText(damage);

    }

    private void RewardXP()
    {

        EventsManager.instance.ExperienceGained(xpReward);
    }

    public bool IsOccluded(Transform playerTransform)
    {
        Vector3 directionToPlayer = playerTransform.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;
        RaycastHit hit;

        if (Physics.Raycast(transform.position + Vector3.up * 0.5f, directionToPlayer.normalized, out hit, distanceToPlayer))
        {
            // Return true if the raycast hits something other than the player
            return !hit.collider.gameObject.CompareTag("Player");
        }
        return false;
    }

    public void EnableWallhack(Material wallhackMaterial)
    {
        enemyRenderer.material = wallhackMaterial;
    }

    public void DisableWallhack()
    {
        enemyRenderer.material = originalMaterial;
    }

}

