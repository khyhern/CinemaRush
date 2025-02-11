using System.Collections;
using UnityEngine;

public class MaterialChangeOnContact : MonoBehaviour
{
    [Header("Material Settings")]
    public Material targetMaterial;         // The material to apply when an object is revealed
    public string objectNamePrefix = "Fill_"; // Naming prefix (e.g., "Fill_1", "Fill_2", etc.)
    public int objectCount = 10;            // Total number of objects to reveal

    private Collider detectorCollider;      // Reference to this GameObject's collider (should be a trigger)
    public int sodaLayer;                   // Layer index for the "Soda" layer
    public int sodaFillLayer;               // Layer index for the "SodaFill" layer

    public string changeSodaName;

    private void Start()
    {
        // Get the collider attached to this GameObject
        detectorCollider = GetComponent<Collider>();
        if (detectorCollider == null)
        {
            Debug.LogError("No Collider found! Please attach a Collider component to this GameObject.");
        }
        else if (!detectorCollider.isTrigger)
        {
            Debug.LogWarning("The Collider is not set as a Trigger. Setting it as a Trigger automatically.");
            detectorCollider.isTrigger = true;
        }

        // Get the layer index for "Soda"
        sodaLayer = LayerMask.NameToLayer("Soda");
        if (sodaLayer == -1)
        {
            Debug.LogError("Layer 'Soda' not found! Make sure you have created a layer with this name in Unity.");
        }

        // Get the layer index for "SodaFill"
        sodaFillLayer = LayerMask.NameToLayer("SodaFill");
        if (sodaFillLayer == -1)
        {
            Debug.LogWarning("Layer 'SodaFill' not found! Only 'Soda' layer will be considered.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object that entered is on the "Soda" or "SodaFill" layer
        if (other.gameObject.layer == sodaLayer || other.gameObject.layer == sodaFillLayer)
        {
            Debug.Log($"Object {other.name} entered the trigger and is on 'Soda' or 'SodaFill' layer.");

            // Process the object (set tag & update FoodItem name)
            ProcessFoodItem(other.gameObject);

            // If the object is from the "Soda" layer, start revealing objects
            if (other.gameObject.layer == sodaLayer)
            {
                StartCoroutine(RevealObjectsWithDelay(other.transform));
            }
        }
    }

    private void ProcessFoodItem(GameObject obj)
    {
        // Set the object's tag to "Food"
        obj.tag = "Food";

        // Try to get the FoodItem component
        FoodItem foodItem = obj.GetComponent<FoodItem>();
        if (foodItem != null)
        {
            foodItem.ChangeFoodName(changeSodaName);
        }
        else
        {
            Debug.LogWarning($"FoodItem component not found on {obj.name}.");
        }
    }

    private IEnumerator RevealObjectsWithDelay(Transform sodaCupTransform)
    {
        // Find the Soda_FillMesh child in this specific cup
        Transform fillMeshParent = sodaCupTransform.Find("Soda_FillMesh");
        if (fillMeshParent == null)
        {
            Debug.LogWarning($"Soda_FillMesh not found in {sodaCupTransform.name}.");
            yield break;
        }

        // Track if at least one fill was revealed
        bool fillsRevealed = false;

        // Loop through the expected objects (Fill_1, Fill_2, etc.)
        for (int i = 1; i <= objectCount; i++)
        {
            string objectName = $"{objectNamePrefix}{i}"; // e.g., "Fill_1"
            Transform fillMesh = fillMeshParent.Find(objectName);

            if (fillMesh != null)
            {
                Renderer objectRenderer = fillMesh.GetComponent<Renderer>();
                if (objectRenderer != null)
                {
                    // Make the object visible
                    objectRenderer.enabled = true;

                    // Optionally, change its material to the target material
                    if (targetMaterial != null)
                    {
                        objectRenderer.material = targetMaterial;
                    }

                    // Change the object's name to "IsFilled"
                    fillMesh.name = "IsFilled";
                    fillsRevealed = true;

                    Debug.Log($"Revealed and renamed {objectName} -> IsFilled in {sodaCupTransform.name}.");
                }
                else
                {
                    Debug.LogWarning($"Object {objectName} does not have a Renderer component.");
                }
            }
            else
            {
                Debug.LogWarning($"Object {objectName} not found under {fillMeshParent.name}.");
            }

            // Wait for 0.7 seconds before revealing the next object
            yield return new WaitForSeconds(0.7f);
        }

        // If any fills were revealed, rename the parent to "IsFilled"
        if (fillsRevealed)
        {
            fillMeshParent.name = "IsFilled";
            Debug.Log($"Parent {sodaCupTransform.name} -> Renamed Soda_FillMesh to IsFilled.");
        }
    }
}




