using UnityEngine;

public class RoomAdjuster : MonoBehaviour
{
   void OnMouseDown()
    {
        Debug.Log("Player has clicked on a room: " + gameObject.name);
    }
}
