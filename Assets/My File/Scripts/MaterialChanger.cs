using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialChangeOnContact : MonoBehaviour
{
    [Header("Material Settings")]
    public Material targetMaterial;         // The material to apply when an object is revealed
    public string objectNamePrefix = "Fill_"; // Naming prefix (e.g., "Fill_1", "Fill_2", etc.)
    public int objectCount = 10;            // Total number of objects to reveal

    private Collider detectorCollider;      // Reference to this GameObject's collider (should be a trigger)
    public int sodaLayer;                  // Layer index for the "Soda" layer
    public int sodaFillLayer;              // Layer index for the "SodaFill" layer

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
                StartCoroutine(RevealObjectsWithDelay());
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

    private IEnumerator RevealObjectsWithDelay()
    {
        // Loop through the expected objects (Fill_1, Fill_2, etc.)
        for (int i = 1; i <= objectCount; i++)
        {
            string objectName = $"{objectNamePrefix}{i}"; // e.g., "Fill_1"
            GameObject obj = GameObject.Find(objectName);

            if (obj != null)
            {
                Renderer objectRenderer = obj.GetComponent<Renderer>();
                if (objectRenderer != null)
                {
                    // Make the object visible
                    objectRenderer.enabled = true;

                    // Optionally, change its material to the target material
                    if (targetMaterial != null)
                    {
                        objectRenderer.material = targetMaterial;
                    }

                    Debug.Log($"Revealed {obj.name}.");
                }
                else
                {
                    Debug.LogWarning($"Object {objectName} does not have a Renderer component.");
                }
            }
            else
            {
                Debug.LogWarning($"Object {objectName} not found!");
            }

            // Wait for 0.7 seconds before revealing the next object
            yield return new WaitForSeconds(0.7f);
        }
    }
}



/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialChangeOnContact : MonoBehaviour
{
    [Header("Material Settings")]
    public Material targetMaterial;         // The material to apply when an object is revealed
    public string objectNamePrefix = "Fill_"; // Naming prefix (e.g., "Fill_1", "Fill_2", etc.)
    public int objectCount = 10;            // Total number of objects to reveal

    private Collider detectorCollider;      // Reference to this GameObject's collider (should be a trigger)
    private int sodaLayer;                  // Layer index for the "Soda" layer

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
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object that entered is on the "Soda" layer
        if (other.gameObject.layer == sodaLayer)
        {
            Debug.Log($"Object {other.name} entered the trigger and is on the 'Soda' layer.");
            // Start the coroutine to reveal objects based on naming convention
            StartCoroutine(RevealObjectsWithDelay());
        }
    }

    private IEnumerator RevealObjectsWithDelay()
    {
        // Loop through the expected objects (Fill_1, Fill_2, etc.)
        for (int i = 1; i <= objectCount; i++)
        {
            string objectName = $"{objectNamePrefix}{i}"; // e.g., "Fill_1"
            GameObject obj = GameObject.Find(objectName);

            if (obj != null)
            {
                Renderer objectRenderer = obj.GetComponent<Renderer>();
                if (objectRenderer != null)
                {
                    // Make the object visible
                    objectRenderer.enabled = true;

                    // Optionally, change its material to the target material
                    if (targetMaterial != null)
                    {
                        objectRenderer.material = targetMaterial;
                    }

                    Debug.Log($"Revealed {obj.name}.");
                }
                else
                {
                    Debug.LogWarning($"Object {objectName} does not have a Renderer component.");
                }
            }
            else
            {
                Debug.LogWarning($"Object {objectName} not found!");
            }

            // Wait for 0.7 seconds before revealing the next object
            yield return new WaitForSeconds(0.7f);
        }
    }
}*/




