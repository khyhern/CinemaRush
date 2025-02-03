using UnityEngine;

public class CashierTrigger : MonoBehaviour
{
    private NPCController currentNPC;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            currentNPC = other.GetComponent<NPCController>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            currentNPC = null;
        }
    }

    public NPCController GetCurrentNPC()
    {
        return currentNPC;
    }
}
