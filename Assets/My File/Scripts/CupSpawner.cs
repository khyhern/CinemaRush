using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [Tooltip("Cup Prefab to spawn")]
    public GameObject cupPrefab;

    [Tooltip("Lid Prefab to spawn")]
    public GameObject lidPrefab;

    [Tooltip("Transform at which to spawn the cup and lid")]
    public Transform spawnPoint; // Reference to spawn point (Transform)

    [Tooltip("Offset for the lid relative to the cup")]
    public Vector3 lidOffset = new Vector3(0, 0.1f, 0);

    [Tooltip("Cooldown time (in seconds) after a spawn")]
    public float cooldownTime = 0.1f;

    private bool isOnCooldown = false;
    private HashSet<GameObject> sodaObjects = new HashSet<GameObject>(); // Track "Soda" objects

    private void Start()
    {
        SpawnInitialObjects();
    }

    private void SpawnInitialObjects()
    {
        if (!isOnCooldown && sodaObjects.Count == 0 && spawnPoint != null)
        {
            SpawnCupAndLid();
            StartCoroutine(CooldownRoutine());
        }
        else
        {
            if (spawnPoint == null)
            {
                Debug.LogWarning("Spawn Point is not assigned.");
            }
            else
            {
                Debug.LogWarning("Cooldown active or Soda object detected.");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Soda"))
        {
            sodaObjects.Add(other.gameObject);
            Debug.Log($"Soda entered: {other.name}. Total: {sodaObjects.Count}");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (sodaObjects.Contains(other.gameObject))
        {
            sodaObjects.Remove(other.gameObject);
            Debug.Log($"Soda exited: {other.name}. Remaining: {sodaObjects.Count}");

            if (sodaObjects.Count == 0 && !isOnCooldown)
            {
                SpawnCupAndLid();
            }
        }
    }

    private void SpawnCupAndLid()
    {
        if (cupPrefab != null && spawnPoint != null)
        {
            // Spawn the cup with a specific rotation to correct the -90 degree issue
            Instantiate(cupPrefab, spawnPoint.position, Quaternion.Euler(-90, 0, 0)); // Adjust rotation if needed
            Debug.Log("Spawned Cup at: " + spawnPoint.position);
        }
        else
        {
            if (cupPrefab == null)
                Debug.LogWarning("Cup Prefab is not assigned.");
            if (spawnPoint == null)
                Debug.LogWarning("Spawn Point is not assigned.");
        }

        if (lidPrefab != null && spawnPoint != null)
        {
            // Spawn the lid with the same rotation to match the cup
            Instantiate(lidPrefab, spawnPoint.position + lidOffset, Quaternion.Euler(-90, 0, 0)); // Offset cup spawn rotation
            Debug.Log("Spawned Lid at: " + (spawnPoint.position + lidOffset));
        }
        else
        {
            if (lidPrefab == null)
                Debug.LogWarning("Lid Prefab is not assigned.");
            if (spawnPoint == null)
                Debug.LogWarning("Spawn Point is not assigned.");
        }

        StartCoroutine(CooldownRoutine());
    }


    private IEnumerator CooldownRoutine()
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(cooldownTime);
        isOnCooldown = false;
    }
}
