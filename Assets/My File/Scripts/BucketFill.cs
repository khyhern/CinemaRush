using UnityEngine;

public class BucketFill : MonoBehaviour
{
    private bool firstFillComplete = false; // Tracks if bucket_fill_mesh1 has been filled
    public AudioSource fillSoundEffect;      // Sound to play AFTER the effect ends

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object is named "ScoopCollider"
        if (other.gameObject.name == "ScoopCollider")
        {
            Debug.Log($"FillDetector collided with {other.name}");

            // Find 'scoop_fill_mesh' under 'ScoopCollider'
            Transform scoopFillMesh = other.transform.Find("scoop_fill_mesh");

            if (scoopFillMesh != null)
            {
                Renderer scoopRenderer = scoopFillMesh.GetComponent<Renderer>();

                if (scoopRenderer != null && scoopRenderer.enabled)
                {
                    Debug.Log("'scoop_fill_mesh' is ON. Proceeding with fill process.");

                    if (!firstFillComplete)
                    {
                        FillBucket("bucket_fill_mesh1");  // Fill first layer
                        firstFillComplete = true;
                        fillSoundEffect.Play();
                    }
                    else
                    {
                        FillBucket("bucket_fill_mesh2");  // Fill second layer
                        fillSoundEffect.Play();
                        UpdatePopcornBucket(); // Only update this specific bucket
                    }

                    // Disable scoop_fill_mesh after transferring
                    scoopRenderer.enabled = false;
                    Debug.Log("'scoop_fill_mesh' is now OFF.");
                }
                else
                {
                    Debug.Log("'scoop_fill_mesh' is OFF. No action taken.");
                }
            }
            else
            {
                Debug.LogWarning("'scoop_fill_mesh' not found under ScoopCollider.");
            }
        }
    }

    private void FillBucket(string bucketMeshName)
    {
        // Find the bucket fill mesh inside THIS specific popcorn bucket instance
        Transform bucketFillMesh = FindChildRecursively(transform, bucketMeshName);

        if (bucketFillMesh != null)
        {
            Renderer bucketRenderer = bucketFillMesh.GetComponent<Renderer>();

            if (bucketRenderer != null)
            {
                bucketRenderer.enabled = true;
                Debug.Log($"'{bucketMeshName}' is now ON in {gameObject.name}.");
            }
        }
        else
        {
            Debug.LogWarning($"'{bucketMeshName}' not found in {gameObject.name}.");
        }
    }

    private void UpdatePopcornBucket()
    {
        // Find the top-level parent (PopcornBucket)
        Transform rootBucket = transform;
        while (rootBucket.parent != null)
        {
            rootBucket = rootBucket.parent; // Keep going up until we reach the root
        }

        Debug.Log($"Root bucket found: {rootBucket.name}");

        // Ensure this is a PopcornBucket instance
        if (rootBucket.name.StartsWith("PopcornBucket"))
        {
            rootBucket.tag = "Untagged"; // Reset to force Unity to recognize the change
            rootBucket.tag = "Food";

            // Change the tag of all children
            foreach (Transform child in rootBucket)
            {
                child.tag = "Untagged";
                child.tag = "Food";
            }

            // Ensure FoodItem script exists on the PopcornBucket
            FoodItem foodItem = rootBucket.GetComponent<FoodItem>();
            if (foodItem != null)
            {
                foodItem.ChangeFoodName("Popcorn");
                Debug.Log($"{rootBucket.name} and its children are now tagged as 'Food', and foodName is set to 'Popcorn'.");
            }
            else
            {
                Debug.LogWarning($"{rootBucket.name} does not have a FoodItem script attached!");
            }
        }
        else
        {
            Debug.LogWarning($"Root object is not a PopcornBucket! Found: {rootBucket.name}");
        }
    }


    // Recursive function to find a child by name inside the object
    private Transform FindChildRecursively(Transform parent, string childName)
    {
        foreach (Transform child in parent)
        {
            if (child.name == childName)
            {
                return child;
            }
            Transform found = FindChildRecursively(child, childName);
            if (found != null)
            {
                return found;
            }
        }
        return null;
    }
}


/*private void UpdatePopcornBucket(GameObject popcornBucket)
    {
        // Change the tag of PopcornBucket itself
        popcornBucket.tag = "Food";

        // Change the tag of all its children
        foreach (Transform child in popcornBucket.transform)
        {
            child.tag = "Food";
        }

        // Update the FoodItem script on PopcornBucket
        FoodItem foodItem = popcornBucket.GetComponent<FoodItem>();
        if (foodItem != null)
        {
            foodItem.ChangeFoodName("Popcorn");
            Debug.Log($"PopcornBucket and its children are now tagged as 'Food', and foodName is set to 'Popcorn'.");
        }
        else
        {
            Debug.LogWarning("'PopcornBucket' does not have a FoodItem script.");
        }
*/