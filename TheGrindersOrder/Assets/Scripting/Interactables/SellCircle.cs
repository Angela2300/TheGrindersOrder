using System;
using UnityEngine;

//     PlayerInventory must have:
//     public int  GetItemCount(string resourceType)
//     public void RemoveAllOfType(string resourceType)
//     public void AddItem(string resourceType, int amount)

public class SellCircle : MonoBehaviour
{
    public static event Action<int> OnSellComplete;

    [Header("Sell Settings")]
    [Tooltip("How many Coins are earned per unit of Meat Juice")]
    public int juiceSellValue = 5;

    [Tooltip("Key the player presses to sell")]
    public KeyCode sellKey = KeyCode.E;

    [Header("UI")]
    [Tooltip("Drag the sell prompt panel here")]
    public GameObject sellPromptUI;

    
    PlayerInventory currentInventory;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        // ++++
        currentInventory = other.GetComponent<PlayerInventory>();

        if (sellPromptUI != null)
            sellPromptUI.SetActive(true);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        currentInventory = null;

        if (sellPromptUI != null)
            sellPromptUI.SetActive(false);
    }

    void Update()
    {
        if (currentInventory == null) return;
        if (!Input.GetKeyDown(sellKey)) return;

        int juiceCount = currentInventory.GetItemCount("loot_meat_juice");

        if (juiceCount <= 0)
        {
            Debug.Log("[SellCircle] No juice to sell.");
            return;
        }

        int coinsEarned = juiceCount * juiceSellValue;

        currentInventory.RemoveAllOfType("loot_meat_juice");
        currentInventory.AddItem("loot_coin", coinsEarned);

        OnSellComplete?.Invoke(coinsEarned);

        Debug.Log($"[SellCircle] Sold {juiceCount} juice → {coinsEarned} coins.");
    }
}
