using UnityEngine;

public class FireExtinguisher : MonoBehaviour
{
    [SerializeField] private ParticleSystem extinguishEffect; // Optional particle effect for the extinguisher
    [SerializeField] private Transform raycastPoint; // The point where the raycast starts (e.g., an empty GameObject)
    [SerializeField] private float range = 5f; // Range of the raycast
    [SerializeField] private Color rayColor = Color.red; // Color of the debug ray

    /// <summary>
    /// Activates the fire extinguisher.
    /// Call this function to use the extinguisher and attempt to reset a frying pan.
    /// </summary>
    public void UseExtinguisher()
    {
        Debug.Log("Fire extinguisher activated!");

        // Ensure the raycast point is assigned
        if (raycastPoint == null)
        {
            Debug.LogError("Raycast point is not assigned!");
            return;
        }

        // Perform a raycast from the raycast point forward
        if (Physics.Raycast(raycastPoint.position, raycastPoint.forward, out RaycastHit hit, range))
        {
            // Visualize the ray as a debug line in the scene view
            Debug.DrawRay(raycastPoint.position, raycastPoint.forward * hit.distance, rayColor, 1f);

            // Check if the object hit has the "FryingPan" tag
            if (hit.collider.CompareTag("FryingPan"))
            {
                FryingPan fryingPan = hit.collider.GetComponent<FryingPan>();
                if (fryingPan != null && fryingPan.IsBurnt)
                {
                    fryingPan.ResetPan(); // Reset the frying pan
                    Debug.Log("Frying pan has been reset!");

                    // Play extinguish particle effect, if assigned
                    if (extinguishEffect != null)
                    {
                        extinguishEffect.Play();
                    }
                }
                else
                {
                    Debug.Log("The frying pan is not burnt.");
                }
            }
            else
            {
                Debug.Log("The object hit by the ray is not a frying pan.");
            }
        }
        else
        {
            // Visualize the ray as a debug line in the scene view when it doesn't hit anything
            Debug.DrawRay(raycastPoint.position, raycastPoint.forward * range, rayColor, 1f);
            Debug.Log("No object detected within range.");
        }
    }
}
