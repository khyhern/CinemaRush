using UnityEngine;

public class CupDetector : MonoBehaviour
{
    private bool isCupInSocket = false;  // Track if NEWSodaCup is in the socket

    public bool IsCupInSocket() => isCupInSocket; // Function to return true/false

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("NEWSodaCup"))  // Check if object is filled
        {
            if (other.transform.Find("IsFilled") != null)
            {
                isCupInSocket = false;  // If "IsFilled" is found, set isCupInSocket to false
            }
            else
            {
                isCupInSocket = true; // Otherwise, set isCupInSocket to true
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
