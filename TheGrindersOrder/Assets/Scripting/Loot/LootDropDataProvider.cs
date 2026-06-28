using System.Collections.Generic;
using UnityEngine;


public class LootDropDataProvider : MonoBehaviour
{
    [Header("Mode")]
    public bool useInspectorFallback = true;

    [Header("Inspector Fallback Drop Tables")]
    public List<EnemyDropTable> fallbackTables = new List<EnemyDropTable>();

    public List<LootDropEntry> GetDropsForEnemy(string enemyType)
    {
        if (useInspectorFallback)
        {
            return GetDropsFromInspector(enemyType);
        }
        else
        {
            return GetDropsFromInspector(enemyType);
        }
    }

    List<LootDropEntry> GetDropsFromInspector(string enemyType)
    {
        foreach (var table in fallbackTables)
        {
            if (table.enemyType == enemyType)
                return table.drops;
        }

        return new List<LootDropEntry>();
    }
}

[System.Serializable]
public class EnemyDropTable
{
    public string enemyType;

    public List<LootDropEntry> drops = new List<LootDropEntry>();
}
