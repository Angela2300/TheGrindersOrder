[System.Serializable]
public class EnemyData
{
    // Basic Info
    public string enemyID;
    public string displayName;
    public string role;
    public string attackType;

    // Combat Stats
    public float damageHearts;
    public int damageLives;
    public int health;

    // Movement & Behavior
    public string moveSpeedTier;
    public float moveSpeedValue;
    public string rangeTier;
    public bool followsPlayer;

    // Loot
    public int meatDropAmt;
    public int coinDropAmt;
    public string weaponId;
    public int spawnCount;
    public bool isBomber;
}