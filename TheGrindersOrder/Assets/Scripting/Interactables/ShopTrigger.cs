using System;
using UnityEngine;

public class ShopTrigger : MonoBehaviour
{
    public static event Action OnShopOpened;
    public static event Action OnShopClosed;

    [Header("References")]
    [Tooltip("Drag Panel_Shop from Canvas")]
    public GameObject shopPanel;

    [Tooltip("Drag the ShopSystem GameObject from the scene")]
    public ShopSystem shopSystem;

    [Tooltip("Optional prompt UI shown when player is in range")]
    public GameObject promptUI;

    [Header("Open Mode")]
    [Tooltip("ON = shop opens instantly. OFF = player presses openKey.")]
    public bool openOnEnter = true;

    public KeyCode openKey = KeyCode.E;

    private bool playerInRange = false;

    private void Start()
    {
        if (shopPanel != null)
            shopPanel.SetActive(false);

        if (promptUI != null)
            promptUI.SetActive(false);
    }

    private void Update()
    {
        if (!openOnEnter && playerInRange && Input.GetKeyDown(openKey))
        {
            OpenShop();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerInventory inventory = other.GetComponent<PlayerInventory>();

        if (inventory == null)
        {
            Debug.LogWarning("[ShopTrigger] Player has no PlayerInventory component.");
            return;
        }

        playerInRange = true;

        if (shopSystem != null)
            shopSystem.SetInventory(inventory);

        if (promptUI != null)
            promptUI.SetActive(true);

        if (openOnEnter)
            OpenShop();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = false;

        if (shopSystem != null)
            shopSystem.ClearInventory();

        if (promptUI != null)
            promptUI.SetActive(false);

        CloseShop();
    }

    public void OpenShop()
    {
        if (shopPanel == null)
        {
            Debug.LogError("[ShopTrigger] shopPanel is not assigned.");
            return;
        }

        shopPanel.SetActive(true);

        if (shopSystem != null)
            shopSystem.RefreshAllRows();

        OnShopOpened?.Invoke();
    }

    public void CloseShop()
    {
        if (shopPanel == null) return;

        shopPanel.SetActive(false);
        OnShopClosed?.Invoke();
    }
}