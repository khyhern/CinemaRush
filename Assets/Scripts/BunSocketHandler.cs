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
                // Change the sausage tag to "CombinedFood" so it doesn't count separately in tray
                insertedObject.tag = "CombinedFood";

                // Check the state of the sausage and update the bun's name
                switch (sausage.state)
                {
                    case CookSausage.SausageState.Raw:
                        bun.ChangeFoodName("Raw Hotdog");
                        Debug.Log("Raw Sausage inserted! Bun is now a Raw Hotdog.");
                        break;

                    case CookSausage.SausageState.Cooked:
                        bun.ChangeFoodName("Hotdog");
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
        GameObject removedObject = args.interactableObject.transform.gameObject;

        if (removedObject.CompareTag("CombinedFood")) // Ensure we are dealing with a previously inserted sausage
        {
            removedObject.tag = "Food"; // Reset tag back to "Food"
            Debug.Log("Sausage removed from bun, tag reset to Food.");
        }

        if (bun != null)
        {
            bun.ChangeFoodName("Bun"); // Reset to Bun if sausage is removed
            Debug.Log("Sausage removed! Bun is back to normal.");
        }
    }
}
