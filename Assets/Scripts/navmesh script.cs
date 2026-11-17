using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class navmesh : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public List<Transform> wayPoint;

    NavMeshAgent navMeshAgent;

    public int currentWayPointIndex = 0;


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

    // Update is called once per frame
    void Update()
    {
        Walking();
    }

    void Walking()
    {
        
        if (wayPoint.Count == 0)
        {
            return;
        }

        float distanceToWayPoint = Vector3.Distance(wayPoint[currentWayPointIndex].position, transform.position);
   
        if(distanceToWayPoint <= 3f)
        {
            // arrived
            Debug.Log("Near enough");

            //set next destination
            currentWayPointIndex = (currentWayPointIndex + 1) % wayPoint.Count;
        }

        navMeshAgent.SetDestination(wayPoint[currentWayPointIndex].position);
    
    }

}
