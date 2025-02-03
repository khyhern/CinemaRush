using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BunSocketHandler : MonoBehaviour
{
    private FoodItem bun; // Reference to the FoodItem in the parent Bun

    private void Start()
    {
        // Get the FoodItem component from the parent Bun object
        bun = GetComponentInParent<FoodItem>();

        if (bun == null)
        {
            Debug.LogError("FoodItem component not found on the parent of the socket!");
        }
    }

    // Called when a sausage is inserted into the socket
    public void OnSausageInserted(SelectEnterEventArgs args)
    {
        GameObject insertedObject = args.interactableObject.transform.gameObject;

        if (insertedObject.CompareTag("Food")) // Ensure the inserted object is food
        {
            CookSausage sausage = insertedObject.GetComponent<CookSausage>();

            if (sausage != null && bun != null)
            {
                // Check the state of the sausage and update the bun's name
                switch (sausage.state)
                {
                    case CookSausage.SausageState.Raw:
                        bun.ChangeFoodName("Raw Hotdog");
                        Debug.Log("Raw Sausage inserted! Bun is now a Raw Hotdog.");
                        break;

                    case CookSausage.SausageState.Cooked:
                        bun.ChangeFoodName("Cooked Hotdog");
                        Debug.Log("Cooked Sausage inserted! Bun is now a Cooked Hotdog.");
                        break;

                    case CookSausage.SausageState.Burnt:
                        bun.ChangeFoodName("Burnt Hotdog");
                        Debug.Log("Burnt Sausage inserted! Bun is now a Burnt Hotdog.");
                        break;
                }
            }
        }
    }

    // Called when a sausage is removed from the socket
    public void OnSausageRemoved(SelectExitEventArgs args)
    {
        if (bun != null)
        {
            bun.ChangeFoodName("Bun"); // Reset to Bun if sausage is removed
            Debug.Log("Sausage removed! Bun is back to normal.");
        }
    }
}
