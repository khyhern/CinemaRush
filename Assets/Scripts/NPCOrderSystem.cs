using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCOrderSystem : MonoBehaviour
{
    public string trayTag = "Tray";
    private Transform itemCollider; // Reference to the ItemCollider inside the tray
    private string order; // Store NPC order

    private NPCController npcController; // Reference to the NPCController

    private void Start()
    {
        npcController = FindObjectOfType<NPCController>(); // Find the NPCController in the scene
        if (npcController == null)
        {
            Debug.LogError("NPCController not found in the scene!");
        }

        // Find the tray and then locate the ItemCollider inside it
        GameObject trayObject = GameObject.FindGameObjectWithTag(trayTag);
        if (trayObject != null)
        {
            Transform trayTransform = trayObject.transform;
            itemCollider = trayTransform.Find("ItemCollider");

            if (itemCollider == null)
            {
                Debug.LogError("ItemCollider not found inside the Tray! Make sure it's named correctly.");
            }
        }
        else
        {
            Debug.LogError("Tray object not found! Make sure it's tagged correctly.");
        }

        // Get NPC order from NPCController
        order = npcController.npcOrder;
    }

    // This method will be called from XR Simple Interactable (SelectEntered)
    public void CheckOrder()
    {
        if (CheckTrayForOrder())
        {
            ValidateOrder();
        }
        else
        {
            Debug.Log("No valid order found on the tray.");
        }
    }

    // Checks if there are items in the ItemCollider child of the tray
    private bool CheckTrayForOrder()
    {
        if (itemCollider == null) return false;

        foreach (Transform item in itemCollider)
        {
            OrderItem orderItem = item.GetComponent<OrderItem>();
            if (orderItem != null)
            {
                Debug.Log($"Order found in ItemCollider: {orderItem.itemName}");
                return true;
            }
        }

        return false;
    }

    // Validates the order by comparing items inside ItemCollider with the NPC order
    private void ValidateOrder()
    {
        if (itemCollider == null) return;

        string trayOrder = "";

        foreach (Transform item in itemCollider)
        {
            OrderItem orderItem = item.GetComponent<OrderItem>();
            if (orderItem != null)
            {
                trayOrder += trayOrder == "" ? orderItem.itemName : $" + {orderItem.itemName}";
            }
        }

        if (order == trayOrder)
        {
            CompleteOrder(true);
            Debug.Log("✅ Correct order! NPC is happy.");
        }
        else
        {
            CompleteOrder(false);
            Debug.Log("❌ Wrong order! NPC is angry.");
        }
    }

    public void CompleteOrder(bool isOrderCorrect)
    {
        if (npcController != null)
        {
            npcController.SetOrderStatus(isOrderCorrect);
        }
        else
        {
            Debug.LogError("NPCController reference is null!");
        }
    }
}
