using UnityEngine;

public class NPCController : MonoBehaviour
{
    private Transform target; // The NPC's current target queue point
    public int CurrentTargetIndex { get; private set; } // Index of the current target queue point
    private QueueManager queueManager; // Reference to the QueueManager
    public float moveSpeed = 3f; // Movement speed
    public float stoppingDistance = 0.1f; // Distance to stop moving

    private bool isMovingToNextPoint = false;

    public void SetTarget(Transform newTarget, int index, QueueManager manager)
    {
        target = newTarget;
        CurrentTargetIndex = index; // Track the index of the target queue point
        queueManager = manager;
        isMovingToNextPoint = true;
    }

    private void Update()
    {
        if (target != null && isMovingToNextPoint)
        {
            // Move toward the target position
            transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

            // Face the target
            Vector3 direction = (target.position - transform.position).normalized;
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(direction);
            }

            // Check if the NPC has reached its current target
            if (Vector3.Distance(transform.position, target.position) <= stoppingDistance)
            {
                isMovingToNextPoint = false; // Stop moving
                queueManager.MoveNPCToNextPoint(gameObject, CurrentTargetIndex); // Check if the NPC can move closer
            }
        }
    }
}
