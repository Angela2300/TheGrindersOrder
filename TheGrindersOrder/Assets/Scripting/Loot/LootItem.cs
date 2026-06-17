using UnityEngine;
public class LootItem : MonoBehaviour
{
    [Header("Loot Settings")]
    public string resourceType = "loot_coin"; 
    public int amount = 1;

    void OnTriggerEnter2D(Collider2D other)
    {
        // Only the player can pick up loot
        if (!other.CompareTag("Player")) return;

        PlayerInventory inventory = other.GetComponent<PlayerInventory>();
        if (inventory == null)
        {
            Debug.LogWarning("[LootItem] Player missing PlayerInventory component.");
            return;
        }

        inventory.AddItem(resourceType, amount);
        Destroy(gameObject);
    }
}

