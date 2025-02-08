using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit; // Import XR Interaction Toolkit

public class FillCup : MonoBehaviour
{
    public Transform liquidSocket; // The socket for the liquid effect
    public Transform lockSocket; // The socket where the cup is locked
    public ParticleSystem liquidEffect; // Particle system for the liquid effect
    public float fillDuration = 7f; // Time to fill the cup
    public string cupTag = "Cups"; // Tag used to identify the cup

    private bool isFilling = false;
    private GameObject currentCup; // The cup currently being filled

    public void StartFilling()
    {
        if (!isFilling && IsCupOnSocket())
        {
            isFilling = true;
            StartCoroutine(FillProcess());
        }
        else
        {
            Debug.Log("No cup detected or filling already in progress.");
        }
    }

    private bool IsCupOnSocket()
    {
        // Check if there's an object with the correct tag in the lock socket
        Collider[] hitColliders = Physics.OverlapSphere(lockSocket.position, 0.1f); // Adjust radius if needed
        foreach (var collider in hitColliders)
        {
            if (collider.CompareTag(cupTag))
            {
                currentCup = collider.gameObject;
                return true; // Cup detected
            }
        }
        return false; // No cup detected
    }

    private IEnumerator FillProcess()
    {
        // Lock the cup in place
        if (currentCup != null)
        {
            // Disable physics
            Rigidbody cupRigidbody = currentCup.GetComponent<Rigidbody>();
            if (cupRigidbody != null)
            {
                cupRigidbody.isKinematic = true;
            }

            // Disable grabbing
            XRGrabInteractable grabComponent = currentCup.GetComponent<XRGrabInteractable>();
            if (grabComponent != null)
            {
                grabComponent.enabled = false; // Lock the cup in place
            }

            // Align the cup to the lock socket
            currentCup.transform.position = lockSocket.position;
            currentCup.transform.rotation = lockSocket.rotation;
        }

        // Start the liquid effect
        if (liquidEffect != null && liquidSocket != null)
        {
            liquidEffect.transform.position = liquidSocket.position;
            liquidEffect.Play();
        }

        // Wait for the fill duration
        yield return new WaitForSeconds(fillDuration);

        // Stop the liquid effect
        if (liquidEffect != null)
        {
            liquidEffect.Stop();
        }

        // Unlock the cup
        if (currentCup != null)
        {
            // Enable physics
            Rigidbody cupRigidbody = currentCup.GetComponent<Rigidbody>();
            if (cupRigidbody != null)
            {
                cupRigidbody.isKinematic = false;
            }

            // Re-enable grabbing
            XRGrabInteractable grabComponent = currentCup.GetComponent<XRGrabInteractable>();
            if (grabComponent != null)
            {
                grabComponent.enabled = true; // Unlock the cup
            }
        }

        isFilling = false;
    }

    private void OnDrawGizmos()
    {
        // Optional: Visualize the lock socket detection radius in the editor
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(lockSocket.position, 0.1f); // Matches the OverlapSphere radius
    }
}
