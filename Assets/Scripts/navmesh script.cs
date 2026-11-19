using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class navmesh : MonoBehaviour
{
    public List<Transform> wayPoint;
    NavMeshAgent navMeshAgent;
    public int currentWayPointIndex = 0;

    public float waypointTolerance = 1.5f;

    IEnumerator Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        // Wait one frame for the Manager to wake up
        yield return null;

        if (WaypointManager.Instance != null)
        {
            wayPoint = WaypointManager.Instance.allWaypoints;
        }

        if (wayPoint.Count > 0)
        {
            currentWayPointIndex = Random.Range(0, wayPoint.Count);
            navMeshAgent.speed = Random.Range(3.0f, 4.5f);
        }
    }

    void Update()
    {
        Walking();
    }

    void Walking()
    {
        if (wayPoint.Count == 0 || !navMeshAgent.isOnNavMesh)
            return;

        // Check if we are close enough to the destination
        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance + 1.0f)
        {
            // Move to the next waypoint index
            currentWayPointIndex = (currentWayPointIndex + 1) % wayPoint.Count;

            // Set the new destination (this function now includes the room logic)
            SetNewDestination();
        }
    }

    void SetNewDestination()
    {
        Vector3 targetPos;

        // If rooms exist, 70% chance they walk to a room, 30% chance they patrol
        if (WaypointManager.Instance != null && WaypointManager.Instance.roomWaypoints.Count > 0 && Random.value > 0.3f)
        {
            // Pick a random room
            int randomRoom = Random.Range(0, WaypointManager.Instance.roomWaypoints.Count);
            targetPos = WaypointManager.Instance.roomWaypoints[randomRoom].position;
        }
        else
        {
            // Go to a standard waypoint
            targetPos = wayPoint[currentWayPointIndex].position;
        }

        // Random offset so they don't crowd
        Vector3 randomOffset = new Vector3(
            Random.Range(-waypointTolerance, waypointTolerance),
            0,
            Random.Range(-waypointTolerance, waypointTolerance)
        );

        navMeshAgent.SetDestination(targetPos + randomOffset);
    }
}