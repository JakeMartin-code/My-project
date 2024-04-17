using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : EnemyManager
{
    public float attackRange = 2f;
    public int attackDamage = 20;

    protected override void HostileBehavior()
    {
        agent.SetDestination(player.position);
        if (Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            TransitionToState(State.Attacking);
        }
        else if (Vector3.Distance(transform.position, player.position) > detectionRange)
        {
            TransitionToState(State.Patrolling);
        }
    }

    protected override void AttackBehavior()
    {
        agent.isStopped = true;
        if (Vector3.Distance(transform.position, player.position) > attackRange)
        {
            TransitionToState(State.Hostile);
        }
        else if (attackTimer <= 0)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange);
            foreach (Collider col in hitColliders)
            {
                if (col.CompareTag("Player"))
                {
                    PlayerStats player = col.GetComponent<PlayerStats>();
                    if (player != null)
                    {
                      
                        player.TakeDamage(attackDamage);
                        player.StopRegeneration();
                    }
                }
            }
            attackTimer = attackCooldown;
        }
    }

    protected override void OnStateChange(State newState)
    {
        if (newState == State.Attacking)
        {
            agent.isStopped = true;
        }
        else
        {
            agent.isStopped = false;
        }
    }
}