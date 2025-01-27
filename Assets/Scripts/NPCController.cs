using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    private Queue<Transform> waypoints = new Queue<Transform>(); // Queue of waypoints for L-shaped movement
    public Transform target; // Final queue position
    public float moveSpeed = 3f; // Speed of movement
    public float stoppingDistance = 0.1f; // Distance to stop moving

    private Transform currentWaypoint; // The current waypoint the NPC is moving toward

    private void Update()
    {
        if (currentWaypoint != null)
        {
            // Move toward the current waypoint
            MoveTo(currentWaypoint);

            // If reached the waypoint, move to the next one
            if (Vector3.Distance(transform.position, currentWaypoint.position) <= stoppingDistance)
            {
                currentWaypoint = waypoints.Count > 0 ? waypoints.Dequeue() : target;
            }
        }
        else if (target != null)
        {
            // Move toward the final target (queue position)
            MoveTo(target);
        }
    }

    public void SetWaypoints(List<Transform> waypointList)
    {
        // Add waypoints to the queue
        waypoints = new Queue<Transform>(waypointList);
        currentWaypoint = waypoints.Count > 0 ? waypoints.Dequeue() : target; // Start with the first waypoint
    }

    public void SetFinalTarget(Transform finalTarget)
    {
        target = finalTarget;
    }

    private void MoveTo(Transform destination)
    {
        // Move towards the destination
        transform.position = Vector3.MoveTowards(transform.position, destination.position, moveSpeed * Time.deltaTime);

        // Rotate to face the destination
        Vector3 direction = (destination.position - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }
}
