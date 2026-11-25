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
            // 1. Get the room's script
            ClickableRoom roomScript = selectedRoom.GetComponent<ClickableRoom>();

            // 2. Subtract the correct room count
            if (buildManager != null && roomScript != null)
            {
                if (roomScript.roomType == "Classroom") buildManager.numberOfClassrooms--;
                if (roomScript.roomType == "Library") buildManager.numberOfLibraries--;
                if (roomScript.roomType == "Canteen") buildManager.numberOfCanteens--;
            }

            // 3. Respawn the plot and destroy the room
            if (roomScript != null)
            {
                roomScript.DestroyAndRespawnPlot();
            }

            selectedRoom = null;
        }

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