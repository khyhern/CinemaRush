using UnityEngine;

public class Cashier : MonoBehaviour
{
    public QueueManager queueManager; // Reference to the QueueManager
    public float processTime = 3f; // Time to process each NPC
    private bool isProcessing = false; // Prevent multiple processes at once

    private void OnTriggerStay(Collider other)
    {
        // Check if an NPC is in the cashier zone and we’re not already processing
        if (other.CompareTag("NPC") && !isProcessing)
        {
            StartCoroutine(ProcessNPC());
        }
    }

    private System.Collections.IEnumerator ProcessNPC()
    {
        isProcessing = true;

        // Simulate processing time
        yield return new WaitForSeconds(processTime);

        // Notify the QueueManager to remove the first NPC
        queueManager.ServeNextCustomer();

        isProcessing = false;

    }
}
