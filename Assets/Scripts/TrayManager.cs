using System.Collections.Generic;
using UnityEngine;

public class TrayManager : MonoBehaviour
{
    public List<string> trayItems = new List<string>(); // Stores names of finalized food items
    public Color gizmoColor = new Color(1, 0, 0, 0.3f); // Semi-transparent red for visualization

    // Method to be called when submitting the order
    public void ScanTrayForFinalItems()
    {
        trayItems.Clear(); // Reset the list before scanning
        Collider[] foodColliders = Physics.OverlapBox(transform.position, transform.localScale / 2, Quaternion.identity);

        foreach (Collider collider in foodColliders)
        {
            if (collider.CompareTag("Food"))
            {
                FoodItem foodItem = collider.GetComponent<FoodItem>();

                if (foodItem != null)
                {
                    string foodName = foodItem.foodName;

                    if (!trayItems.Contains(foodName))
                    {
                        trayItems.Add(foodName);
                        Debug.Log("Finalized Tray Item: " + foodName);
                    }
                }
            }
        }
    }

    // Method to delete all food objects in the tray
    public void ClearTray()
    {
        Collider[] foodColliders = Physics.OverlapBox(transform.position, transform.localScale / 2, Quaternion.identity);

        foreach (Collider collider in foodColliders)
        {
            if (collider.CompareTag("Food"))
            {
                Debug.Log("Removing: " + collider.gameObject.name);
                Destroy(collider.gameObject); // Destroy all food items
            }
        }

        trayItems.Clear(); // Clear the stored food list
    }

    // Method to get the finalized tray contents
    public List<string> GetFinalizedTrayItems()
    {
        return new List<string>(trayItems); // Return a copy of the list
    }

    // **Draw the Overlap Box in Scene View**
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = gizmoColor; // Set color to semi-transparent red
        Gizmos.matrix = Matrix4x4.TRS(transform.position, Quaternion.identity, transform.localScale); // Match transform
        Gizmos.DrawCube(Vector3.zero, Vector3.one); // Draw cube at object's position
    }
}
