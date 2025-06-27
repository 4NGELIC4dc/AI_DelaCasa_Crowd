using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FleeBehavior : MonoBehaviour
{

    public Transform target; // To flee from
    private NavMeshAgent agent;
    public float fleeDistance = 5f;

    public static bool isFleeing;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>(); 

        if (target == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                target = player.transform;
            }
        }
    }
    void Update()
    {
        if (IsTargetClose())
        {
            FleeFromTheTarget();
        }
    }

    private bool IsTargetClose()
    {
        float distanceTarget = Vector3.Distance(transform.position, target.position);
        isFleeing = distanceTarget <= fleeDistance;
        return isFleeing;
    }

    private void FleeFromTheTarget()
    {
        RaycastHit hit;
        Vector3 fleeDirection = transform.position - target.position;
        fleeDirection.y = 0; // Flatten direction

        // Raycast to check for wall/building in flee direction
        if (Physics.Raycast(transform.position, fleeDirection.normalized, out hit, 2f))
        {
            // If hit wall/building, pick a side direction
            fleeDirection = Vector3.Cross(fleeDirection, Vector3.up); // Turn perpendicular
        }

        Vector3 fleePosition = transform.position + fleeDirection.normalized * 10f;

        if (NavMesh.SamplePosition(fleePosition, out NavMeshHit navHit, 10, NavMesh.AllAreas))
        {
            agent.SetDestination(navHit.position);
        }
    }
}
