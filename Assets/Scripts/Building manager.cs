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

    private GameObject currentSelectedPlot;
    private Camera mainCam;

    void Start()
    {
        mainCam = Camera.main;
        if (buildMenuPanel != null) buildMenuPanel.SetActive(false);
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

        if (roomPrefab != null) Instantiate(roomPrefab, spawnPos, Quaternion.identity);

        Destroy(currentSelectedPlot);
        buildMenuPanel.SetActive(false);

        if (spawnerScript != null) spawnerScript.SpawnStudent();
    }
}
