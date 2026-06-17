using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private Dictionary<string, int> items = new Dictionary<string, int>();

    public void AddItem(string resourceType, int amount)
    {
        if (!items.ContainsKey(resourceType))
            items[resourceType] = 0;

        items[resourceType] += amount;

        Debug.Log($"Added {amount} {resourceType}. Total: {items[resourceType]}");
    }

    public int GetItemCount(string resourceType)
    {
        if (!items.ContainsKey(resourceType))
            return 0;

        return items[resourceType];
    }

    public bool RemoveItem(string resourceType, int amount)
    {
        if (!items.ContainsKey(resourceType))
            return false;

        if (items[resourceType] < amount)
            return false;

        items[resourceType] -= amount;

        Debug.Log($"Removed {amount} {resourceType}. Remaining: {items[resourceType]}");

        return true;
    }

    public void RemoveAllOfType(string resourceType)
    {
        if (!items.ContainsKey(resourceType))
            return;

        items[resourceType] = 0;
    }

    // Optional coin API for ShopSystem
    public int GetCoins()
    {
        return GetItemCount("loot_coin");
    }

    public bool SpendCoins(int amount)
    {
        return RemoveItem("loot_coin", amount);
    }

    // Debug helper
    [ContextMenu("Give Test Resources")]
    void GiveTestResources()
    {
        AddItem("loot_coin", 999);
        AddItem("loot_meat", 20);
        AddItem("loot_meat_juice", 10);
        AddItem("loot_medkit", 5);
    }
}