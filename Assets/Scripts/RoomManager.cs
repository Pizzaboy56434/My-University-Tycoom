using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance;

    [Header("UI References")]
    public GameObject menuPanel;

    [Header("Prefabs")]
    public GameObject plotPrefab;

    [Header("References")]
    public BuildingManager buildManager; // Drag your GameManager here

    private GameObject selectedRoom;

    void Awake()
    {
        Instance = this;
    }

    public void SelectRoom(GameObject room)
    {
        selectedRoom = room;

        if (menuPanel != null)
        {
            menuPanel.SetActive(true);
        }
    }

    public void DeleteRoom()
    {
        if (selectedRoom != null)
        {
            // 1. Subtract the room count
            if (buildManager != null)
            {
                if (selectedRoom.name.Contains("Classroom")) buildManager.numberOfClassrooms--;
                if (selectedRoom.name.Contains("Library")) buildManager.numberOfLibraries--;
                if (selectedRoom.name.Contains("Canteen")) buildManager.numberOfCanteens--;
            }

            // 2. Get the ClickableRoom script and SAVE the position
            ClickableRoom roomScript = selectedRoom.GetComponent<ClickableRoom>();

            if (roomScript != null)
            {
                Vector3 savedPos = roomScript.plotPosition;
                Quaternion savedRot = roomScript.plotRotation;

                // 3. Hide the room first so it disappears visually
                selectedRoom.SetActive(false);

                // 4. Spawn the plot at the saved position
                if (plotPrefab != null)
                {
                    Instantiate(plotPrefab, savedPos, savedRot);
                }

                // 5. NOW destroy the room
                Destroy(selectedRoom);
            }
            else
            {
                Destroy(selectedRoom);
            }
        }

        // 6. Clear and hide
        selectedRoom = null;
        if (menuPanel != null) menuPanel.SetActive(false);
    }
    public void RotateRoom()
    {
        if (selectedRoom != null)
        {
            selectedRoom.transform.Rotate(0, 90, 0);
        }
    }

    public void CloseMenu()
    {
        selectedRoom = null; // Clear the selection!
        if (menuPanel != null) menuPanel.SetActive(false);
    }
}