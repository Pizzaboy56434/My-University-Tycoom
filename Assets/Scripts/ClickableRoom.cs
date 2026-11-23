using UnityEngine;

public class ClickableRoom : MonoBehaviour
{
    // These are the variables the BuildingManager needs!
    public Vector3 plotPosition;
    public Quaternion plotRotation;

    void OnMouseDown()
    {
        if (RoomManager.Instance != null)
        {
            RoomManager.Instance.SelectRoom(gameObject);
        }
    }
}