using UnityEngine;

public class CookSausage : MonoBehaviour
{
    private bool isInPan = false; // Tracks if the sausage is in the frying pan
    private float cookTime = 0f; // Timer to track cooking time
    private bool isCooked = false; // Tracks if sausage is cooked
    private bool isBurnt = false; // Tracks if sausage is burnt

    [SerializeField] private Material cookedMaterial; // Reference to the cooked sausage material
    [SerializeField] private Material burntMaterial;  // Reference to the burnt sausage material
    [SerializeField] private float cookDuration = 5f; // Time needed to cook
    [SerializeField] private float burnDuration = 10f; // Time after which the sausage burns

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the sausage collides with the frying pan
        if (collision.collider.CompareTag("FryingPan") && !isBurnt)
        {
            isInPan = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // Check if the sausage exits collision with the frying pan
        if (collision.collider.CompareTag("FryingPan"))
        {
            isInPan = false;
            cookTime = 0f; // Reset the timer if it leaves the pan
        }
    }

    private void Update()
    {
        if (isInPan && !isBurnt)
        {
            cookTime += Time.deltaTime; // Increment cooking time

            // If the sausage is not yet cooked and reaches the cookDuration
            if (!isCooked && cookTime >= cookDuration)
            {
                CookSausageFully();
            }

            // If the sausage is cooked and exceeds the burnDuration
            if (cookTime >= burnDuration)
            {
                BurnSausage();
            }
        }
    }

    private void CookSausageFully()
    {
        if (!isCooked && !isBurnt) // Only cook if not burnt
        {
            isCooked = true;
            Debug.Log("Sausage is cooked!");

            // Change the sausage's material to the cooked material
            if (cookedMaterial != null)
            {
                GetComponent<Renderer>().material = cookedMaterial;
            }
            else
            {
                Debug.LogWarning("Cooked material is not assigned!");
            }
        }
    }

    private void BurnSausage()
    {
        isBurnt = true; // Sausage is now burnt
        isCooked = false; // Mark as no longer "just cooked"
        Debug.Log("Sausage is burnt!");

        // Change the sausage's material to the burnt material
        if (burntMaterial != null)
        {
            GetComponent<Renderer>().material = burntMaterial;
        }
        else
        {
            Debug.LogWarning("Burnt material is not assigned!");
        }
    }
}
