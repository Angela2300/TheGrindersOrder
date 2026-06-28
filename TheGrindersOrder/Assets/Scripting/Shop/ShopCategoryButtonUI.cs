using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopCategoryButtonUI : MonoBehaviour
{
    [Header("Category")]
    public string upgradeCategory = "health";

    [Header("UI References")]
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
            return;

            button.onClick.AddListener(OnClick);
      
        RefreshPreview();
    }

    void OnEnable()
    {
        RefreshPreview();
    }

    public void RefreshPreview()
    {
        if (shopDetailPage == null || shopDetailPage.shopSystem == null) return;

        UpgradeOption option = shopDetailPage.shopSystem.GetOption(upgradeCategory);
       
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
