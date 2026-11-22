using UnityEngine;
using TMPro;

public class RatingSystem : MonoBehaviour
{
    public TextMeshProUGUI ratingText;
    public BuildingManager buildManager;
    public float campusCapacity = 10f;

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

        // 4. Apply the Formula
        float rating = (totalRooms + averageHappiness) / campusCapacity;

        // 5. Determine the Campus Title and Color (The new unique part!)
        string campusTitle = "";

        if (rating < 2f)
        {
            campusTitle = "Struggling";
            ratingText.color = Color.red;
        }
        else if (rating >= 2f && rating < 4f)
        {
            campusTitle = "Average";
            ratingText.color = Color.yellow;
        }
        else
        {
            campusTitle = "Elite";
            ratingText.color = Color.green;
        }

        // 6. Update the UI
        ratingText.text = "Rating: " + rating.ToString("F1") + " (" + campusTitle + ")";
    }
}