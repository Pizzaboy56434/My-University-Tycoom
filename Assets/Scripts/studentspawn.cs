using UnityEngine;

public class StudentSpawner : MonoBehaviour
{
    public GameObject studentPrefab;

    public void SpawnStudent()
    {
        if (studentPrefab != null)
        {
            Instantiate(studentPrefab, transform.position, Quaternion.identity);
            Debug.Log("A student spawned!");
        }
    }
}
