using System;
using UnityEngine;
using UnityEngine.InputSystem; 

public class ShopTrigger : MonoBehaviour
{
    public static event Action OnShopOpened;
    public static event Action OnShopClosed;

    [Header("References — drag these in")]
    public GameObject shopPanel;
    public ShopSystem shopSystem;
    public GameObject promptUI;

    [Header("NEW — Page References")]
    [Tooltip("Drag Page_CategoryList here")]
    public GameObject categoryListPage;
    [Tooltip("Drag Page_UpgradeDetail here")]
    public GameObject detailPage;

    [Header("Open Mode")]
    public bool openOnEnter = false;
    public KeyCode openKey = KeyCode.E;

    bool playerInRange = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerInventory inv = other.GetComponent<PlayerInventory>();
        if (inv == null)
        {
            return;
        }

        playerInRange = true;
        shopSystem?.SetInventory(inv);

        if (promptUI != null) promptUI.SetActive(true);
        if (openOnEnter) OpenShop();
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = false;
        shopSystem?.ClearInventory();

        if (promptUI != null) promptUI.SetActive(false);
        CloseShop();
    }

    void Update()
    {
        if (!openOnEnter && playerInRange && UnityEngine.InputSystem.Keyboard.current.eKey.wasPressedThisFrame)
        {

            OpenShop();
        }
    }

    public void OpenShop()
    {

        if (shopPanel == null) return;

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayShopOpen();

        shopPanel.SetActive(true);

        // NEW: Freeze the game when the shop is open
        Time.timeScale = 0f;

        //NEW: always reset to category list when shop opens
        if (categoryListPage != null) categoryListPage.SetActive(true);
        if (detailPage != null) detailPage.SetActive(false);

        OnShopOpened?.Invoke();
    }

    public void CloseShop()
    {
        if (shopPanel == null) return;

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayShopClose();

        shopPanel.SetActive(false);

        // NEW: Resume the game when the shop closes
        Time.timeScale = 1f;
    }
}

