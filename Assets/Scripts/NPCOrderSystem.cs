using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCOrderSystem : MonoBehaviour
{
    public string trayTag = "Tray";
    private Transform Tray;
    private string order; // Store NPC order

    [Header("UI Elements")]
    public string buttonTag = "Button";
    private Button checkOrderButton; // Changed to Button type

    private NPCController npcController; // Reference to the NPCController

    private void Start()
    {
        npcController = FindObjectOfType<NPCController>(); // Find the NPCController in the scene
        if (npcController == null)
        {
            Debug.LogError("NPCController not found in the scene!");
        }

        // Find the tray
        GameObject trayObject = GameObject.FindGameObjectWithTag(trayTag);
        if (trayObject != null)
        {
            Tray = trayObject.transform;
        }
        else
        {
            Debug.LogError("Tray object not found! Make sure it's tagged correctly.");
        }

        // Find the check order button
        GameObject buttonObject = GameObject.FindGameObjectWithTag(buttonTag);
        if (buttonObject != null)
        {
            checkOrderButton = buttonObject.GetComponent<Button>();
            if (checkOrderButton != null)
            {
                checkOrderButton.onClick.AddListener(OnCheckOrderButtonPressed);
            }
            else
            {
                Debug.LogError("Check Order Button does not have a Button component!");
            }
        }
        else
        {
            Debug.LogError("Button object not found! Make sure it's tagged correctly.");
        }

        // Get NPC order from NPCController
        order = npcController.npcOrder;
    }

    // This method gets triggered when the button is pressed
    public void OnCheckOrderButtonPressed()
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

    // Checks if there are items on the tray
    private bool CheckTrayForOrder()
    {
        if (Tray == null) return false;

        foreach (Transform item in Tray)
        {
            OrderItem orderItem = item.GetComponent<OrderItem>();
            if (orderItem != null)
            {
                Debug.Log($"Order found on tray: {orderItem.itemName}");
                return true;
            }
        }

        return false;
    }

    // Validates the order
    private void ValidateOrder()
    {
        if (Tray == null) return;

        string trayOrder = "";

        foreach (Transform item in Tray)
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