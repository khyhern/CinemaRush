using UnityEngine;

public class BucketFill : MonoBehaviour
{
    private bool firstFillComplete = false; // Tracks if bucket_fill_mesh1 has been filled

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object is named "ScoopCollider"
        if (other.gameObject.name == "ScoopCollider")
        {
            SoundManager.Instance.PlayOneShot("popcorn fill");
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
                        FillBucket("bucket_fill_mesh1");
                        firstFillComplete = true; // Mark first fill complete
                    }
                    else
                    {
                        FillBucket("bucket_fill_mesh2");

                        // Find the 'PopcornBucket' in the scene
                        GameObject popcornBucket = GameObject.Find("PopcornBucket");

                        if (popcornBucket != null && popcornBucket.layer == LayerMask.NameToLayer("Popcorn"))
                        {
                            UpdatePopcornBucket(popcornBucket);
                        }
                        else
                        {
                            Debug.LogWarning("'PopcornBucket' not found or not on the 'Popcorn' layer.");
                        }
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
        // Find the bucket fill mesh by name
        GameObject bucketFillMesh = GameObject.Find(bucketMeshName);

        if (bucketFillMesh != null)
        {
            Renderer bucketRenderer = bucketFillMesh.GetComponent<Renderer>();

            if (bucketRenderer != null)
            {
                bucketRenderer.enabled = true;
                Debug.Log($"'{bucketMeshName}' is now ON.");
            }
        }
        else
        {
            Debug.LogWarning($"'{bucketMeshName}' not found in the scene.");
        }
    }

    private void UpdatePopcornBucket(GameObject popcornBucket)
    {
        SoundManager.Instance.PlayOneShot("popcorn done");
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
    }
}
