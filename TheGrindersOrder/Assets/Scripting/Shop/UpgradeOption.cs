using System;
using UnityEngine;


[Serializable]
public class UpgradeOption
{
    [Tooltip("Internal ID — must match upgradeCategory on ShopUpgradeRowUI")]
    public string category;

    [Tooltip("Name shown in the shop row")]
    public string displayName;

    [Tooltip("Maximum purchasable level")]
    public int maxLevel = 5;

    [Tooltip("Coin cost per level. Index 0 = cost to buy level 1. Must have maxLevel entries.")]
    public int[] costPerLevel = { 5, 10, 20, 35, 55 };

    [Tooltip("Effect type string sent in OnUpgradePurchased event")]
    public string effectType;

    [Tooltip("Effect value per level. Index 0 = value at level 1. Must have maxLevel entries.")]
    public float[] effectPerLevel = { 1f, 2f, 3f, 4f, 5f };

    [Tooltip("Human-readable label for effectText in the shop row")]
    public string effectDescription;

    public int GetCost(int nextLevel)
    {
        int index = nextLevel - 1;
        if (index >= 0 && index < costPerLevel.Length)
            return costPerLevel[index];
        Debug.LogWarning($"[UpgradeOption] GetCost called with out-of-range level {nextLevel} for {category}");
        return 9999;
    }

 
    public float GetEffect(int level)
    {
        int index = level - 1;
        if (index >= 0 && index < effectPerLevel.Length)
            return effectPerLevel[index];
        Debug.LogWarning($"[UpgradeOption] GetEffect called with out-of-range level {level} for {category}");
        return 0f;
    }
}
