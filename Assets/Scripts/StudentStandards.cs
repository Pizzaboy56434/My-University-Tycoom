using UnityEngine;

public class StudentStats : MonoBehaviour
{
    public float personalSpace = 2f;
    public enum StudentStandard { Low, Medium }

    [Header("Student Identity")]
    public StudentStandard standard;

    [Header("Current Happiness")]
    public float happiness = 100f;

    private BuildingManager buildManager;

    // Variables to prevent popup spam
    private float messageCooldown = 6f;
    private float currentCooldown = 0f;
    private float crowdCooldown = 0f;

    void Start()
    {
        // 1. Randomize their standard (0=Low, 1=Medium)
        int randomStandard = Random.Range(0, 2);
        standard = (StudentStandard)randomStandard;

        // 2. Name the object
        gameObject.name = standard.ToString() + " Standard Student";

        // 3. Find the Building Manager
        buildManager = FindAnyObjectByType<BuildingManager>();

        Debug.Log("Spawned a " + standard + " standard student.");
    }

    void Update()
    {
        if (buildManager == null) return;

        // Count down the cooldown timer
        if (currentCooldown > 0) currentCooldown -= Time.deltaTime;
        if (crowdCooldown > 0) crowdCooldown -= Time.deltaTime;

        if (standard == StudentStandard.Low)
        {
            if (buildManager.numberOfClassrooms >= 1)
            {
                happiness += 2f * Time.deltaTime;
            }
            else
            {
                happiness -= 1f * Time.deltaTime; // Sped up drain
            }
        }
        else if (standard == StudentStandard.Medium)
        {
            // Medium standard: Needs 1 classroom AND 1 library
            if (buildManager.numberOfClassrooms >= 1 && buildManager.numberOfLibraries >= 1)
            {
                happiness += 2f * Time.deltaTime;
            }
            else
            {
                // Sped up the drain so you don't have to wait as long
                happiness -= 2f * Time.deltaTime;

                // Debug line to prove it's working in the Console
                Debug.Log(gameObject.name + " is unhappy! Happiness: " + Mathf.Round(happiness));

                // Show popup if they are unhappy and the timer is ready
                if (currentCooldown <= 0 && PopupManager.Instance != null)
                {
                    PopupManager.Instance.ShowMessage(gameObject.name + ": I need a library to study!");
                    currentCooldown = messageCooldown;
                }
            }
        }
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, personalSpace);
        int nearbyStudents = 0;

        // 2. Count how many other students are in the bubble
        foreach (var hitCollider in hitColliders)
        {
            StudentStats otherStudent = hitCollider.GetComponent<StudentStats>();
            if (otherStudent != null && otherStudent != this)
            {
                nearbyStudents++;
            }
        }

        // 3. If crowded, drain happiness and show a popup
        if (nearbyStudents >= 1)
        {
            happiness -= 3f * Time.deltaTime; // Drain faster than normal

            if (crowdCooldown <= 0 && PopupManager.Instance != null)
            {
                PopupManager.Instance.ShowMessage(gameObject.name + ": Too crowded! I need space!");
                crowdCooldown = 5f;
            }
        }

        // Clamp happiness
        happiness = Mathf.Clamp(happiness, 0f, 100f);

        // If they hit 0, they leave
        if (happiness <= 0)
        {
            if (PopupManager.Instance != null)
            {
                PopupManager.Instance.ShowMessage(gameObject.name + " dropped out due to unhappiness!");
            }

            Debug.Log(gameObject.name + " is unhappy and is leaving the university!");
            Destroy(gameObject);
        }
    }
}