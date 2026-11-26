using UnityEngine;

public class StudentSpawner : MonoBehaviour
{
    public GameObject studentPrefab;

    [Header("Spawn Settings")]
    public int maxStudentsInWorld = 10; // The total limit on campus
    public int studentsPerRoom = 2;     // How many spawn per room built

    public void SpawnStudent()
    {
        if (studentPrefab == null) return;

        // 1. Count how many students are already in the scene
   
        int currentStudentCount = Object.FindObjectsByType<navmesh>(FindObjectsSortMode.None).Length;

        // 2. Check if we are already at the limit
        if (currentStudentCount >= maxStudentsInWorld)
        {
            Debug.Log("Campus is full! Cannot spawn more students.");
            return;
        }

        // 3. Spawn them, but don't go over the limit
        for (int i = 0; i < studentsPerRoom; i++)
        {
            // Stop if we hit the max limit mid-loop
            if (currentStudentCount >= maxStudentsInWorld) break;

            Vector3 randomOffset = new Vector3(
                Random.Range(-2f, 2f),
                0,
                Random.Range(-2f, 2f)
            );

            Instantiate(studentPrefab, transform.position + randomOffset, Quaternion.identity);
            currentStudentCount++; // Increase the count so we don't go over
        }
    }
}