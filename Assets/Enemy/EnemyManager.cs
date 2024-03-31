using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

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
    public Transform[] patrolPoints;
    protected int currentPatrolIndex = 0;

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
        InitializeHealthBar();
        TransitionToState(State.Patrolling);
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
            SetNextPatrolPoint();
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

    protected void SetNextPatrolPoint()
    {
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        agent.destination = patrolPoints[currentPatrolIndex].position;
    }

    protected void TransitionToState(State newState)
    {
        currentState = newState;
        OnStateChange(newState);
    }

    protected virtual void OnStateChange(State newState)
    {
        // Default implementation can be overridden by subclasses
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
        GameObject healthBarUI = Instantiate(healthBarUIPrefab, transform.position + Vector3.up * 2, Quaternion.identity, transform); // Adjust Vector3.up * 2 as needed for height above enemy
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

