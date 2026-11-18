using UnityEngine;

public class StudentStats : MonoBehaviour
{
    public enum StudentStandard { Low, Medium,}

    [Header("Student Identity")]
    public StudentStandard standard;

    [Header("Current Happiness")]
    public float happiness = 100f; // Starts fully happy

    // Reference to the Building Manager to check campus stats
    private BuildingManager buildManager;

    void Start()
    {
        // 1. Randomize their standard when they spawn
        int randomStandard = Random.Range(0, 3); // 0=Low, 1=Medium, 2=High
        standard = (StudentStandard)randomStandard;

        // 2. Name the object in the hierarchy so we can see their standard
        gameObject.name = standard.ToString() + " Standard Student";

        // 3. Find the Building Manager in the scene
        buildManager = FindObjectOfType<BuildingManager>();

        Debug.Log("Spawned a " + standard + " standard student.");
    }

    void Update()
    {
        // If the manager doesn't exist, do nothing
        if (buildManager == null) return;

        // 1. Check what they need based on their standard and adjust happiness
        if (standard == StudentStandard.Low)
        {
            // Low standard: Happy if there is at least 1 classroom
            if (buildManager.numberOfClassrooms >= 1)
                happiness += 2f * Time.deltaTime; // Gain happiness
            else
                happiness -= 0.5f * Time.deltaTime; // Lose happiness
        }
        else if (standard == StudentStandard.Medium)
        {
            // Medium standard: Needs 1 classroom AND 1 library
            if (buildManager.numberOfClassrooms >= 1 && buildManager.numberOfLibraries >= 1)
                happiness += 2f * Time.deltaTime;
            else
                happiness -= 0.8f * Time.deltaTime;
        }
        
        // Clamp happiness so it doesn't go over 100 or below 0
        happiness = Mathf.Clamp(happiness, 0f, 100f);

        // 2. If they hit 0, they leave
        if (happiness <= 0)
        {
            Debug.Log(gameObject.name + " is unhappy and is leaving the university!");
            Destroy(gameObject);
        }
    }
}