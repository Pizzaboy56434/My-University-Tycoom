using UnityEngine;
using TMPro;

public class RatingSystem : MonoBehaviour
{
    public TextMeshProUGUI ratingText;
    public BuildingManager buildManager;

    [Header("Formula Settings")]
    public float campusCapacity = 4f; // Set this to 4 in the Inspector

    void Update()
    {
        if (buildManager == null || ratingText == null) return;

        // 1. Calculate Total Rooms
        int totalRooms = buildManager.numberOfClassrooms + buildManager.numberOfLibraries + buildManager.numberOfCanteens;

        // 2. Find All Students
        StudentStats[] students = FindObjectsByType<StudentStats>(FindObjectsSortMode.None);

        float totalHappiness = 0f;
        float averageHappiness = 0f;
        
        // 3. Calculate Average Happiness
        if (students.Length > 0)
        {
            foreach (StudentStats student in students)
            {
                totalHappiness += student.happiness;
            }

            averageHappiness = totalHappiness / students.Length;
        }

        // 4. Normalize Happiness (turn 0-100 into 0.0-1.0)
        float normalizedHappiness = averageHappiness / 100f;

        // 5. Apply the Formula
        float rating = (totalRooms + normalizedHappiness) / campusCapacity;

        // 6. Determine the Campus Title and Color
        string campusTitle = "";

        if (rating < 4f)
        {
            campusTitle = "Struggling";
            ratingText.color = Color.red;
        }
        else if (rating >= 4f && rating < 8f)
        {
            campusTitle = "Average";
            ratingText.color = Color.yellow;
        }
        else
        {
            campusTitle = "Elite";
            ratingText.color = Color.green;
        }

        // 7. Update the UI
        ratingText.text = "Rating: " + rating.ToString("F1") + " (" + campusTitle + ")";
    }
}