using UnityEngine;
public class LootItem : MonoBehaviour
{

    [Header("Loot Config")]
    
    public string resourceType = "loot_coin";

    public int amount = 1;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerInventory inventory = other.GetComponent<PlayerInventory>();

        if (inventory == null)
        {
            return;
        }

        inventory.AddItem(resourceType, amount);

        if (AudioManager.Instance != null)
        {
            if (resourceType == "loot_meat")
                AudioManager.Instance.PlayMeatPickup();
            else if (resourceType == "loot_coin")
                AudioManager.Instance.PlayCoinPickup();
            else
                AudioManager.Instance.PlayLootPickup();
        }

        Destroy(gameObject);
    }
}
