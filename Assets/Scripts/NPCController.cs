using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class NPCController : MonoBehaviour
{
    public string npcOrder;

    [Header("QueuePoints Settings")]
    public string queuePointTag = "QueuePoint"; // Tag to identify queue points in the scene
    public string happyExitTag = "HappyExitPath"; // Tag to identify happyExit waypoints in the scene
    public string angryExitTag = "AngryExitPath"; // Tag to identify angryExit waypoints in the scene

    public float moveSpeed = 2f; // Movement speed of the NPC
    public float checkDelay = 0.5f; // Delay between checks for the next queue point

    private List<Transform> queuePoints = new List<Transform>(); // List of queue points
    private List<Transform> happyExitPath = new List<Transform>(); // List of happyExit waypoints
    private List<Transform> angryExitPath = new List<Transform>(); // List of angryExit waypoints

    private int currentQueuePointIndex = 0; // Index of the current queue point
    private static Dictionary<int, bool> queuePointOccupied = new Dictionary<int, bool>(); // Tracks if queue points are occupied

    [Header("Spawner Reference")]
    private NPCSpawner spawner; // Reference to the NPCSpawner

    private bool isOrderCompletedCorrectly = false; // Tracks if the order was completed correctly

    public TextMesh NPCorderText;

    private void Start()
    {
        // Find all queue points tagged with the specified tag
        GameObject[] queuePointObjects = GameObject.FindGameObjectsWithTag(queuePointTag);
        foreach (GameObject obj in queuePointObjects)
        {
            queuePoints.Add(obj.transform);
        }

        // Find all happy exit points tagged with the specified tag
        GameObject[] happyExitObjects = GameObject.FindGameObjectsWithTag(happyExitTag);
        foreach (GameObject obj in happyExitObjects)
        {
            happyExitPath.Add(obj.transform);
        }

        // Find all angry exit points tagged with the specified tag
        GameObject[] angryExitObjects = GameObject.FindGameObjectsWithTag(angryExitTag);
        foreach (GameObject obj in angryExitObjects)
        {
            angryExitPath.Add(obj.transform);
        }

        // Sort queue points in descending order by their name or position if needed
        queuePoints.Sort((a, b) => b.name.CompareTo(a.name));

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

            // Check if the queue point is not occupied
            if (!queuePointOccupied[currentQueuePointIndex])
            {
                queuePointOccupied[currentQueuePointIndex] = true; // Mark current queue point as occupied

                // Move towards the queue point
                while (Vector3.Distance(transform.position, targetQueuePoint.position) > 0.1f)
                {
                    transform.position = Vector3.MoveTowards(transform.position, targetQueuePoint.position, moveSpeed * Time.deltaTime);
                    yield return null;
                }

                transform.position = targetQueuePoint.position; // Snap to exact position

                // Mark the previous queue point as unoccupied (if not the first one)
                if (currentQueuePointIndex > 0)
                {
                    queuePointOccupied[currentQueuePointIndex - 1] = false;
                }

                if (currentQueuePointIndex == 0)
                {
                    GenerateRandomOrder();
                }

                currentQueuePointIndex++; // Move to the next queue point
            }

            // Wait before checking again
            yield return new WaitForSeconds(checkDelay);
        }


        // NPC reached the end of the queue points
        OnQueueComplete();
    }

    private void OnQueueComplete()
    {
        Debug.Log("NPC has completed the queue.");

        // Delay duration can be a fixed value or dynamically calculated
        float delayDuration = Random.Range(2f, 5f); // Example: random delay between 2-5 seconds
        StartCoroutine(HandleExitAfterDelay(delayDuration));
    }

    private IEnumerator HandleExitAfterDelay(float delay)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);


        // Determine which path to take (happy or angry)
        bool isHappy = isOrderCompletedCorrectly; // Replace with your own logic to decide path

        if (isHappy)
        {
            StartCoroutine(FollowExitPath(happyExitPath));
        }
        else
        {
            StartCoroutine(FollowExitPath(angryExitPath));
        }
    }

    private IEnumerator FollowExitPath(List<Transform> exitPath)
    {
        // Ensure the last queue point is marked as unoccupied after the NPC moves past it
        if (currentQueuePointIndex == queuePoints.Count)
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

        // Once the NPC reaches the end of the path, destroy it
        Debug.Log("NPC has exited the scene.");

        // Notify the spawner that this NPC has been processed
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

    public void SetOrderStatus(bool isCompleted)
    {
        isOrderCompletedCorrectly = isCompleted; // Set this value based on external logic
    }
    private void GenerateRandomOrder()
    {
        string[] menuItems = { "Bun", "Sausage", "Hotdog", "Popcorn"}; //"Popcorn"
        string[] sodaFlavors = { "Green Soda", "Orange Soda", "Purple Soda" };

        // Randomly pick a main item
        string mainItem = menuItems[Random.Range(0, menuItems.Length)];

        // 50% chance to include a soda in the order
        //bool includeSoda = false; // Random.value > 0.5f;
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