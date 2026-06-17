using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ShopUpgradeRowUI : MonoBehaviour
{
    [Header("Category — must match UpgradeOption.category in ShopSystem")]
    [Tooltip("Values: health, armor, speed, weapon, pet (lowercase, no spaces)")]
    public string upgradeCategory = "health";

    [Header("UI References — drag children of this row")]
    public TextMeshProUGUI upgradeNameText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI effectText;
    public TextMeshProUGUI costText;
    public Button buyButton;

    [Header("System Reference")]
    [Tooltip("Drag the ShopSystem GameObject — same reference for all rows")]
    public ShopSystem shopSystem;

    void Start()
    {
        shopSystem?.RegisterRow(this);

        if (buyButton != null)
            buyButton.onClick.AddListener(OnBuyClicked);
        else
            Debug.LogWarning($"[ShopUpgradeRowUI] buyButton is not assigned on {upgradeCategory} row.");

        RefreshDisplay();
    }

    public void RefreshDisplay()
    {
        if (shopSystem == null)
        {
            Debug.LogWarning($"[ShopUpgradeRowUI] shopSystem is not assigned on {upgradeCategory} row.");
            return;
        }

        UpgradeOption option = shopSystem.GetOption(upgradeCategory);
        if (option == null)
        {
            Debug.LogWarning($"[ShopUpgradeRowUI] No UpgradeOption found for category '{upgradeCategory}'. " +
                             "Check ShopSystem Inspector upgrade options list.");
            return;
        }

        int currentLevel = shopSystem.GetCurrentLevel(upgradeCategory);
        bool isMaxed = currentLevel >= option.maxLevel;
        int nextLevel = Mathf.Min(currentLevel + 1, option.maxLevel);

        if (upgradeNameText != null)
            upgradeNameText.text = option.displayName;

        if (levelText != null)
            levelText.text = $"Lv {currentLevel} / {option.maxLevel}";

        if (effectText != null)
        {
            float currentEffect = option.GetEffect(currentLevel);
            float nextEffect = option.GetEffect(nextLevel);

            effectText.text = isMaxed
                ? $"{option.effectDescription}: {currentEffect}"
                : $"{option.effectDescription}: {currentEffect} → {nextEffect}";
        }

        if (costText != null)
            costText.text = isMaxed ? "MAX" : $"{option.GetCost(nextLevel)} Coins";

        if (buyButton != null)
            buyButton.interactable = !isMaxed;
    }

    void OnBuyClicked()
    {
        shopSystem?.TryPurchase(upgradeCategory);
    }
}
