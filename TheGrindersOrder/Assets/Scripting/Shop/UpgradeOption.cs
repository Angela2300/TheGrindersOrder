using System;
using UnityEngine;


[Serializable]
public class UpgradeOption
{
    public string category;

    public string displayName;

    public int maxLevel = 5;

    public int[] costPerLevel = { 5, 10, 20, 35, 55 };

    public string effectType;

    public float[] effectPerLevel = { 1f, 2f, 3f, 4f, 5f };

    public string effectDescription;

    public int GetCost(int nextLevel)
    {
        int index = nextLevel - 1;
        if (index >= 0 && index < costPerLevel.Length)
            return costPerLevel[index];
        return 9999;
    }

 
    public float GetEffect(int level)
    {
        int index = level - 1;
        if (index >= 0 && index < effectPerLevel.Length)
            return effectPerLevel[index];
        return 0f;
    }
}
