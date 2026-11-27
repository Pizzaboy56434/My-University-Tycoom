using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class navmesh : MonoBehaviour
{
    public List<Transform> wayPoint;
    NavMeshAgent navMeshAgent;

    [Header("Assignment System")]
    public TextMeshPro thoughtBubble;
    public List<string> assignments = new List<string>();
    public int currentWayPointIndex = 0;
    public string currentTask = "";
    public float taskTimer = 0f;
    public bool isDoingTask = false;

    private float waypointTolerance = 1.5f;
    private float currentCooldown = 0f;
    private float messageCooldown = 6f;
    private StudentStats stats;

    public float stuckTimer = 0f;
    private float frustrationCooldown = 0f;


    IEnumerator Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        stats = GetComponent<StudentStats>();
        thoughtBubble = GetComponentInChildren<TextMeshPro>();

        yield return null;

        if (WaypointManager.Instance != null)
        {
            wayPoint = WaypointManager.Instance.allWaypoints;
        }

        // Give the student their schedule
        assignments.Add("Classroom");
        assignments.Add("Library");

        PickNextAssignment();
    }

    void Update()
    {
        if (wayPoint.Count == 0 || !navMeshAgent.isOnNavMesh) return;

        if (currentCooldown > 0) currentCooldown -= Time.deltaTime;

        // --- 1. IF THEY ARE DOING A TASK (Studying) ---
        if (isDoingTask)
        {
            taskTimer -= Time.deltaTime;
            navMeshAgent.SetDestination(transform.position); // Stand still

            if (taskTimer <= 0)
            {
                isDoingTask = false;
                PickNextAssignment(); // Move to the next checklist item
            }
            return; // Don't do the walking logic while studying
        }

        // --- 2. FRUSTRATION LOGIC ---
        if (frustrationCooldown > 0) frustrationCooldown -= Time.deltaTime;

        if (currentTask != "" && !isDoingTask)
        {
            // They are stuck if they are barely moving OR if they lost their path entirely
            bool isStuck = (navMeshAgent.velocity.magnitude < 0.1f) || (!navMeshAgent.hasPath && !navMeshAgent.pathPending);

            if (isStuck)
            {
                stuckTimer += Time.deltaTime;

                // If they've been stuck for 2 seconds, get frustrated
                if (stuckTimer > 2f)
                {
                    if (stats != null) stats.happiness -= 5f * Time.deltaTime;

                    if (frustrationCooldown <= 0 && PopupManager.Instance != null)
                    {
                        PopupManager.Instance.ShowMessage(gameObject.name + ": I'm stuck! Where is the door?");
                        frustrationCooldown = 6f;
                    }
                }

                // If they've been stuck for 10 seconds, they give up and wander
                if (stuckTimer > 10f)
                {
                    currentTask = "";
                    stuckTimer = 0f;
                    if (thoughtBubble != null) thoughtBubble.text = "Giving up...";
                    SetNewDestination();
                }
            }
            else
            {
                stuckTimer = 0f; // They are moving fine
            }
        }

        // --- 3. IF THEY ARE WALKING ---
        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance + 1.0f)
        {
            // They arrived at their destination
            if (currentTask != "")
            {
                // They arrived at a room! Start studying.
                isDoingTask = true;
                taskTimer = 5f; // Study for 5 seconds

                if (thoughtBubble != null) thoughtBubble.text = "Studying in " + currentTask;
                Debug.Log(gameObject.name + " is now in the " + currentTask);
            }
            else
            {
                // They arrived at a normal waypoint, just pick a new one
                currentWayPointIndex = (currentWayPointIndex + 1) % wayPoint.Count;
                SetNewDestination();
            }
        }
    }

    void PickNextAssignment()
    {
        // Check if they are hungry (Happiness below 20)
        if (stats != null && stats.happiness < 20f)
        {
            currentTask = "Canteen";
            if (thoughtBubble != null) thoughtBubble.text = "I'm hungry!";
            Debug.Log(gameObject.name + " is hungry! Going to the canteen.");
        }
        else if (assignments.Count > 0)
        {
            currentTask = assignments[0];
            assignments.RemoveAt(0);

            if (thoughtBubble != null) thoughtBubble.text = "Going to " + currentTask;
            Debug.Log(gameObject.name + " is going to the " + currentTask);
        }
        else
        {
            currentTask = "";
            if (thoughtBubble != null) thoughtBubble.text = "Wandering...";
            SetNewDestination();
            return;
        }

        // Find the room and go there
        Transform targetRoom = WaypointManager.Instance.GetRoomByType(currentTask);
        if (targetRoom != null)
        {
            navMeshAgent.SetDestination(targetRoom.position);
        }
        else
        {
            // NEW: Don't clear currentTask here! Let the frustration logic handle it.
            if (thoughtBubble != null) thoughtBubble.text = "Where is the " + currentTask + "?";
            Debug.LogWarning("Could not find a room of type: " + currentTask);
        }
    }

    void SetNewDestination()
    {
        Vector3 targetPos;
        currentTask = "";

        targetPos = wayPoint[currentWayPointIndex].position;

        Vector3 randomOffset = new Vector3(
            Random.Range(-waypointTolerance, waypointTolerance),
            0,
            Random.Range(-waypointTolerance, waypointTolerance)
        );

        navMeshAgent.SetDestination(targetPos + randomOffset);
    }
}