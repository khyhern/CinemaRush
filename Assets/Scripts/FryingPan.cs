using System.Collections.Generic;
using UnityEngine;

public class FryingPan : MonoBehaviour
{
    public bool IsBurnt { get; private set; } = false; // Tracks if the pan is burnt
    private Dictionary<GameObject, float> burntSausageTimers = new Dictionary<GameObject, float>(); // Tracks timers for each burnt sausage

    [SerializeField] private float timeToBurnPan = 5f; // Time for the pan to burn
    [SerializeField] private Material burntPanMaterial; // Material to indicate the pan is burnt
    [SerializeField] private Material originalPanMaterial; // Material for the clean (non-burnt) pan

    private Renderer panRenderer;

    private void Start()
    {
        panRenderer = GetComponent<Renderer>();
        if (panRenderer == null)
        {
            Debug.LogError("Renderer not found on frying pan!");
        }
    }

    private void Update()
    {
        if (IsBurnt) return; // If the pan is already burnt, no need to track timers

        List<GameObject> keys = new List<GameObject>(burntSausageTimers.Keys);

        foreach (GameObject sausage in keys)
        {
            if (sausage == null)
            {
                burntSausageTimers.Remove(sausage);
                continue;
            }

            burntSausageTimers[sausage] += Time.deltaTime;

            if (burntSausageTimers[sausage] >= timeToBurnPan)
            {
                BurnPan();
                break;
            }
        }
    }

    public void AddBurntSausage(GameObject sausage)
    {
        if (!IsBurnt && !burntSausageTimers.ContainsKey(sausage))
        {
            burntSausageTimers.Add(sausage, 0f);
        }
    }

    public void RemoveSausage(GameObject sausage)
    {
        if (burntSausageTimers.ContainsKey(sausage))
        {
            burntSausageTimers.Remove(sausage);
        }
    }

    private void BurnPan()
    {
        IsBurnt = true;
        Debug.Log("The pan is now burnt!");

        if (burntPanMaterial != null)
        {
            panRenderer.material = burntPanMaterial;
        }
        else
        {
            Debug.LogWarning("Burnt pan material is not assigned!");
        }

        burntSausageTimers.Clear();
    }

    public void ResetPan()
    {
        IsBurnt = false;
        Debug.Log("The pan has been reset!");

        if (originalPanMaterial != null)
        {
            panRenderer.material = originalPanMaterial;
        }
        else
        {
            Debug.LogWarning("Original pan material is not assigned!");
        }
    }
}
