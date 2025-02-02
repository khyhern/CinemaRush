using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    [Header("QueuePoints Settings")]
    public string queuePointTag = "QueuePoint";
    public string happyExitTag = "HappyExitPath";
    public string angryExitTag = "AngryExitPath";
    public string trayTag = "Tray"; // Tag to identify the tray object

    public float moveSpeed = 2f;
    public float checkDelay = 0.5f;

    private List<Transform> queuePoints = new List<Transform>();
    private List<Transform> happyExitPath = new List<Transform>();
    private List<Transform> angryExitPath = new List<Transform>();

    private int currentQueuePointIndex = 0;
    private static Dictionary<int, bool> queuePointOccupied = new Dictionary<int, bool>();

    [Header("UI Elements")]
    public TextMesh orderText;

    [Header("Spawner Reference")]
    private NPCSpawner spawner;

    private bool isOrderCompletedCorrectly = false;
    private string npcOrder;
    private Transform tray; // Reference to the tray object

    private void Start()
    {
        // Find all queue points
        GameObject[] queuePointObjects = GameObject.FindGameObjectsWithTag(queuePointTag);
        foreach (GameObject obj in queuePointObjects)
        {
            queuePoints.Add(obj.transform);
        }

        // Find happy exit points
        GameObject[] happyExitObjects = GameObject.FindGameObjectsWithTag(happyExitTag);
        foreach (GameObject obj in happyExitObjects)
        {
            happyExitPath.Add(obj.transform);
        }

        // Find angry exit points
        GameObject[] angryExitObjects = GameObject.FindGameObjectsWithTag(angryExitTag);
        foreach (GameObject obj in angryExitObjects)
        {
            angryExitPath.Add(obj.transform);
        }

        // Find the tray
        GameObject trayObject = GameObject.FindGameObjectWithTag(trayTag);
        if (trayObject != null)
        {
            tray = trayObject.transform;
        }
        else
        {
            Debug.LogError("Tray object not found! Make sure it's tagged correctly.");
        }

        queuePoints.Sort((a, b) => a.position.z.CompareTo(b.position.z));

        // Initialize queue point occupied status
        for (int i = 0; i < queuePoints.Count; i++)
        {
            if (!queuePointOccupied.ContainsKey(i))
            {
                queuePointOccupied[i] = false;
            }
        }

        StartCoroutine(MoveToQueuePoint());
    }

    private IEnumerator MoveToQueuePoint()
    {
        while (currentQueuePointIndex < queuePoints.Count)
        {
            Transform targetQueuePoint = queuePoints[currentQueuePointIndex];

            if (!queuePointOccupied[currentQueuePointIndex])
            {
                queuePointOccupied[currentQueuePointIndex] = true;

                while (Vector3.Distance(transform.position, targetQueuePoint.position) > 0.1f)
                {
                    transform.position = Vector3.MoveTowards(transform.position, targetQueuePoint.position, moveSpeed * Time.deltaTime);
                    yield return null;
                }

                transform.position = targetQueuePoint.position;

                if (currentQueuePointIndex > 0)
                {
                    queuePointOccupied[currentQueuePointIndex - 1] = false;
                }

                if (currentQueuePointIndex == queuePoints.Count - 1)
                {
                    GenerateRandomOrder();
                    StartCoroutine(WaitForOrder());
                }

                currentQueuePointIndex++;
            }

            yield return new WaitForSeconds(checkDelay);
        }
    }

    /// <summary>
    /// Waits at the tray for an order to be placed.
    /// </summary>
    private IEnumerator WaitForOrder()
    {
        Debug.Log("NPC is waiting for the order...");

        float waitTime = 60f;
        float timer = 0f;

        while (timer < waitTime)
        {
            yield return new WaitForSeconds(1f);
            timer += 1f;

            // Check if an order is placed on the tray
            if (CheckTrayForOrder())
            {
                ValidateOrder();
                yield break;
            }
        }

        // If time runs out, NPC leaves angrily
        Debug.Log("NPC waited too long! Leaving angrily.");
        LeaveQueue(false);
    }

    /// <summary>
    /// Checks if an order has been placed on the tray.
    /// </summary>
    private bool CheckTrayForOrder()
    {
        if (tray == null) return false;

        foreach (Transform item in tray)
        {
            OrderItem orderItem = item.GetComponent<OrderItem>(); // Assume there's an OrderItem script
            if (orderItem != null)
            {
                Debug.Log($"Order found on tray: {orderItem.itemName}");
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Validates the order on the tray against the NPC's order.
    /// </summary>
    private void ValidateOrder()
    {
        if (tray == null) return;

        string trayOrder = "";

        foreach (Transform item in tray)
        {
            OrderItem orderItem = item.GetComponent<OrderItem>();
            if (orderItem != null)
            {
                trayOrder += trayOrder == "" ? orderItem.itemName : $" + {orderItem.itemName}";
            }
        }

        isOrderCompletedCorrectly = npcOrder == trayOrder;
        Debug.Log(isOrderCompletedCorrectly ? "Correct order! NPC is happy." : "Wrong order! NPC is angry.");

        LeaveQueue(isOrderCompletedCorrectly);
    }

    public void LeaveQueue(bool correctOrder)
    {
        isOrderCompletedCorrectly = correctOrder;
        float delayDuration = 2f;
        StartCoroutine(HandleExitAfterDelay(delayDuration));
    }

    private IEnumerator HandleExitAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine(FollowExitPath(isOrderCompletedCorrectly ? happyExitPath : angryExitPath));
    }

    private IEnumerator FollowExitPath(List<Transform> exitPath)
    {
        if (currentQueuePointIndex >= queuePoints.Count)
        {
            queuePointOccupied[currentQueuePointIndex - 1] = false;
        }

        foreach (Transform exitPoint in exitPath)
        {
            while (Vector3.Distance(transform.position, exitPoint.position) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, exitPoint.position, moveSpeed * Time.deltaTime);
                yield return null;
            }
        }

        Debug.Log("NPC has exited the scene.");

        if (spawner != null)
        {
            spawner.OnNPCProcessed();
        }
        else
        {
            Debug.LogError("Spawner reference is null. Make sure it is assigned in the Inspector.");
        }

        Destroy(gameObject);
    }

    public void SetSpawner(NPCSpawner spawner)
    {
        this.spawner = spawner;
    }

    /// <summary>
    /// Generates a random order.
    /// </summary>
    private void GenerateRandomOrder()
    {
        string[] menuItems = { "Hotdog", "Popcorn" };
        string[] sodaFlavors = { "Green Soda", "Orange Soda", "Purple Soda" };

        string mainItem = menuItems[Random.Range(0, menuItems.Length)];
        bool includeSoda = Random.value > 0.5f;
        string soda = includeSoda ? sodaFlavors[Random.Range(0, sodaFlavors.Length)] : "";

        npcOrder = includeSoda ? $"{mainItem} + {soda}" : mainItem;

        if (orderText != null)
        {
            orderText.text = npcOrder;
        }
    }
}