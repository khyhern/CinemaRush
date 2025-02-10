using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } // Singleton pattern

    [Header("Game Timer Settings")]
    public float gameDuration = 300f; // Total game time in seconds
    private float remainingTime;
    private bool isGameActive = false;

    [Header("Customer Tracking")]
    private int happyCustomers = 0;
    private int angryCustomers = 0;

    [Header("UI Elements")]
    public Text timerText;
    public Text happyCustomersText;
    public Text angryCustomersText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        remainingTime = gameDuration;
        Debug.Log("[GameManager] GameManager initialized. Game duration set to " + gameDuration + " seconds.");
    }

    private void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        isGameActive = true;
        Debug.Log("[GameManager] Game started!");
        StartCoroutine(GameTimer());
    }

    private IEnumerator GameTimer()
    {
        while (remainingTime > 0 && isGameActive)
        {
            remainingTime -= Time.deltaTime;
            UpdateUI();
            Debug.Log("[GameManager] Time remaining: " + Mathf.Ceil(remainingTime) + "s");
            yield return null;
        }

        isGameActive = false;
        EndGame();
    }

    public void RegisterCustomer(bool isHappy)
    {
        if (isHappy)
        {
            happyCustomers++;
            Debug.Log("[GameManager] Happy customer registered! Total happy customers: " + happyCustomers);
        }
        else
        {
            angryCustomers++;
            Debug.Log("[GameManager] Angry customer registered! Total angry customers: " + angryCustomers);
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        if (timerText != null)
        {
            timerText.text = $"Time: {Mathf.Ceil(remainingTime)}s";
        }

        if (happyCustomersText != null)
        {
            happyCustomersText.text = $"Happy Customers: {happyCustomers}";
        }

        if (angryCustomersText != null)
        {
            angryCustomersText.text = $"Angry Customers: {angryCustomers}";
        }
    }

    private void EndGame()
    {
        Debug.Log("[GameManager] Game Over!");
        Debug.Log("[GameManager] Final Count - Happy Customers: " + happyCustomers + " | Angry Customers: " + angryCustomers);
    }

    public int GetHappyCustomers() => happyCustomers;
    public int GetAngryCustomers() => angryCustomers;
    public float GetRemainingTime() => remainingTime;
}
