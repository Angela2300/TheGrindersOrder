using UnityEngine;

public class EnemyLootDrop : MonoBehaviour
{
    [Header("Loot Prefabs")]
    public GameObject coinPrefab;
    public GameObject meatPrefab;
    public GameObject medkitPrefab;
    public GameObject bossItemPrefab;

    [Header("Drop Settings")]
    public int meatAmount = 1;
    public int coinAmount = 1;

    [Range(0f, 1f)]
    public float coinDropChance = 0.3f;

    [Range(0f, 1f)]
    public float medkitDropChance = 0.05f;

    public bool isBoss = false;

    public void SpawnLoot(Vector3 position)
    {
        Spawn(meatPrefab, "loot_meat", meatAmount, position);

        if (Random.value <= coinDropChance)
        {
            Spawn(coinPrefab, "loot_coin", coinAmount, position);
        }

        if (Random.value <= medkitDropChance)
        {
            Spawn(medkitPrefab, "loot_medkit", 1, position);
        }

        if (isBoss)
        {
            Spawn(bossItemPrefab, "quest_boss_item", 1, position);
        }
    }

    private void Spawn(GameObject prefab, string resourceType, int amount, Vector3 position)
    {
        if (prefab == null) return;

        GameObject loot = Instantiate(prefab, position, Quaternion.identity);

        LootItem lootItem = loot.GetComponent<LootItem>();
        if (lootItem != null)
        {
            lootItem.resourceType = resourceType;
            lootItem.amount = amount;
        }
    }
}