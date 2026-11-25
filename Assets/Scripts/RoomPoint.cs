using UnityEngine;

public class RoomPoint : MonoBehaviour
{
    public Transform hangoutSpot;

    void Start()
    {
        if (WaypointManager.Instance != null && hangoutSpot != null)
        {
            WaypointManager.Instance.AddRoomPoint(hangoutSpot);

            Debug.Log("Room registered! Total rooms: " + WaypointManager.Instance.roomWaypoints.Count);
        }
        else
        {
            Debug.LogError("RoomPoint failed! Manager exists: " + (WaypointManager.Instance != null) + " Spot exists: " + (hangoutSpot != null));
        }
    }

    // NEW: Remove the room from the list when it is deleted
    void OnDestroy()
    {
        if (WaypointManager.Instance != null && hangoutSpot != null)
        {
            WaypointManager.Instance.roomWaypoints.Remove(hangoutSpot);
            Debug.Log("Room removed! Total rooms remaining: " + WaypointManager.Instance.roomWaypoints.Count);
        }
    }
}