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


    public void TakeDamage(int damage)
    {
        

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayPlayerHurt();

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
                    remainingDamage--; // Even if blocked, it consumes the attack's 'hit'
                    continue;
                }

                shields--;
                remainingDamage--;

                if (shields <= 0) break;
            }
        }

        // 2. Process remaining damage against Hearts
        if (remainingDamage > 0)
        {
            hearts -= remainingDamage;
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

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayPlayerDeath();

        //  Show death canvas instead of reloading scene
        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.PlayerDied();
        }
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
    }
}