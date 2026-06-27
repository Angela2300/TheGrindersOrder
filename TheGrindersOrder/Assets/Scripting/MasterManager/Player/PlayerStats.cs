//using UnityEngine;
//using UnityEngine.SceneManagement;

//public class PlayerStats : MonoBehaviour
//{
//    // Drag PlayerStats.csv here
//    // This file stores the player's starting values
//    public TextAsset playerStatsCSV;

//    // These are the player's current values
//    public int hearts = 5 ;
//    public int shields = 3;
//    public int coins = 0;
//    public string startWeaponID;

//    // Drag the PlayerUI object here
//    // This allows us to tell the UI to update itself
//    public PlayerUI playerUI;

//    void Start()
//    {
//        // When the game starts, load the player stats from the CSV file
//        LoadPlayerStats();
//        RefreshUI();
//    }

//    // Read the PlayerStats.csv file
//    void LoadPlayerStats()
//    {
//        // Stop if no CSV was assigned
//        if (playerStatsCSV == null)
//        {
//            Debug.LogWarning("PlayerStats CSV missing!");
//            return;
//        }

//        // Split the CSV into rows
//        string[] lines = playerStatsCSV.text.Split('\n');

//        // Read row 1
//        // Row 0 is just the column names
//        string[] values = lines[1].Trim().Split(',');

//        // Put the CSV values into our variables
//        hearts = int.Parse(values[1]);
//        shields = int.Parse(values[2]);
//        startWeaponID = values[3];
//        coins = int.Parse(values[4]);

//        // Tell the UI to update itself
//        RefreshUI();
//    }


//    public void AddCoins(int amount)
//    {
//        coins += amount;

//        Debug.Log("Coins = " + coins);

//        RefreshUI();
//    }


//    //-------------------------------------
//    // ENEMY
//    //-------------------------------------

//    public void TakeDamage(int damage)
//    {
//        Debug.Log("TakeDamage called with: " + damage);

//        // 1. Prioritize taking damage from Shields first
//        if (shields > 0)
//        {
//            int damageToShields = Mathf.Min(damage, shields);
//            shields -= damageToShields; // This MUST subtract
//            damage -= damageToShields;
//        }

//        // If there is still damage left, take out Hearts
//        if (damage > 0)
//        {
//            hearts -= damage;
//            if (hearts < 0)
//            {
//                hearts = 0;
//                GameOver();
//            }
//        }
//        Debug.Log("DEBUG: Hearts variable is now: " + hearts);
//        RefreshUI();
//        Debug.Log("Shields remaining: " + shields);
//    }


//    void GameOver()
//    {
//        Debug.Log("Game Over! Player has died.");
//        // Reloads the current level
//        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
//        // If you have a specific game over UI, you can call it here:
//        // gameOverPanel.SetActive(true);
//    }

//    //-------------------------------------
//    // SHOP 
//    //-------------------------------------


//    // Call this when the player buys something
//    public void SpendCoins(int amount)
//    {
//        coins -= amount;

//        // Don't allow negative coins
//        if (coins < 0)
//            coins = 0;

//        // Update the screen
//        RefreshUI();
//    }

//    // Call this when the player gains armor 
//    public void AddShields(int amount)
//    {
//        shields += amount;

//        // Update the screen
//        RefreshUI();
//    }

//    //-------------------------------------
//    // EVERYONE
//    //-------------------------------------

//    // This function refreshes the UI
//    // Think of it as telling the screen:
//    // "Hey! Something changed! Draw the new values!"
//    void RefreshUI()
//    {
//        if (playerUI != null)
//        {
//            playerUI.UpdateUI(hearts, shields, coins);
//        }
//    }
//}


using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    [Header("Data")]
    public TextAsset playerStatsCSV;
    public int hearts = 5;
    public int shields = 3;
    public int coins = 0;
    public string startWeaponID;

    // UPGRADED STATS
    public int maxHearts = 5;
    public float armorReduction = 0f;
    public float speedMultiplier = 1f;
    public float weaponDamageMultiplier = 1f;
 

    [Header("UI Reference")]
    public PlayerUI playerUI;

    void Start()
    {
        LoadPlayerStats();
    }

    void LoadPlayerStats()
    {
        if (playerStatsCSV == null)
        {
            Debug.LogWarning("PlayerStats CSV missing! Using default values.");
            RefreshUI();
            return;
        }

        string[] lines = playerStatsCSV.text.Split('\n');

        if (lines.Length > 1)
        {
            string[] values = lines[1].Trim().Split(',');

            hearts = int.Parse(values[1]);
            maxHearts = hearts;
            shields = int.Parse(values[2]);
            startWeaponID = values[3];
            coins = int.Parse(values[4]);
        }

        RefreshUI();
    }

    //public void TakeDamage(int damage)
    //{
    //    Debug.Log($"Taking {damage} damage. Current Shields: {shields}, Hearts: {hearts}");

    //    if (shields > 0)
    //    {
    //        for (int i = 0; i < damage; i++)
    //        {
    //            bool armorBlocked = Random.value < armorReduction;

    //            if (armorBlocked)
    //            {
    //                Debug.Log("Armor blocked shield damage.");
    //                continue;
    //            }

    //            shields--;
    //            Debug.Log($"Shield damaged! Remaining: {shields}");
    //            if (shields <= 0)
    //                break;
    //        }

    //        RefreshUI();
    //        return;
    //    }

    //    hearts -= damage;

    //    if (hearts <= 0)
    //    {
    //        hearts = 0;
    //        GameOver();
    //    }

    //    RefreshUI();
    //}

    public void TakeDamage(int damage)
    {
        Debug.Log($"Taking {damage} damage. Current Shields: {shields}, Hearts: {hearts}");

        int remainingDamage = damage;

        // 1. Process damage against Shields
        if (shields > 0)
        {
            for (int i = 0; i < damage; i++)
            {
                if (remainingDamage <= 0) break; // No more damage to process

                bool armorBlocked = Random.value < armorReduction;

                if (armorBlocked)
                {
                    Debug.Log("Armor blocked shield damage.");
                    remainingDamage--; // Even if blocked, it consumes the attack's 'hit'
                    continue;
                }

                shields--;
                remainingDamage--;
                Debug.Log($"Shield damaged! Remaining: {shields}");

                if (shields <= 0) break;
            }
        }

        // 2. Process remaining damage against Hearts
        if (remainingDamage > 0)
        {
            hearts -= remainingDamage;
            Debug.Log($"Hearts damaged! Remaining: {hearts}");
        }

        // 3. Final Checks
        if (hearts <= 0)
        {
            hearts = 0;
            GameOver();
        }

        RefreshUI();
    }

    public void AddCoins(int amount)
    {
        coins += amount;
        RefreshUI();
    }

    public void SpendCoins(int amount)
    {
        coins = Mathf.Max(0, coins - amount);
        RefreshUI();
    }

    public void AddShields(int amount)
    {
        shields += amount;
        RefreshUI();
    }

    // SHOP UPGRADE METHODS
    // Called by UpgradeManager

    public void SetMaxHealth(int value)
    {
        maxHearts = value;
        hearts = maxHearts;
        RefreshUI();
    }

    public void SetArmor(float value)
    {
        armorReduction = value;
    }

    public void SetSpeedMultiplier(float value)
    {
        speedMultiplier = value;
    }

    public void SetWeaponDamageMultiplier(float value)
    {
        weaponDamageMultiplier = value;
    }

    void GameOver()
    {
        Debug.Log("Game Over! Player has died.");

        //  Show death canvas instead of reloading scene
        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.PlayerDied();
        }

        // Remove or comment out this line if you don’t want auto reload
        // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void RefreshUI()
    {
        if (playerUI != null)
        {
            playerUI.UpdateUI(hearts, shields, coins);
        }
    }

    public void ResetStats()
    {
        hearts = maxHearts;   // restore full health
        shields = 3;          // or whatever default you want
        coins = 0;            // optional, reset coins if needed

        RefreshUI();
        Debug.Log("[PlayerStats] Stats reset on restart");
    }
}