using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemy : EnemyManager
{
    public GameObject projectilePrefab;
    public float shootRange = 10f; 
    public float minimumRange = 5f;
    public Transform shootingPoint; 

    protected override void HostileBehavior()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer > minimumRange)
        {
            agent.SetDestination(player.position);
            agent.isStopped = false;
        }

       
        if (distanceToPlayer <= shootRange && attackTimer <= 0)
        {
            TransitionToState(State.Attacking);
        }
        else if (distanceToPlayer > detectionRange)
        {

            TransitionToState(State.Patrolling);
        }
    }

    protected override void AttackBehavior()
    {
        
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer > shootRange)
        {
            TransitionToState(State.Hostile);
            return;
        }

       
        ShootProjectile();
        attackTimer = attackCooldown; 

        TransitionToState(State.Hostile); 
    }

    private void ShootProjectile()
    {
        if (shootingPoint != null)
        {
            GameObject projectile = ProjectilePool.Instance.GetProjectile();
            projectile.transform.position = shootingPoint.position;
            projectile.transform.rotation = Quaternion.LookRotation(player.position - transform.position);
            projectile.SetActive(true);
            projectile.GetComponent<Projectile>().Launch(projectile.transform.forward);
        }
    }

    protected override void OnStateChange(State newState)
    {
        base.OnStateChange(newState);
   
    }
}
