using UnityEngine;
using System.Collections.Generic;

public class OrderCheckButton : MonoBehaviour
{
    public CashierTrigger cashierTrigger;  // Reference to the cashier trigger
    public TrayManager trayManager; // Reference to the tray manager

    // Method to check the NPC order when the button is pressed
    public void CheckNPCOrder()
    {
        SoundManager.Instance.PlayOneShot("btnpress");
        if (cashierTrigger == null || trayManager == null)
        {
            Debug.LogError("CashierTrigger or TrayManager reference is missing!");
            return;
        }

        // Scan the tray for items (this ensures only finalized food is checked)
        trayManager.ScanTrayForFinalItems();

        // Get the NPC currently in the cashier trigger
        NPCController currentNPC = cashierTrigger.GetCurrentNPC();

        if (currentNPC != null)
        {
            Debug.Log($"Checking order for NPC: {currentNPC.name}");

            string npcOrderString = currentNPC.npcOrder; // Retrieve the NPC's order as a string
            List<string> npcOrderList = new List<string>(npcOrderString.Split('+'));

            // Trim any spaces in case order formatting has extra spaces
            for (int i = 0; i < npcOrderList.Count; i++)
            {
                npcOrderList[i] = npcOrderList[i].Trim();
            }

            // Get the finalized tray items
            List<string> trayItems = trayManager.GetFinalizedTrayItems();

            // Check if tray contents match NPC order
            bool orderCorrect = trayItems.Count == npcOrderList.Count && trayItems.TrueForAll(npcOrderList.Contains);

            if (orderCorrect)
            {
                Debug.Log("? Order is correct! NPC is happy.");
                currentNPC.SetOrderStatus(true); // NPC processes and leaves
                if (currentNPC.name == "MaleA(Clone)")
                {
                    SoundManager.Instance.PlayOneShot("happy customer 1");
                }
                else if (currentNPC.name == "FemaleA(Clone)")
                {
                    SoundManager.Instance.PlayOneShot("happy customer 2");
                }

                trayManager.ClearTray(); // Remove all food from the tray
            }
            else
            {
                if (currentNPC.name == "MaleA(Clone)")
                {
                    SoundManager.Instance.PlayOneShot("angry customer 1");
                }
                else if(currentNPC.name == "FemaleA(Clone)")
                {
                    SoundManager.Instance.PlayOneShot("angry customer 2");
                }
                Debug.Log("? Order is incorrect. Try again.");
            }
        }
        else
        {
            Debug.LogError("No NPC at the Cashier. Check if NPC entered the trigger.");
        }
    }
}
