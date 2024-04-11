using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolArea : MonoBehaviour
{
    public float radius = 10f;

   // Use this if you're selecting a random point within a radius

    public Vector3 GetRandomPoint()
    {
        Vector3 randomPoint = Random.insideUnitSphere * radius + transform.position;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, radius, NavMesh.AllAreas))
        {
            return hit.position;
        }
        return transform.position; // Fallback
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position to indicate the patrol area
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
