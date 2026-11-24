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

            // 2. Get the room's script and SAVE the position BEFORE we destroy it
            ClickableRoom roomScript = selectedRoom.GetComponent<ClickableRoom>();
            Vector3 spawnPos = Vector3.zero;
            Quaternion spawnRot = Quaternion.identity;

            if (roomScript != null)
            {
                spawnPos = roomScript.plotPosition;
                spawnRot = roomScript.plotRotation;
            }

            // 3. Destroy the room
            Destroy(selectedRoom);

            // 4. NOW spawn the new plot at the saved position
            if (plotPrefab != null)
            {
                Instantiate(plotPrefab, spawnPos, spawnRot);
            }
        }

        // 5. Clear the selection
        selectedRoom = null;

        // 6. Hide the menu
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