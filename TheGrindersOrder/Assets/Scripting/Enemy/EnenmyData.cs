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
    public float damageLives;
    public float health;
 

    // Movement & Behavior
    public string moveSpeedTier;
    public string moveSpeedValue;
    public string rangeTier;
    public bool followsPlayer;

    // Loot
    public string meatDropAmt;
    public string coinDropAmt;
    public string weaponId;
    public int spawnCount;
    public bool isBomber;
}