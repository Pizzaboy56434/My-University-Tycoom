using UnityEngine;

public class CameraForMobile : MonoBehaviour
{
    [Header("Pan Settings")]
    public float panSpeed = 0.5f;

    [Header("Zoom Settings")]
    public float zoomSpeed = 10f;
    public float minZoom = 5f;
    public float maxZoom = 20f;

    private Vector3 dragOrigin;

    void Update()
    {
        HandlePan();
        HandleZoom();
    }

    void HandlePan()
    {
        // 1. When the mouse button is first pressed, save where it started
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = Input.mousePosition;
            return;
        }

        // 2. While the mouse button is held down, move the camera
        if (Input.GetMouseButton(0))
        {
            // Don't move the camera if the player is clicking a UI button
            if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) return;

            // Calculate the difference between where the mouse started and where it is now
            Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);

            // Convert that difference into a 3D movement
            Vector3 move = new Vector3(pos.x * panSpeed, 0, pos.y * panSpeed);

            // Move the camera
            transform.Translate(-move, Space.World);

            // Update the origin so the movement is smooth
            dragOrigin = Input.mousePosition;
        }
    }

    void HandleZoom()
    {
        // Get the scroll wheel input
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0)
        {
            // Calculate new Y position (height)
            float newY = transform.position.y - (scroll * zoomSpeed);

            // Clamp it so the player can't zoom too far in or out
            newY = Mathf.Clamp(newY, minZoom, maxZoom);

            // Apply the new height
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        }
    }
}