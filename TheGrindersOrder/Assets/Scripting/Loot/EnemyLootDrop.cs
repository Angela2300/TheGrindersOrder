using System.Collections.Generic;
using UnityEngine;

//     Enemy teammate adds ONE line to their death function:
//     GetComponent<EnemyLootDrop>()?.SpawnLoot(transform.position);
//
// DOES NOT DEPEND ON: CSVLoader, LootRow, PlayerInventory, PlayerStats


public class EnemyLootDrop : MonoBehaviour
{
    [Header("Enemy Identity")]
    [Tooltip("Must match the enemyType string in LootDropDataProvider tables. " +
             "Values: farmer, townsperson, bomber, knight, marlow_boss")]
    public string enemyType = "farmer";

    [Header("Data Source")]
    [Tooltip("Drag LootDropDataProvider from the scene into this field")]
    public LootDropDataProvider dataProvider;

    [Header("Loot Prefabs")]
    [Tooltip("Drag Loot_Coin prefab here")]
    public GameObject coinPrefab;
    [Tooltip("Drag Loot_Meat prefab here")]
    public GameObject meatPrefab;
    [Tooltip("Drag Loot_Medkit prefab here")]
    public GameObject medkitPrefab;
    [Tooltip("Drag Loot_BossItem prefab here")]
    public GameObject bossItemPrefab;

    [Header("Scatter")]
    [Tooltip("How far apart dropped items scatter from the death position")]
    public float scatterRadius = 0.5f;

    //Called by enemy teammate's death function 
    public void SpawnLoot(Vector3 deathPosition)
    {
        if (dataProvider == null)
        {
            Debug.LogError("[EnemyLootDrop] Data Provider is not assigned. Drag LootDropDataProvider into the Inspector.");
            return;
        }

        List<LootDropEntry> drops = dataProvider.GetDropsForEnemy(enemyType);

        foreach (LootDropEntry entry in drops)
        {
            // Roll against drop chance
            if (Random.value > entry.dropChance) continue;

            int dropAmount = Random.Range(entry.minAmount, entry.maxAmount + 1);
            GameObject prefab = GetPrefabForType(entry.resourceType);

            if (prefab == null)
            {
                Debug.LogWarning($"[EnemyLootDrop] No prefab assigned for resource type: {entry.resourceType}");
                continue;
            }

            for (int i = 0; i < dropAmount; i++)
            {
                Vector2 offset = Random.insideUnitCircle * scatterRadius;
                Vector3 spawnPos = deathPosition + new Vector3(offset.x, offset.y, 0f);
                Instantiate(prefab, spawnPos, Quaternion.identity);
            }
        }
    }

    GameObject GetPrefabForType(string type)
    {
        return type switch
        {
            "loot_coin" => coinPrefab,
            "loot_meat" => meatPrefab,
            "loot_medkit" => medkitPrefab,
            "quest_boss_item" => bossItemPrefab,
            _ => null
        };
    }
}
