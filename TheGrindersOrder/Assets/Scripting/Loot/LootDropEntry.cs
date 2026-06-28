using System;
using UnityEngine;

// Simple data class describing one possible loot drop.
// Used by both LootDropDataProvider and EnemyLootDrop.
// No inheritance, no interfaces — just a plain data holder.
[Serializable]
public class LootDropEntry
{
    [Tooltip("Must match PlayerInventory resource strings: loot_coin, loot_meat, loot_medkit, quest_boss_item")]
    public string resourceType;

    [Tooltip("Minimum number dropped if this roll succeeds")]
    public int minAmount = 1;

    [Tooltip("Maximum number dropped if this roll succeeds")]
    public int maxAmount = 1;

    [Tooltip("0 = never drops. 1 = always drops. 0.3 = 30% chance.")]
    [Range(0f, 1f)]
    public float dropChance = 1f;
}
