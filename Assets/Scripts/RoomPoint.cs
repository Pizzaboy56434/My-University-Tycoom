using UnityEngine;

public class RoomPoint : MonoBehaviour
{
    public Transform hangoutSpot;

    void Start()
    {
        if (WaypointManager.Instance != null && hangoutSpot != null)
        {
            WaypointManager.Instance.AddRoomPoint(hangoutSpot);

            // ADD THIS LINE:
            Debug.Log("Room registered! Total rooms: " + WaypointManager.Instance.roomWaypoints.Count);
        }
        else
        {
            // ADD THIS LINE:
            Debug.LogError("RoomPoint failed! Manager exists: " + (WaypointManager.Instance != null) + " Spot exists: " + (hangoutSpot != null));
        }
    }
}