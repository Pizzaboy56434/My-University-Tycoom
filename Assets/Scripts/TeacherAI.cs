using UnityEngine;
using UnityEngine.AI;

public class TeacherAI : MonoBehaviour
{
    private NavMeshAgent agent;
    private float waitTimer = 0f;
    private bool isTeaching = false;

    void Start()
    {
        Debug.Log("Teacher spawned and is trying to move!");
        agent = GetComponent<NavMeshAgent>();

        if (WaypointManager.Instance != null)
        {
            GoToRandomClassroom();
        }
    }

    void Update()
    {
        if (isTeaching)
        {
            waitTimer -= Time.deltaTime;

            if (waitTimer <= 0)
            {
                isTeaching = false;
                Debug.Log("Class is over! Teacher is going for a walk.");
                GoToRandomWaypoint();
            }
            return;
        }

        // Check if they arrived
        if (agent.hasPath && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            Debug.Log("Teacher arrived at destination.");

            isTeaching = true;
            waitTimer = 10f;
            agent.ResetPath();
        }
    }

    void GoToRandomClassroom()
    {
        if (WaypointManager.Instance != null && WaypointManager.Instance.roomWaypoints.Count > 0)
        {
            int randomRoom = Random.Range(0, WaypointManager.Instance.roomWaypoints.Count);

            // Get the position of the room
            Vector3 targetPos = WaypointManager.Instance.roomWaypoints[randomRoom].position;

            // Add a slight offset so they stand NEXT to the room, not inside it
            targetPos += new Vector3(2f, 0, 2f);

            agent.SetDestination(targetPos);
        }
    }

    void GoToRandomWaypoint()
    {
        if (WaypointManager.Instance != null && WaypointManager.Instance.allWaypoints.Count > 0)
        {
            int randomPoint = Random.Range(0, WaypointManager.Instance.allWaypoints.Count);
            agent.SetDestination(WaypointManager.Instance.allWaypoints[randomPoint].position);

            // After reaching the waypoint, wait a bit and then go back to a classroom
            Invoke(nameof(GoToRandomClassroom), 5f);
        }
    }
}