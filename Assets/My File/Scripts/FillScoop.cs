using System.Collections;
using UnityEngine;

public class FillScoop : MonoBehaviour
{
    [Header("Material Settings")]
    public float revealDelay = 0.2f; // Time delay between revealing each mesh

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object is named "ScoopCollider"
        if (other.gameObject.name == "ScoopCollider")
        {
            Debug.Log($"Object {other.name} entered the trigger.");

            // Start revealing the "scoop_fill_mesh" under the collider
            StartCoroutine(RevealPopcornMeshes(other.gameObject));
        }
    }

    private IEnumerator RevealPopcornMeshes(GameObject scoopCollider)
    {
        // Find the "scoop_fill_mesh" child under the "ScoopCollider"
        Transform scoopFillMeshTransform = scoopCollider.transform.Find("scoop_fill_mesh");

        if (scoopFillMeshTransform != null)
        {
            SoundManager.Instance.PlayOneShot("popcorn fill");
            Renderer renderer = scoopFillMeshTransform.GetComponent<Renderer>();

            if (renderer != null)
            {
                // Enable the mesh renderer
                renderer.enabled = true;
                Debug.Log($"Revealed {scoopFillMeshTransform.name}");
            }
            else
            {
                Debug.LogWarning($"Object {scoopFillMeshTransform.name} does not have a Renderer component.");
            }
        }
        else
        {
            Debug.LogWarning("No child named 'scoop_fill_mesh' found under 'ScoopCollider'.");
        }

        // Optional: You can add a delay or other logic here if necessary
        yield return new WaitForSeconds(revealDelay);
    }
}
