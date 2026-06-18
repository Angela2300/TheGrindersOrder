using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    // Drag PlayerStats.csv here
    // This file stores the player's starting values
    public TextAsset playerStatsCSV;

    // These are the player's current values
    public int hearts;
    public int armorLives;
    public int coins;
    public string startWeaponID;

    // Drag the PlayerUI object here
    // This allows us to tell the UI to update itself
    public PlayerUI playerUI;

    void Start()
    {
        // When the game starts, load the player stats from the CSV file
        LoadPlayerStats();
    }

    // Read the PlayerStats.csv file
    void LoadPlayerStats()
    {
        // Stop if no CSV was assigned
        if (playerStatsCSV == null)
        {
            Debug.LogWarning("PlayerStats CSV missing!");
            return;
        }

        // Split the CSV into rows
        string[] lines = playerStatsCSV.text.Split('\n');

        // Read row 1
        // Row 0 is just the column names
        string[] values = lines[1].Trim().Split(',');

        // Put the CSV values into our variables
        hearts = int.Parse(values[1]);
        armorLives = int.Parse(values[2]);
        startWeaponID = values[3];
        coins = int.Parse(values[4]);

        // Tell the UI to update itself
        RefreshUI();
    }


    public void AddCoins(int amount)
    {
        coins += amount;

        Debug.Log("Coins = " + coins);

        RefreshUI();
    }


    //-------------------------------------
    // ENEMY
    //-------------------------------------

    // Call this when an enemy attacks the player
    public void TakeDamage(int damage)
    {
        // Damage one shield first
        if (armorLives > 0)
        {
            armorLives -= damage;

            // Prevent shields from going negative
            if (armorLives < 0)
                armorLives = 0;
        }

        // If no shields are left, damage hearts
        else
        {
            hearts -= damage;

            // Prevent hearts from going negative
            if (hearts < 0)
                hearts = 0;
        }

        // Tell the UI to update itself
        RefreshUI();
    }

    //-------------------------------------
    // SHOP 
    //-------------------------------------


    // Call this when the player buys something
    public void SpendCoins(int amount)
    {
        coins -= amount;

        // Don't allow negative coins
        if (coins < 0)
            coins = 0;

        // Update the screen
        RefreshUI();
    }

    // Call this when the player gains armor or shields
    public void AddArmor(int amount)
    {
        armorLives += amount;

        // Update the screen
        RefreshUI();
    }

    //-------------------------------------
    // EVERYONE
    //-------------------------------------

    // This function refreshes the UI
    // Think of it as telling the screen:
    // "Hey! Something changed! Draw the new values!"
    void RefreshUI()
    {
        if (playerUI != null)
        {
            playerUI.UpdateUI(hearts, armorLives, coins);
        }
        else
        {
            Debug.LogWarning("PlayerUI not assigned!");
        }
    }
}