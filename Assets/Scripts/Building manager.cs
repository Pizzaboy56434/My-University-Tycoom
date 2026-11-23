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

        if (buildMenuPanel != null)
        {
            buildMenuPanel.SetActive(false);
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Plot"))
                {
                    currentSelectedPlot = hit.collider.gameObject;
                    buildMenuPanel.SetActive(true);
                }
            }
        }
    }

    public void BuildClassroom() { BuildRoom(classroomPrefab); }
    public void BuildLibrary() { BuildRoom(libraryPrefab); }
    public void BuildCanteen() { BuildRoom(canteenPrefab); }

    void BuildRoom(GameObject roomPrefab)
    {
        if (currentSelectedPlot == null) return;

        Vector3 spawnPos = currentSelectedPlot.transform.position;
        spawnPos.y += 0.1f;

        if (roomPrefab != null)
        {
            // 1. Spawn the room
            GameObject newRoom = Instantiate(roomPrefab, spawnPos, Quaternion.identity);

            // 2. Tell the room where its plot was, so it can respawn it later
            ClickableRoom roomScript = newRoom.GetComponent<ClickableRoom>();
            if (roomScript != null)
            {
                roomScript.plotPosition = currentSelectedPlot.transform.position;
                roomScript.plotRotation = currentSelectedPlot.transform.rotation;
            }

            // 3. Count the new room
            if (roomPrefab == classroomPrefab) numberOfClassrooms++;
            if (roomPrefab == libraryPrefab) numberOfLibraries++;
            if (roomPrefab == canteenPrefab) numberOfCanteens++;
        }

        // 4. Delete the empty plot
        Destroy(currentSelectedPlot);

        // 5. Hide the menu
        buildMenuPanel.SetActive(false);

        // 6. Spawn the students
        if (spawnerScript != null)
        {
            spawnerScript.SpawnStudent();
        }
    }
}