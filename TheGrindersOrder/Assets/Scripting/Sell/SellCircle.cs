using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections;

public class SellCircle : MonoBehaviour
{
    [Header("Sell Settings")]
    [SerializeField] private int meatSellValue = 5;

    [Header("Floating Prompt")]
    [SerializeField] private GameObject floatingPrompt;
    [SerializeField] private TextMeshProUGUI feedbackText;
    [SerializeField] private float feedbackDisplayTime = 2f;

    [Header("Debug")]
    [SerializeField] private bool debugLogs = true;

    private Inventory playerInventory;
    private PlayerStats playerStats;
    private bool playerInRange = false;
    private Coroutine feedbackRoutine;

    private void Awake()
    {
        SetupPrompt();
        HidePromptAndFeedback();
    }

    private void Update()
    {
        if (!playerInRange) return;

        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
            SellMeat();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInventory = other.GetComponentInParent<Inventory>();
        playerStats = other.GetComponentInParent<PlayerStats>();

        if (playerInventory == null)
            playerInventory = other.GetComponentInChildren<Inventory>();

        if (playerStats == null)
            playerStats = other.GetComponentInChildren<PlayerStats>();

        if (playerInventory == null || playerStats == null)
        {
            Debug.LogWarning("[SellCircle] Missing Inventory or PlayerStats.");
            return;
        }

        playerInRange = true;
        ShowPrompt();

        if (debugLogs)
            Debug.Log("[SellCircle] Player entered sell zone.");
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = false;
        playerInventory = null;
        playerStats = null;

        if (feedbackRoutine != null)
        {
            StopCoroutine(feedbackRoutine);
            feedbackRoutine = null;
        }

        HidePromptAndFeedback();

        if (debugLogs)
            Debug.Log("[SellCircle] Player exited sell zone.");
    }

    private void SellMeat()
    {
        int meatCount = playerInventory.meatCount;

        if (meatCount <= 0)
        {
            ShowFeedback("No meat to sell.");
            return;
        }

        int coinsEarned = meatCount * meatSellValue;

        playerStats.AddCoins(coinsEarned);
        playerInventory.meatCount = 0;

        RefreshInventoryUI();

        ShowFeedback($"Sold {meatCount} meat for {coinsEarned} coins.");

        LevelManager.OnMeatSold(meatCount);

        if (debugLogs)
            Debug.Log($"[SellCircle] Sold {meatCount} meat for {coinsEarned} coins.");
    }

    private void RefreshInventoryUI()
    {
        if (playerInventory.inventoryUI == null) return;

        playerInventory.inventoryUI.UpdateInventory(
            playerInventory.weapons,
            playerInventory.slotUsed,
            playerInventory.slotTypes,
            playerInventory.meatCount
        );
    }

    private void SetupPrompt()
    {
        if (floatingPrompt == null) return;

        TextMeshProUGUI promptTMP =
            floatingPrompt.GetComponentInChildren<TextMeshProUGUI>();

        if (promptTMP != null)
            promptTMP.text = "Press E To Sell";
    }

    private void ShowPrompt()
    {
        if (floatingPrompt != null)
            floatingPrompt.SetActive(true);

        if (feedbackText != null)
            feedbackText.gameObject.SetActive(false);
    }

    private void ShowFeedback(string message)
    {
        if (feedbackText == null) return;

        if (floatingPrompt != null)
            floatingPrompt.SetActive(false);

        feedbackText.text = message;
        feedbackText.gameObject.SetActive(true);

        if (feedbackRoutine != null)
            StopCoroutine(feedbackRoutine);

        feedbackRoutine = StartCoroutine(HideFeedbackAfterDelay());
    }

    private IEnumerator HideFeedbackAfterDelay()
    {
        yield return new WaitForSeconds(feedbackDisplayTime);

        if (feedbackText != null)
            feedbackText.gameObject.SetActive(false);

        if (playerInRange)
            ShowPrompt();

        feedbackRoutine = null;
    }

    private void HidePromptAndFeedback()
    {
        if (floatingPrompt != null)
            floatingPrompt.SetActive(false);

        if (feedbackText != null)
            feedbackText.gameObject.SetActive(false);
    }
}