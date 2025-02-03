using UnityEngine;

public class OrderCheckButton : MonoBehaviour
{
    public CashierTrigger cashierTrigger;  // Reference to the Cashier trigger

    // Method to check the NPC order when the button is pressed
    public void CheckNPCOrder()
    {
        if (cashierTrigger == null)
        {
            Debug.LogError("CashierTrigger reference is missing!");
            return;
        }

        // Get the NPC currently in the cashier trigger
        NPCController currentNPC = cashierTrigger.GetCurrentNPC();

        if (currentNPC != null)
        {
            string npcOrder = currentNPC.npcOrder; // Retrieve the NPC's order
            Debug.Log("NPC Order: " + npcOrder);
        }
        else
        {
            Debug.Log("No NPC at the Cashier.");
        }
    }
}
