using System.Collections.Generic;
using UnityEngine;
using TMPro; // for TMP_Text

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public bool isLevelActive;
    private float roundTimer;

    public List<GameObject> activeEnemies = new List<GameObject>();

    // HUD references — drag TMP text objects here in Inspector
    public TMP_Text timerText;
    public TMP_Text levelText;
    public TMP_Text customersText;

    private LevelData currentLevel;

    //  Single counter for customers served
    private int customersServed = 0;

    private void Awake()
    {
        Instance = this;
    }

    public void StartLevel(LevelData level)
    {
        currentLevel = level;
        isLevelActive = true;
        roundTimer = level.timeLimit;

        //  Reset counter at start of each level
        customersServed = 0;

        Debug.Log($"Starting Level {level.levelID} | Small:{level.smallCount}, Medium:{level.mediumCount}, Bomber:{level.bomberCount}, Large:{level.largeCount}, Boss:{level.bossCount}, Time:{level.timeLimit}");

        EnemySpawner spawner = Object.FindFirstObjectByType<EnemySpawner>();
        if (spawner != null)
        {
            StartCoroutine(spawner.SpawnWave(level));
        }
        else
        {
            Debug.LogError("EnemySpawner not found in scene!");
            SpawnEnemies(level);
        }

        UpdateHUD();
    }

    private void Update()
    {
        if (!isLevelActive) return;

        // Timer countdown
        if (roundTimer > 0)
        {
            roundTimer -= Time.deltaTime;
            if (roundTimer <= 0f)
            {
                EndRound();
            }
        }

        // Auto-advance if all enemies are cleared
        if (activeEnemies.Count == 0 && isLevelActive)
        {
            EndRound();
        }

        UpdateHUD();
    }

    private void EndRound()
    {
        isLevelActive = false;
        Debug.Log("Round ended.");
        // trigger next level or results here
    }

    private void SpawnEnemies(LevelData level)
    {
        for (int i = 0; i < level.smallCount; i++) SpawnEnemy("enemy_small");
        for (int i = 0; i < level.mediumCount; i++) SpawnEnemy("enemy_medium_shotgun");
        for (int i = 0; i < level.bomberCount; i++) SpawnEnemy("enemy_medium_bomber");
        for (int i = 0; i < level.largeCount; i++) SpawnEnemy("enemy_large");
        for (int i = 0; i < level.bossCount; i++) SpawnEnemy("boss_bartender");
    }

    private void SpawnEnemy(string enemyID)
    {
        if (!EnemyManager.enemyDatabase.ContainsKey(enemyID))
        {
            Debug.LogWarning("Enemy ID not found: " + enemyID);
            return;
        }

        EnemyData data = EnemyManager.enemyDatabase[enemyID];
        GameObject prefab = EnemyManager.GetPrefabForEnemy(enemyID);
        if (prefab == null)
        {
            Debug.LogWarning("No prefab assigned for enemy: " + enemyID);
            return;
        }

        GameObject enemy = Instantiate(prefab, GetSpawnPoint(), Quaternion.identity);
        enemy.GetComponent<EnemyController>().Setup(enemyID);
        RegisterEnemy(enemy);

        Debug.Log("Spawned enemy (fallback): " + data.displayName + " (" + enemyID + ")");
    }

    private Vector3 GetSpawnPoint()
    {
        return new Vector3(Random.Range(-5f, 5f), Random.Range(-3f, 3f), 0f);
    }

    public void RegisterEnemy(GameObject enemy)
    {
        if (!activeEnemies.Contains(enemy))
        {
            activeEnemies.Add(enemy);
        }
    }

    //  Increment customers served when enemy is removed
    public void RegisterCustomerServed(GameObject enemy)
    {
        if (activeEnemies.Contains(enemy))
        {
            activeEnemies.Remove(enemy);
        }

        customersServed++;
        UpdateHUD();
    }

    private void UpdateHUD()
    {
        if (timerText != null)
            timerText.text = $"Time: {(roundTimer > 0 ? Mathf.Ceil(roundTimer) : 0)}s";

        if (levelText != null && currentLevel != null)
            levelText.text = $"Level: {currentLevel.levelID}";

        if (customersText != null && currentLevel != null)
            customersText.text = $"Customers Served: {customersServed}/{currentLevel.customersToServe}";
    }
}
