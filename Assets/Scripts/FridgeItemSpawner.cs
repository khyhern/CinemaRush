using UnityEngine;

public class FridgeItemSpawner : MonoBehaviour
{
    public GameObject itemPrefab; // The primary item to instantiate
    public Transform spawnPoint; // The primary spawn location inside the fridge

    public GameObject additionalItemPrefab; // The second item to instantiate
    public Transform additionalSpawnPoint; // The second spawn location

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FridgeTopDoor")) // Ensure the door has the correct tag
        {
            // Spawn the first item
            if (itemPrefab != null && spawnPoint != null)
            {
                Instantiate(itemPrefab, spawnPoint.position, spawnPoint.rotation);
            }
            else
            {
                Debug.LogError("Primary itemPrefab or spawnPoint is missing!");
            }

            // Spawn the second item at a different location
            if (additionalItemPrefab != null && additionalSpawnPoint != null)
            {
                Instantiate(additionalItemPrefab, additionalSpawnPoint.position, additionalSpawnPoint.rotation);
            }
            else
            {
                Debug.LogError("Additional itemPrefab or additionalSpawnPoint is missing!");
            }
        }
    }
}
