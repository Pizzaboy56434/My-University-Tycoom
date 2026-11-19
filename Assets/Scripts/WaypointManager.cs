using System.Collections.Generic;
using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    public static WaypointManager Instance;

    public List<Transform> allWaypoints = new List<Transform>();
    public List<Transform> roomWaypoints = new List<Transform>();

    void Awake()
    {
        Instance = this;
    }

    public void AddRoomPoint(Transform roomTransform)
    {
        roomWaypoints.Add(roomTransform);
    }
}