using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopCategoryButtonUI : MonoBehaviour
{
    [Header("Category — must match UpgradeOption.category in ShopSystem")]
    public string upgradeCategory = "health";

    [Header("UI References — drag children of this button")]
    public TextMeshProUGUI categoryNameText;
    public Image iconImage;
    public TextMeshProUGUI levelPreviewText;

    [Header("Detail Page Reference")]
    [Tooltip("Drag the Page_UpgradeDetail GameObject — same reference for all category buttons")]
    public ShopDetailPageUI shopDetailPage;

    Button button;

    void Start()
    {
        button = GetComponent<Button>();
        if (button != null)
            button.onClick.AddListener(OnClick);
        else
            Debug.LogWarning($"[ShopCategoryButtonUI] No Button component found on {gameObject.name}.");

        RefreshPreview();
    }

    void OnEnable()
    {
        RefreshPreview();
    }

    //Updates the small preview shown on the category card
    public void RefreshPreview()
    {
        if (shopDetailPage == null || shopDetailPage.shopSystem == null) return;

        UpgradeOption option = shopDetailPage.shopSystem.GetOption(upgradeCategory);
        if (option == null)
        {
            Debug.LogWarning($"[ShopCategoryButtonUI] No UpgradeOption found for category '{upgradeCategory}'.");
            return;
        }

        int currentLevel = shopDetailPage.shopSystem.GetCurrentLevel(upgradeCategory);

        if (categoryNameText != null)
            categoryNameText.text = option.displayName;

        if (levelPreviewText != null)
            levelPreviewText.text = $"Lv {currentLevel} / {option.maxLevel}";
    }
    void OnClick()
    {
        if (shopDetailPage == null)
        {
            Debug.LogError("[ShopCategoryButtonUI] shopDetailPage is not assigned. Drag Page_UpgradeDetail into the Inspector.");
            return;
        }

        shopDetailPage.ShowCategory(upgradeCategory);
    }
}
