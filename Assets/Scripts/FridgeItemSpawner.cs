using UnityEngine;

public class FridgeItemSpawner : MonoBehaviour
{
    public GameObject itemPrefab; // The prefab to instantiate
    public Transform spawnPoint; // The location inside the fridge where the prefab will be instantiated

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FridgeTopDoor")) // Ensure the door has the correct tag
        {
            Instantiate(itemPrefab, spawnPoint.position, spawnPoint.rotation);
        }
    }
}
