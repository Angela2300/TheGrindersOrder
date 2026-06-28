using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ShopDetailPageUI : MonoBehaviour
{
    [Header("System Reference")]
    public ShopSystem shopSystem;

    [Header("Page References")]
    [Tooltip("Drag Page_CategoryList here")]
    public GameObject categoryListPage;
    [Tooltip("Drag this same GameObject (Page_UpgradeDetail) here")]
    public GameObject detailPage;

    [Header("Buttons")]
    public Button backButton;
    public Button buyButton;

    [Header("Display Fields")]
    public Image upgradeIcon;
    public TextMeshProUGUI upgradeNameText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI effectText;
    public TextMeshProUGUI costText;
    public TextMeshProUGUI feedbackText;

    [Header("Feedback Timing")]
    [Tooltip("How long the feedback message stays visible after a failed purchase")]
    public float feedbackDuration = 1.5f;

    string selectedCategory = "";
    float feedbackTimer = 0f;

    void Start()
    {
        if (backButton != null)
            backButton.onClick.AddListener(BackToCategoryList);
        else
            Debug.LogWarning("[ShopDetailPageUI] backButton is not assigned.");

        if (buyButton != null)
            buyButton.onClick.AddListener(TryBuySelectedUpgrade);
        else
            Debug.LogWarning("[ShopDetailPageUI] buyButton is not assigned.");

        if (feedbackText != null)
            feedbackText.text = "";
    }

    void OnEnable()
    {
        ShopSystem.OnPurchaseFailed += HandlePurchaseFailed;
    }

    void OnDisable()
    {
        ShopSystem.OnPurchaseFailed -= HandlePurchaseFailed;
    }

    void Update()
    {
        if (feedbackTimer > 0f)
        {
            feedbackTimer -= Time.deltaTime;
            if (feedbackTimer <= 0f && feedbackText != null)
                feedbackText.text = "";
        }
    }

    public void ShowCategory(string category)
    {
        selectedCategory = category;

        if (categoryListPage != null) categoryListPage.SetActive(false);
        if (detailPage != null) detailPage.SetActive(true);

        RefreshDisplay();
    }

    public void RefreshDisplay()
    {
        if (shopSystem == null)
        {
            Debug.LogWarning("[ShopDetailPageUI] shopSystem is not assigned.");
            return;
        }

        if (string.IsNullOrEmpty(selectedCategory)) return;

        UpgradeOption option = shopSystem.GetOption(selectedCategory);
        if (option == null)
        {
            Debug.LogWarning($"[ShopDetailPageUI] No UpgradeOption found for category '{selectedCategory}'.");
            return;
        }

        int currentLevel = shopSystem.GetCurrentLevel(selectedCategory);
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


    public void TryBuySelectedUpgrade()
    {
        if (string.IsNullOrEmpty(selectedCategory)) return;


        shopSystem.TryPurchase(selectedCategory);

        RefreshDisplay();
    }

    public void BackToCategoryList()
    {
        if (detailPage != null) detailPage.SetActive(false);
        if (categoryListPage != null) categoryListPage.SetActive(true);
    }

    void HandlePurchaseFailed(string reason)
    {
        if (feedbackText == null) return;
        feedbackText.text = reason;
        feedbackTimer = feedbackDuration;
    }
}
