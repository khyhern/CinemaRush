using UnityEngine;

public class CupDetector : MonoBehaviour
{
    private bool isCupInSocket = false;  // Track if NEWSodaCup is in the socket

    public bool IsCupInSocket() => isCupInSocket; // Function to return true/false

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("NEWSodaCup"))   // Checks if the name contains "NEWSodaCup"
        {
            {
                isCupInSocket = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name.Contains("NEWSodaCup"))
        {
            isCupInSocket = false;
        }
    }
}
