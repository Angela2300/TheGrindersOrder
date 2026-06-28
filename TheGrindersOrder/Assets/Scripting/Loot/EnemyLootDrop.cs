using System.Collections.Generic;
using UnityEngine;

//     Enemy teammate adds ONE line to their death function:
//     GetComponent<EnemyLootDrop>()?.SpawnLoot(transform.position);
//
// DOES NOT DEPEND ON: CSVLoader, LootRow, PlayerInventory, PlayerStats


public class EnemyLootDrop : MonoBehaviour
{
    [Header("Enemy Identity")]
   
    public string enemyType = "farmer";

    [Header("Data Source")]
    public LootDropDataProvider dataProvider;

    [Header("Loot Prefabs")]
    public GameObject coinPrefab;
    public GameObject meatPrefab;
    public GameObject medkitPrefab;
    public GameObject bossItemPrefab;

    [Header("Scatter")]
    public float scatterRadius = 0.5f;

    //Called by enemy teammate's death function 
    public void SpawnLoot(Vector3 deathPosition)
    {
        if (dataProvider == null)
        {
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
