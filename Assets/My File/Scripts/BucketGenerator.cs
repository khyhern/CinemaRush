using System.Collections;
using UnityEngine;

public class BucketGenerator : MonoBehaviour
{
    [Header("Spawn Settings")]
    [Tooltip("Prefab to spawn when the object leaves the socket")]
    public GameObject prefabToSpawn;

    [Tooltip("Position at which to spawn the prefab")]
    public Vector3 spawnPosition = new Vector3(-0.1364518f, 3.46f, -8.879797f);

    [Tooltip("Cooldown time (in seconds) after a spawn")]
    public float cooldownTime = 0.1f;

    [Tooltip("Time to ignore trigger events immediately after spawning")]
    public float ignoreTimeAfterSpawn = 0.5f;

    private GameObject currentObject;
    private bool isOnCooldown = false;
    private bool ignoreTrigger = false;

    private void Start()
    {
        SpawnInitialObject();
    }

    private void SpawnInitialObject()
    {
        if (prefabToSpawn != null)
        {
            currentObject = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
            Debug.Log("Initial object spawned: " + currentObject.name);
            StartCoroutine(IgnoreTriggerRoutine());
        }
        else
        {
            Debug.LogWarning("Prefab to spawn is not assigned.");
        }
    }

    private IEnumerator IgnoreTriggerRoutine()
    {
        ignoreTrigger = true;
        yield return new WaitForSeconds(ignoreTimeAfterSpawn);
        ignoreTrigger = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (ignoreTrigger) return;

        GameObject rootObj = other.transform.root.gameObject;
        if (currentObject == null)
        {
            currentObject = rootObj;
            Debug.Log("Object entered socket: " + currentObject.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (ignoreTrigger) return;

        GameObject rootObj = other.transform.root.gameObject;
        if (currentObject != null && rootObj == currentObject)
        {
            Debug.Log("Object left socket: " + currentObject.name);
            currentObject = null;
            SpawnPrefab(); // Immediately spawn a new one
        }
    }

    private void SpawnPrefab()
    {
        if (prefabToSpawn != null)
        {
            if (!isOnCooldown)
            {
                Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
                Debug.Log("Spawned new object at: " + spawnPosition);
                StartCoroutine(CooldownRoutine());
            }
        }
        else
        {
            Debug.LogWarning("Prefab to spawn is not assigned.");
        }
    }

    private IEnumerator CooldownRoutine()
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(cooldownTime);
        isOnCooldown = false;
    }
}
