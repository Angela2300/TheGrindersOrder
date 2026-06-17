[System.Serializable]
public class EnemyData
{
    // Basic Info
    public string enemyID;
    public string displayName;
    public string role;

    // Combat Stats
    public float health;
    public float damageHearts;
    public float damageLives;

    // Movement & Behavior
    public string moveSpeedTier;
    public string moveSpeedValue;
    public string rangeTier;
    public bool followsPlayer;

    // Loot
    public string coinDropAmt;
    public string weaponId;
    public string meatDropAmt;
    public int spawnCount;
    public bool isBomber;
}