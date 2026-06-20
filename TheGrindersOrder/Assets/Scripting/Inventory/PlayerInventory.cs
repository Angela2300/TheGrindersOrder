using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private Inventory inventory;
    private PlayerStats playerStats;

    public void RemoveAllOfType(string resourceType)
    {
        if (resourceType == "loot_meat")
        {
            inventory.meatCount = 0;

            if (inventory.inventoryUI != null)
                inventory.inventoryUI.UpdateInventory(
                    inventory.weapons,
                    inventory.slotUsed,
                    inventory.slotTypes,
                    inventory.meatCount
                );
        }
    }

    void Awake()
    {
        inventory = GetComponent<Inventory>();
        playerStats = GetComponent<PlayerStats>();
    }

    public void AddItem(string resourceType, int amount)
    {
        if (resourceType == "loot_meat")
        {
            for (int i = 0; i < amount; i++)
                inventory.AddMeat();
        }
        else if (resourceType == "loot_coin")
        {
            playerStats.AddCoins(amount);
        }
    }   

    public int GetItemCount(string resourceType)
    {
        if (resourceType == "loot_meat")
            return inventory.meatCount;

        if (resourceType == "loot_coin")
            return playerStats.coins;

        return 0;
    }

    public bool RemoveItem(string resourceType, int amount)
    {
        if (resourceType == "loot_coin")
        {
            if (playerStats.coins < amount) return false;

            playerStats.SpendCoins(amount);
            return true;
        }

        return false;
    }

    public int GetCoins()
    {
        return playerStats.coins;
    }

    public bool SpendCoins(int amount)
    {
        return RemoveItem("loot_coin", amount);
    }
}