using UnityEngine;

public class CashierTrigger : MonoBehaviour
{
    private NPCController currentNPC;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            Debug.Log($"NPC {other.name} entered the cashier trigger!");
            currentNPC = other.GetComponent<NPCController>();

            if (currentNPC == null)
            {
                Debug.LogError($"NPC {other.name} is missing NPCController script!");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            Debug.Log($"NPC {other.name} exited the cashier trigger.");
            currentNPC = null;
        }
    }

    public NPCController GetCurrentNPC()
    {
        return currentNPC;
    }
}
