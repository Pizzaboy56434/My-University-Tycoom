using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    [Header("Room Prefabs")]
    public GameObject classroomPrefab;
    public GameObject libraryPrefab;
    public GameObject canteenPrefab;

    [Header("References")]
    public GameObject buildMenuPanel;
    public StudentSpawner spawnerScript;

    [Header("Campus Stats")]
    public int numberOfClassrooms = 0;
    public int numberOfLibraries = 0;
    public int numberOfCanteens = 0;

    private GameObject currentSelectedPlot;
    private Camera mainCam;

    void Start()
    {
        mainCam = Camera.main;

        // Hide the build menu at the start of the game
        if (buildMenuPanel != null)
        {
            buildMenuPanel.SetActive(false);
        }
    }

    void Update()
    {
        // Player clicks with Left Mouse Button
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // If we clicked a plot of land
                if (hit.collider.CompareTag("Plot"))
                {
                    currentSelectedPlot = hit.collider.gameObject;
                    buildMenuPanel.SetActive(true); // Show the menu
                }
            }
        }
    }

    // --- These functions are linked to your UI buttons ---

    public void BuildClassroom()
    {
        BuildRoom(classroomPrefab);
    }

    public void BuildLibrary()
    {
        BuildRoom(libraryPrefab);
    }

    public void BuildCanteen()
    {
        BuildRoom(canteenPrefab);
    }

    void BuildRoom(GameObject roomPrefab)
    {
        // If no plot is selected, do nothing
        if (currentSelectedPlot == null) return;

        // 1. Spawn the room on top of the empty plot
        Vector3 spawnPos = currentSelectedPlot.transform.position;
        spawnPos.y += 0.1f; // Raise it slightly so it sits on the floor

        if (roomPrefab != null)
        {
            Instantiate(roomPrefab, spawnPos, Quaternion.identity);

            // Count the new room so the students can see it
            if (roomPrefab == classroomPrefab) numberOfClassrooms++;
            if (roomPrefab == libraryPrefab) numberOfLibraries++;
            if (roomPrefab == canteenPrefab) numberOfCanteens++;
        }

        // 2. Delete the empty plot so we can't build on it again
        Destroy(currentSelectedPlot);

        // 3. Hide the menu
        buildMenuPanel.SetActive(false);

        // 4. Spawn the students!
        if (spawnerScript != null)
        {
            spawnerScript.SpawnStudent();
        }
    }
}