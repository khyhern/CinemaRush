using System.Collections;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject[] npcPrefabs;     // Array of NPC prefabs
    public Transform spawnPoint;        // The location where NPCs will spawn
    public float spawnInterval = 2f;    // Time interval between spawns (in seconds)
    public int maxNPCs = 10;            // Maximum number of NPCs allowed

    private int currentNPCCount = 0;    // Current number of spawned NPCs

    void Start()
    {
        // Start spawning NPCs
        StartCoroutine(SpawnNPCs());
    }

    private IEnumerator SpawnNPCs()
    {
        while (true) // Keep the coroutine running indefinitely
        {
            // Check if we can spawn new NPCs
            if (currentNPCCount < maxNPCs)
            {
                SpawnNPC();
            }

            // Wait for the next spawn attempt
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnNPC()
    {
        if (currentNPCCount < maxNPCs && npcPrefabs.Length > 0)
        {
            // Select a random NPC prefab
            GameObject selectedPrefab = npcPrefabs[Random.Range(0, npcPrefabs.Length)];

            // Spawn the selected NPC
            GameObject npcObject = Instantiate(selectedPrefab, spawnPoint.position, spawnPoint.rotation);
            NPCController npcController = npcObject.GetComponent<NPCController>();
            if (npcController != null)
            {
                npcController.SetSpawner(this); // Pass the reference to the spawner
            }
            currentNPCCount++;
        }
    }

    public void OnNPCProcessed()
    {
        // Decrement the NPC count
        currentNPCCount--;

        // Log the current count for debugging
        Debug.Log($"NPC processed. Current count: {currentNPCCount}");
    }
}
