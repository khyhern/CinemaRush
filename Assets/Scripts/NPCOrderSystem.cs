using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class NPCOrderSystem : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMesh NPCorderText; // Reference to a 3D TextMesh object

    private string npcOrder;
    private void GenerateRandomOrder()
    {
        string[] menuItems = { "Hotdog", "Popcorn" };
        string[] sodaFlavors = { "Green Soda", "Orange Soda", "Purple Soda" };

        // Randomly pick a main item
        string mainItem = menuItems[Random.Range(0, menuItems.Length)];

        // 50% chance to include a soda in the order
        bool includeSoda = Random.value > 0.5f;
        string soda = includeSoda ? sodaFlavors[Random.Range(0, sodaFlavors.Length)] : "";

        // Construct order
        npcOrder = includeSoda ? $"{mainItem} + {soda}" : mainItem;

        Debug.Log($"NPC Order: {npcOrder}");

        // Update 3D TextMesh if the NPC is in the first position
        if (NPCorderText != null)
        {
            NPCorderText.text = npcOrder;
        }
        else
        {
            Debug.LogError("OrderText is not assigned in the Inspector.");
        }
    }
}
