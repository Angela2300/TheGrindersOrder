using UnityEngine;
public class LootItem : MonoBehaviour
{

//    Player GameObject must have PlayerInventory attached with:
//    public void AddItem(string resourceType, int amount)
//    Player GameObject Tag must be set to "Player"

    [Header("Loot Config")]
    [Tooltip("Resource string passed to PlayerInventory.AddItem(). " +
             "Values: loot_coin, loot_meat, loot_medkit, quest_boss_item")]
    public string resourceType = "loot_coin";

    [Tooltip("How many units of this resource are given on pickup")]
    public int amount = 1;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerInventory inventory = other.GetComponent<PlayerInventory>();

        if (inventory == null)
        {
            Debug.LogWarning("[LootItem] Player is missing PlayerInventory component.");
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
