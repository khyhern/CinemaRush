using System.Collections;
using UnityEngine;

public class CupSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public Transform spawnPoint;  // The point where prefabs will spawn
    public GameObject prefab1;    // First prefab to spawn
    public float spawnCooldown = 3f;  // Cooldown before respawning

    private GameObject currentPrefab;  // Tracks the last spawned prefab
    private bool isSpawning = false;   // Prevents multiple spawns during cooldown

    private void Start()
    {
        SpawnPrefabs(); // Initial spawn
    }

    private void OnTriggerStay(Collider other)
    {
        // Check if the detected object is the currently spawned prefab
        if (currentPrefab != null && other.gameObject == currentPrefab)
        {
            // The prefab is still in the trigger area, no need to spawn a new one
            return;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the leaving object is the currently spawned prefab
        if (currentPrefab != null && other.gameObject == currentPrefab)
        {
            // Start the cooldown before spawning a new one
            if (!isSpawning)
            {
                StartCoroutine(SpawnWithCooldown());
            }
        }
    }

    private void SpawnPrefabs()
    {
        if (spawnPoint == null || prefab1 == null)
        {
            Debug.LogWarning("Spawn settings are not assigned properly!");
            return;
        }

        // Spawn first prefab
        currentPrefab = Instantiate(prefab1, spawnPoint.position, spawnPoint.rotation);

    }


    private IEnumerator SpawnWithCooldown()
    {
        isSpawning = true;
        yield return new WaitForSeconds(spawnCooldown);
        SpawnPrefabs();
        isSpawning = false;
    }
}
