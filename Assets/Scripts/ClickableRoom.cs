using UnityEngine;

public class ClickableRoom : MonoBehaviour
{
    public string roomType;
    public Vector3 plotPosition;
    public Quaternion plotRotation;
    public GameObject plotPrefab;

    void OnMouseDown()
    {
        if (RoomManager.Instance != null)
        {
            RoomManager.Instance.SelectRoom(gameObject);
        }
    }

    // When the room is deleted, we put the plot back
    public void DestroyAndRespawnPlot()
    {
        // 1. Spawn the plot instantly
        if (plotPrefab != null)
        {
            Instantiate(plotPrefab, plotPosition, plotRotation);
        }

        // 2. Destroy the room
        Destroy(gameObject);
    }
}