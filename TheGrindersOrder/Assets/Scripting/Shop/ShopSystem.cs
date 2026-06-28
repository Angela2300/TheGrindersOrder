using System;
using System.Collections.Generic;
using UnityEngine;

public class ShopSystem : MonoBehaviour
{
    public static event Action<string, int, string, float> OnUpgradePurchased;
    public static event Action<string> OnPurchaseFailed;

    [Header("Data Source")]
    public bool useCSV = false;

    [Header("Upgrade Options")]
    public List<UpgradeOption> upgradeOptions = new List<UpgradeOption>();

    [Header("PlayerInventory API Mode")]
    public bool useCoinSpecificAPI = false;

    private Dictionary<string, int> currentLevels = new Dictionary<string, int>();
    private PlayerInventory currentInventory;

    private void Awake()
    {
        LoadUpgradeOptions();
        InitialiseLevels();
    }

    private void LoadUpgradeOptions()
    {
        if (!useCSV) return;

    }

    private void InitialiseLevels()
    {
        currentLevels.Clear();

        foreach (UpgradeOption option in upgradeOptions)
        {
            if (option == null || string.IsNullOrEmpty(option.category)) continue;

            currentLevels[option.category] = 0;
        }
    }

    public void SetInventory(PlayerInventory inventory)
    {
        currentInventory = inventory;
    }

    public void ClearInventory()
    {
        currentInventory = null;
    }

    public int GetCurrentLevel(string category)
    {
        return currentLevels.TryGetValue(category, out int level) ? level : 0;
    }

    public UpgradeOption GetOption(string category)
    {
        foreach (UpgradeOption option in upgradeOptions)
        {
            if (option != null && option.category == category)
                return option;
        }

        return null;
    }

    public void TryPurchase(string category)
    {
        if (currentInventory == null)
        {
            OnPurchaseFailed?.Invoke("No player inventory found.");
            return;
        }

        UpgradeOption option = GetOption(category);

        if (option == null)
        {
            return;
        }

        int currentLevel = GetCurrentLevel(category);

        if (currentLevel >= option.maxLevel)
        {
            OnPurchaseFailed?.Invoke("Max level reached.");
            return;
        }

        int nextLevel = currentLevel + 1;
        int cost = option.GetCost(nextLevel);
        int playerCoins = GetCoinCount();

        if (playerCoins < cost)
        {
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlayUpgradeFail();

            OnPurchaseFailed?.Invoke($"Not enough coins. Need {cost}, have {playerCoins}.");
            return;
        }

        bool deducted = SpendCoins(cost);

        if (!deducted)
        {
            OnPurchaseFailed?.Invoke("Not enough coins.");
            return;
        }

        currentLevels[category] = nextLevel;

        float effectValue = option.GetEffect(nextLevel);

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayUpgradeSuccess();

        OnUpgradePurchased?.Invoke(category, nextLevel, option.effectType, effectValue);
    }

    private int GetCoinCount()
    {
        if (currentInventory == null) return 0;

        if (useCoinSpecificAPI)
            return currentInventory.GetCoins();

        return currentInventory.GetItemCount("loot_coin");
    }

    private bool SpendCoins(int amount)
    {
        if (currentInventory == null) return false;

        if (useCoinSpecificAPI)
            return currentInventory.SpendCoins(amount);

        return currentInventory.RemoveItem("loot_coin", amount);
    }
}