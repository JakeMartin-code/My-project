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

    [Header("Combat")]
    public float detectionRange = 10f;
    public float attackCooldown = 2f;
    protected float attackTimer;

    [Header("Health")]
    public int maxHealth = 100;
    protected int currentHealth;

    [Header("UI")]
    public GameObject healthBarUIPrefab;
    protected Slider healthBarSlider;
    public GameObject damageTextPrefab;
    public Canvas worldSpaceCanvas;


    protected State currentState;
    protected NavMeshAgent agent;
    protected Transform player;
    public int xpReward = 10;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        worldSpaceCanvas = GameObject.FindGameObjectWithTag("WorldSpaceCanvas").GetComponent<Canvas>();
        InitializeHealthBar();
        TransitionToState(State.Patrolling);
        StartCoroutine(DynamicPatrol());
    }

    protected virtual void Update()
    {
        switch (currentState)
        {
            case State.Patrolling:
                PatrolBehavior();
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

    protected void PatrolBehavior()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            StartCoroutine(DynamicPatrol());
        }

        if (Vector3.Distance(transform.position, player.position) <= detectionRange)
        {
            TransitionToState(State.Hostile);
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

    private IEnumerator DynamicPatrol()
    {
        yield return new WaitForSeconds(5f); // Wait for 5 seconds before picking a new patrol point
        if (currentState == State.Patrolling)
        {
            Vector3 patrolPoint = GetRandomPatrolPoint();
            agent.SetDestination(patrolPoint);
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

    public void TakeDamage(int damage, Vector3 spawnPosition)
    {
        Debug.Log("taking damage");
        currentHealth -= damage;
        UpdateHealthBar();
        if (currentHealth <= 0)
        {
            EnemyKilled?.Invoke(this);

            RewardXP();
            Destroy(gameObject);
        }
        
        ShowDamage(damage, spawnPosition);
        
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

    protected void ShowDamage(int damage, Vector3 spawnPosition)
    {
        GameObject dmgTextObj = Instantiate(damageTextPrefab, spawnPosition, Quaternion.identity, worldSpaceCanvas.transform);
        dmgTextObj.GetComponent<DamageText>().SpawnText(damage);

    }

    private void RewardXP()
    {

        EventsManager.instance.ExperienceGained(xpReward);
    }

}

