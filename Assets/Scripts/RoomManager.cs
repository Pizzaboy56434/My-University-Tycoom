using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance;

    [Header("UI References")]
    public GameObject menuPanel;

    [Header("Prefabs")]
    public GameObject plotPrefab; // Drag your Plot Prefab here!

    private GameObject selectedRoom;

    void Awake()
    {
        // This lets the rooms find this script
        Instance = this;
    }

    public void SelectRoom(GameObject room)
    {
        selectedRoom = room;

        if (menuPanel != null)
        {
            menuPanel.SetActive(true);
            // Optional: If you don't want it at the mouse, delete the line below
            // menuPanel.transform.position = Input.mousePosition; 
        }
    }

    public void DeleteRoom()
    {
        if (selectedRoom != null)
        {
            // 1. Get the room's script to find out where the plot used to be
            ClickableRoom roomScript = selectedRoom.GetComponent<ClickableRoom>();

            // 2. Spawn a new plot at that exact spot
            if (roomScript != null && plotPrefab != null)
            {
                Instantiate(plotPrefab, roomScript.plotPosition, roomScript.plotRotation);
            }

            // 3. Destroy the room
            Destroy(selectedRoom);
        }

        // 4. Hide the menu
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
        if (menuPanel != null) menuPanel.SetActive(false);
    }
}