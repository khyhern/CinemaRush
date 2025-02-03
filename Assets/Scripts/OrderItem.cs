using UnityEngine;

public class OrderItem : MonoBehaviour
{
    [Header("Order Item Properties")]
    public string itemName; // The name of the item (e.g., "Hotdog", "Popcorn", "Green Soda")

    private void Start()
    {
        if (string.IsNullOrEmpty(itemName))
        {
            Debug.LogError($"OrderItem on {gameObject.name} is missing an itemName!");
        }
    }
}