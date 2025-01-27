using System.Collections.Generic;
using UnityEngine;

public class QueueManager : MonoBehaviour
{
    public Transform spawnPoint; // Where NPCs spawn
    public List<Transform> queuePositions; // Queue points (QueuePoint1, QueuePoint2, etc.)
    public GameObject npcPrefab; // Prefab for NPCs
    public float spawnInterval = 5f; // Time between NPC spawns

    private Queue<GameObject> queue = new Queue<GameObject>(); // Queue of NPCs

    private void Start()
    {
        // Start spawning NPCs at intervals
        InvokeRepeating(nameof(SpawnNPC), 0f, spawnInterval);
    }

    private void SpawnNPC()
    {
        if (npcPrefab == null || spawnPoint == null || queuePositions.Count == 0) return;

        // Spawn an NPC at the spawn point
        GameObject npc = Instantiate(npcPrefab, spawnPoint.position, Quaternion.identity);
        queue.Enqueue(npc); // Add NPC to the queue
        AssignQueuePositions(); // Assign queue positions to NPCs
    }

    public void ServeNextCustomer()
    {
        if (queue.Count > 0)
        {
            // Remove the first NPC in the queue
            GameObject firstNPC = queue.Dequeue();
            Destroy(firstNPC); // Despawn NPC
            AssignQueuePositions(); // Move the queue forward
        }
    }

    private void AssignQueuePositions()
    {
        int index = 0;
        foreach (var npc in queue)
        {
            var npcController = npc.GetComponent<NPCController>();
            if (npcController != null)
            {
                // Assign each NPC to their respective queue point
                npcController.target = index < queuePositions.Count ? queuePositions[index] : null;
            }
            index++;
        }
    }
}
