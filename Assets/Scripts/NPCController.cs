using UnityEngine;

public class NPCController : MonoBehaviour
{
    public Transform target; // The NPC's assigned queue position
    public float moveSpeed = 3f; // Speed of movement
    public float stoppingDistance = 0.1f; // Distance to stop moving

    private void Update()
    {
        if (target != null)
        {
            // Move towards the target position
            transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

            // Face the target
            Vector3 direction = (target.position - transform.position).normalized;
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(direction);
            }

            // Stop moving if close enough
            if (Vector3.Distance(transform.position, target.position) <= stoppingDistance)
            {
                target = null; // Stop movement
            }
        }
    }
}
