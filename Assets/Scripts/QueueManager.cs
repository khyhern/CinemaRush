using System.Collections.Generic;
using UnityEngine;

public class QueueManager : MonoBehaviour
{
    public Transform spawnPoint; // Where NPCs spawn
    public List<Transform> queuePositions; // Queue points (QueuePoint1, QueuePoint2, ..., QueuePoint7)
    public GameObject npcPrefab; // Prefab for NPCs
    public float spawnInterval = 5f; // Time between NPC spawns

    private List<bool> occupiedPoints; // Tracks which queue points are occupied
    private List<GameObject> activeNPCs; // List of all NPCs in the queue

    private void Start()
    {
        // Initialize the occupancy list and NPC tracking
        occupiedPoints = new List<bool>(new bool[queuePositions.Count]);
        activeNPCs = new List<GameObject>();

        // Start spawning NPCs at intervals
        InvokeRepeating(nameof(SpawnNPC), 0f, spawnInterval);
    }

    private void SpawnNPC()
    {
        // Check if the last queue point is open before spawning
        int lastIndex = queuePositions.Count - 1;

        if (!occupiedPoints[lastIndex]) // If the last queue point is NOT occupied
        {
            if (npcPrefab == null || spawnPoint == null || queuePositions.Count == 0) return;

            // Spawn an NPC at the spawn point
            GameObject npc = Instantiate(npcPrefab, spawnPoint.position, Quaternion.identity);

            // Assign the NPC to the last queue point
            npc.GetComponent<NPCController>().SetTarget(queuePositions[lastIndex], lastIndex, this);

            // Mark the last queue point as occupied
            occupiedPoints[lastIndex] = true;
            activeNPCs.Add(npc); // Track the NPC
        }
        else
        {
            Debug.Log("Last queue point is occupied. Cannot spawn a new NPC.");
        }
    }

    public void MoveNPCToNextPoint(GameObject npc, int currentIndex)
    {
        if (currentIndex > 0) // Ensure the NPC isn't already at QueuePoint1
        {
            int nextIndex = currentIndex - 1;

            if (!occupiedPoints[nextIndex]) // Check if the next point is free
            {
                // Update the NPC's target to the next point
                npc.GetComponent<NPCController>().SetTarget(queuePositions[nextIndex], nextIndex, this);

                // Update the occupancy status
                occupiedPoints[currentIndex] = false;
                occupiedPoints[nextIndex] = true;
            }
        }
    }

    public void ServeNPC(GameObject npc, int currentIndex)
    {
        if (currentIndex == 0) // NPC is at QueuePoint1 (ready to be served)
        {
            // Remove the NPC from the game
            Destroy(npc);
            occupiedPoints[currentIndex] = false; // Mark QueuePoint1 as available
            activeNPCs.Remove(npc); // Remove the NPC from the active list

            // Move all remaining NPCs forward
            UpdateQueue();
        }
    }

    private void UpdateQueue()
    {
        for (int i = 1; i < queuePositions.Count; i++) // Start from QueuePoint2
        {
            if (occupiedPoints[i] && !occupiedPoints[i - 1]) // If the current point is occupied, and the next is free
            {
                GameObject npc = activeNPCs.Find(n => n.GetComponent<NPCController>().CurrentTargetIndex == i);
                if (npc != null)
                {
                    MoveNPCToNextPoint(npc, i);
                }
            }
        }
    }
}
