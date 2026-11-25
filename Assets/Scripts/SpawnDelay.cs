using UnityEngine;

public class SpawnDelay : MonoBehaviour
{
    private GameObject prefabToSpawn;
    private Vector3 pos;
    private Quaternion rot;

    public void Setup(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        prefabToSpawn = prefab;
        pos = position;
        rot = rotation;
        Invoke(nameof(SpawnNow), 0.1f); // Wait 0.1 seconds
    }

    void SpawnNow()
    {
        if (prefabToSpawn != null)
        {
            Instantiate(prefabToSpawn, pos, rot);
        }
        Destroy(gameObject); // Delete the timer object
    }
}