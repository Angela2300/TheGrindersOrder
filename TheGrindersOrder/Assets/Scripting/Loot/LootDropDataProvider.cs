using System.Collections.Generic;
using UnityEngine;

//   When CSVLoader is ready, wire it inside GetDropsFromCSV() below.
//   EnemyLootDrop.cs requires ZERO edits when that happens.


public class LootDropDataProvider : MonoBehaviour
{
    [Header("Mode")]
    [Tooltip("ON = use Inspector drop tables below. OFF = try to use CSVLoader.")]
    public bool useInspectorFallback = true;

    [Header("Inspector Fallback Drop Tables")]
    [Tooltip("One entry per enemy type. Fill these while CSV is not ready.")]
    public List<EnemyDropTable> fallbackTables = new List<EnemyDropTable>();

    public List<LootDropEntry> GetDropsForEnemy(string enemyType)
    {
        if (useInspectorFallback)
        {
            return GetDropsFromInspector(enemyType);
        }
        else
        {

            Debug.LogWarning($"[LootDropDataProvider] CSV mode not connected yet. Falling back to Inspector for '{enemyType}'.");
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

        Debug.LogWarning($"[LootDropDataProvider] No Inspector drop table found for enemy type: '{enemyType}'");
        return new List<LootDropEntry>();
    }
}

[System.Serializable]
public class EnemyDropTable
{
    [Tooltip("Must exactly match the enemyType string on EnemyLootDrop.cs")]
    public string enemyType;

    public List<LootDropEntry> drops = new List<LootDropEntry>();
}
