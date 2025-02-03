using UnityEngine;

public class Bun : MonoBehaviour
{
    public Transform Socket; // The location where the sausage should be inserted
    private CookSausage insertedSausage; // Reference to the sausage inside the bun
    public GameObject Hotdog; // The final hotdog prefab to spawn if conditions are met


    private void OnTriggerEnter(Collider other)
    {
        // Check if a sausage is inserted
        CookSausage sausage = other.GetComponent<CookSausage>();
        if (sausage != null && insertedSausage == null) // Only one sausage allowed
        {
            insertedSausage = sausage;

            CheckForHotdog();
        }
    }

    private void CheckForHotdog()
    {
        if (insertedSausage != null && insertedSausage.state == CookSausage.SausageState.Cooked)
        {
            // Create a combined hotdog
            GameObject newHotdog = Instantiate(Hotdog, transform.position, transform.rotation);

            // Destroy old bun and sausage objects
            Destroy(insertedSausage.gameObject);
            Destroy(gameObject);
        }
    }
}