using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public bool isLevelActive;
    private float roundTimer;

    public List<GameObject> activeEnemies = new List<GameObject>();

    public TMP_Text timerText;
    public TMP_Text levelText;
    public TMP_Text customersText;
    public TMP_Text meatinventorytext;

    private LevelData currentLevel;

    public GameObject Youdied;
    public GameObject RanOutOfTimeCanvas;
    public GameObject MoveOnNextLevelCanvas;
    public GameObject YouWonCanvas;

    private int customersServed = 0;
    private int meatCollected = 0;

    private void Awake()
    {
        Instance = this;
    }

    public void StartLevel(LevelData level)
    {
        //  Clear leftover enemies
        foreach (var enemy in activeEnemies)
        {
            if (enemy != null)
                Destroy(enemy);
        }
        activeEnemies.Clear();

        currentLevel = level;
        isLevelActive = true;
        roundTimer = level.timeLimit;

        customersServed = 0;

        EnemySpawner spawner = Object.FindFirstObjectByType<EnemySpawner>();
        if (spawner != null)
        {
            StartCoroutine(spawner.SpawnWave(level));
        }
        else
        {
            SpawnEnemies(level);
        }

        UpdateHUD();
    }

    private void Update()
    {
        if (!isLevelActive) return;

        if (roundTimer > 0)
        {
            roundTimer -= Time.deltaTime;
            if (roundTimer <= 0f)
            {
                RanOutOfTime();
            }
        }

        UpdateHUD();
    }

    // ---------------------------
    // MEAT COLLECTION
    // ---------------------------

    public void RegisterMeatCollected(int amount = 1)
    {
        meatCollected += amount;
        Debug.Log($"[LevelManager] Meat collected: +{amount}, total = {meatCollected}");
        UpdateHUD();
    }

    public void ResetMeatCollected()
    {
        meatCollected = 0;
        UpdateHUD();
    }

    public static void OnMeatSold(int meatCount)
    {
        if (Instance != null)
        {
            Instance.AddCustomersServed(meatCount);
            Instance.ResetMeatCollected();
        }
    }

    public void AddCustomersServed(int amount)
    {
        customersServed += amount;
        UpdateHUD();

        if (customersServed >= currentLevel.customersToServe)
        {
            MoveOnNextLevel();
        }
    }

    // ---------------------------
    // CANVAS METHODS
    // ---------------------------

    private void MoveOnNextLevel()
    {
        isLevelActive = false;
        if (MoveOnNextLevelCanvas != null)
            MoveOnNextLevelCanvas.SetActive(true);
        Time.timeScale = 0f; //pause
    }

    private void RanOutOfTime()
    {
        isLevelActive = false;
        if (RanOutOfTimeCanvas != null)
            RanOutOfTimeCanvas.SetActive(true);
        Time.timeScale = 0f; //pause
    }

    private void YouWon()
    {
        isLevelActive = false;
        if (YouWonCanvas != null)
            YouWonCanvas.SetActive(true);
        Time.timeScale = 0f; //pause
    }

    public void PlayerDied()
    {
        isLevelActive = false;
        if (Youdied != null)
            Youdied.SetActive(true);
        Time.timeScale = 0f; //pause
    }

    // ---------------------------
    // ENEMY SPAWNING
    // ---------------------------

    private void SpawnEnemies(LevelData level)
    {
        for (int i = 0; i < level.smallCount; i++) SpawnEnemy("enemy_small");
        for (int i = 0; i < level.smallCountdrop; i++) SpawnEnemy("enemy_small_drop");
        for (int i = 0; i < level.mediumCount; i++) SpawnEnemy("enemy_medium_shotgun");
        for (int i = 0; i < level.mediumCountdrop; i++) SpawnEnemy("enemy_medium_shotgun_drop");
        for (int i = 0; i < level.bomberCount; i++) SpawnEnemy("enemy_medium_bomber");
        for (int i = 0; i < level.bomberCountdrop; i++) SpawnEnemy("enemy_medium_bomberdrop");
        for (int i = 0; i < level.largeCount; i++) SpawnEnemy("enemy_large");
        for (int i = 0; i < level.largeCountdrop; i++) SpawnEnemy("enemy_large_drop");
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

        Debug.Log("Spawned enemy: " + data.displayName + " (" + enemyID + ")");
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

    public void RegisterCustomerServed(GameObject enemy)
    {
        if (activeEnemies.Contains(enemy))
        {
            activeEnemies.Remove(enemy);
        }
    }

    // ---------------------------
    // NEXT LEVEL BUTTON
    // ---------------------------

    public void LoadNextLevel()
    {
        int currentIndex = LevelLoader.levels.IndexOf(currentLevel);

        if (currentIndex >= 0 && currentIndex < LevelLoader.levels.Count - 1)
        {
            StartLevel(LevelLoader.levels[currentIndex + 1]);
            Time.timeScale = 1f;
        }
    }

    // ---------------------------
    // FINAL BOSS VICTORY
    // ---------------------------
    public void RegisterBossKilled(string enemyID)
    {
        int currentIndex = LevelLoader.levels.IndexOf(currentLevel);

        // Only trigger victory if this is the last level AND the boss enemy is killed
        if (currentIndex == LevelLoader.levels.Count - 1 && enemyID == "boss_bartender")
        {
            YouWon();
        }
    }


    // ---------------------------
    // HUD UPDATE
    // ---------------------------

    private void UpdateHUD()
    {
        if (timerText != null)
            timerText.text = $"Time: {(roundTimer > 0 ? Mathf.Ceil(roundTimer) : 0)}s";

        if (levelText != null && currentLevel != null)
            levelText.text = $"Level: {currentLevel.levelID}";

        if (customersText != null && currentLevel != null)
            customersText.text = $"Meat Sold: {customersServed}/{currentLevel.customersToServe}";

        if (meatinventorytext != null)
            meatinventorytext.text = $"Meat Collected: {meatCollected}";
    }
}