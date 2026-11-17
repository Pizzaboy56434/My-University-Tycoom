using System.Collections.Generic;
using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    public static WaypointManager Instance;
    public List<Transform> allWaypoints = new List<Transform>();

    void Awake()
    {
        // This allows other scripts to easily find this manager
        Instance = this;
    }
}
