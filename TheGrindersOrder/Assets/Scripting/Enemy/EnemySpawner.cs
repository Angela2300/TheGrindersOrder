//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//[System.Serializable]
//public class LevelConfig
//{
//    public string levelName = "Level 1";

//    public float timeLimit = 30f;
//    public bool isUnlimitedTime = false;

//    public int quota = 3;

//    public float spawnInterval = 2f;
//    public GameObject[] humanPrefabs;
//    public Transform[] spawnPoints;
//}

//public class EnemySpawner : MonoBehaviour
//{
//    public List<LevelConfig> levels = new List<LevelConfig>();

//    public Transform[] defaultSpawnPoints;
//    public int maxActiveHumans = 10;

//    private int currentLevelIndex = 0;
//    private int currentDrinksServed = 0;
//    private float currentTimeRemaining = 0f;

//    private readonly List<GameObject> activeHumans = new List<GameObject>();
//    private Coroutine spawnRoutine;
//    private Coroutine timerRoutine;
//    private bool levelActive = false;

//    public delegate void LevelEvent(int levelIndex);
//    public static event LevelEvent OnLevelStart;
//    public static event LevelEvent OnLevelComplete;
//    public static event LevelEvent OnLevelFailed;
//    public static event LevelEvent OnRunReset;

//    public delegate void HumanEvent(GameObject human);
//    public static event HumanEvent OnHumanHarvested;

//    public int CurrentLevelIndex => currentLevelIndex;
//    public int CurrentDrinksServed => currentDrinksServed;
//    public float CurrentTimeRemaining => currentTimeRemaining;
//    public LevelConfig CurrentLevelConfig => (currentLevelIndex >= 0 && currentLevelIndex < levels.Count) ? levels[currentLevelIndex] : null;


//    private void Reset()
//    {
//        levels = new List<LevelConfig>
//        {
//            new LevelConfig { levelName = "Level 1",    timeLimit = 30f,  quota = 3 },
//            new LevelConfig { levelName = "Level 2",    timeLimit = 60f,  quota = 5 },
//            new LevelConfig { levelName = "Level 3",    timeLimit = 90f,  quota = 8 },
//            new LevelConfig { levelName = "Level 4",    timeLimit = 120f, quota = 12 },
//            new LevelConfig { levelName = "Boss Level", timeLimit = 0f,   quota = 1, isUnlimitedTime = true },
//        };
//    }

//    private void Start()
//    {
//        StartLevel(0);
//    }

//    public void StartLevel(int index)
//    {
//        if (index < 0 || index >= levels.Count)
//        {
//            levelActive = false;
//            return;
//        }

//        StopSpawning();
//        ClearActiveHumans();

//        currentLevelIndex = index;
//        LevelConfig config = levels[index];
//        currentDrinksServed = 0;
//        currentTimeRemaining = config.timeLimit;
//        levelActive = true;

//        OnLevelStart?.Invoke(currentLevelIndex);

//        spawnRoutine = StartCoroutine(SpawnRoutine(config));

//        if (!config.isUnlimitedTime)
//        {
//            timerRoutine = StartCoroutine(TimerRoutine(config));
//        }
//    }

//    private IEnumerator SpawnRoutine(LevelConfig config)
//    {
//        while (levelActive)
//        {
//            if (activeHumans.Count < maxActiveHumans)
//            {
//                SpawnHuman(config);
//            }
//            yield return new WaitForSeconds(config.spawnInterval);
//        }
//    }

//    private void SpawnHuman(LevelConfig config)
//    {
//        if (config.humanPrefabs == null || config.humanPrefabs.Length == 0) return;

//        Transform[] points = (config.spawnPoints != null && config.spawnPoints.Length > 0)
//            ? config.spawnPoints
//            : defaultSpawnPoints;

//        if (points == null || points.Length == 0) return;

//        GameObject prefab = config.humanPrefabs[Random.Range(0, config.humanPrefabs.Length)];
//        Transform spawnPoint = points[Random.Range(0, points.Length)];

//        GameObject human = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
//        activeHumans.Add(human);

//        //HumanTarget target = human.GetComponent<HumanTarget>();
//        //if (target != null)
//        //{
//        //    target.OnHarvested += HandleHumanHarvested;
//        //    target.OnRemoved += HandleHumanRemoved;
//        //}
//    }

//    private void HandleHumanHarvested(GameObject human)
//    {
//        activeHumans.Remove(human);
//        OnHumanHarvested?.Invoke(human);
//    }

//    private void HandleHumanRemoved(GameObject human)
//    {
//        activeHumans.Remove(human);
//    }

//    private IEnumerator TimerRoutine(LevelConfig config)
//    {
//        while (currentTimeRemaining > 0f && levelActive)
//        {
//            currentTimeRemaining -= Time.deltaTime;
//            yield return null;
//        }

//        if (levelActive && currentDrinksServed < config.quota)
//        {
//            FailLevel();
//        }
//    }

//    private void CompleteLevel()
//    {
//        levelActive = false;
//        StopSpawning();
//        OnLevelComplete?.Invoke(currentLevelIndex);
//        StartLevel(currentLevelIndex + 1);
//    }

//    private void FailLevel()
//    {
//        levelActive = false;
//        StopSpawning();
//        OnLevelFailed?.Invoke(currentLevelIndex);
//        OnRunReset?.Invoke(currentLevelIndex);
//        StartLevel(0);
//    }

//    private void StopSpawning()
//    {
//        if (spawnRoutine != null) StopCoroutine(spawnRoutine);
//        if (timerRoutine != null) StopCoroutine(timerRoutine);
//    }

//    private void ClearActiveHumans()
//    {
//        foreach (var h in activeHumans)
//        {
//            if (h != null) Destroy(h);
//        }
//        activeHumans.Clear();
//    }

//    public void ServeDrink()
//    {
//        if (!levelActive) return;

//        currentDrinksServed++;

//        LevelConfig config = CurrentLevelConfig;
//        if (config != null && currentDrinksServed >= config.quota)
//        {
//            CompleteLevel();
//        }
//    }

//    public void RegisterHarvest(GameObject human)
//    {
//        HandleHumanHarvested(human);
//    }
//}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelConfig
{
    public string levelName = "Level 1";
    public float timeLimit = 30f;
    public bool isUnlimitedTime = false;
    public int quota = 3;
    public float spawnInterval = 2f;
    public GameObject[] humanPrefabs;
    public Transform[] spawnPoints;
}

public class EnemySpawner : MonoBehaviour
{
    [Header("Level Configuration")]
    public List<LevelConfig> levels = new List<LevelConfig>();
    public Transform[] defaultSpawnPoints;
    public int maxActiveHumans = 10;
    public GameObject enemyPrefab;

    private int currentLevelIndex = 0;
    private int currentDrinksServed = 0;
    private float currentTimeRemaining = 0f;

    private readonly List<GameObject> activeHumans = new List<GameObject>();
    private Coroutine spawnRoutine;
    private Coroutine timerRoutine;
    private bool levelActive = false;

    // Events
    public delegate void LevelEvent(int levelIndex);
    public static event LevelEvent OnLevelStart;
    public static event LevelEvent OnLevelComplete;
    public static event LevelEvent OnLevelFailed;
    public static event LevelEvent OnRunReset;

    private void Start()
    {
        StartLevel(0);
    }

    public void StartLevel(int index)
    {
        if (index < 0 || index >= levels.Count)
        {
            levelActive = false;
            return;
        }

        StopSpawning();
        ClearActiveHumans();

        currentLevelIndex = index;
        LevelConfig config = levels[index];
        currentDrinksServed = 0;
        currentTimeRemaining = config.timeLimit;
        levelActive = true;

        OnLevelStart?.Invoke(currentLevelIndex);
        spawnRoutine = StartCoroutine(SpawnRoutine(config));

        if (!config.isUnlimitedTime)
            timerRoutine = StartCoroutine(TimerRoutine(config));
    }

    // --- Spawning Logic ---

    //// Note: Ensure LevelData exists in your project with these fields
    //public void SpawnLevel(LevelData levelData)
    //{
    //    StartCoroutine(SpawnOverTime("enemy_small", levelData.smallCount));
    //    StartCoroutine(SpawnOverTime("enemy_medium_shotgun", levelData.mediumCount));
    //    StartCoroutine(SpawnOverTime("enemy_medium_bomber", levelData.bomberCount));
    //    StartCoroutine(SpawnOverTime("enemy_large", levelData.largeCount));
    //    StartCoroutine(SpawnOverTime("boss_bartender", levelData.bossCount));
    //}
    public void SpawnLevel(List<string> enemyIDsToSpawn)
    {
        foreach (string id in enemyIDsToSpawn)
        {
            // Now only passes the ID string
            StartCoroutine(SpawnOverTime(id));
        }
    }

    public void SpawnEnemyByID(string enemyID)
    {
        StartCoroutine(SpawnOverTime(enemyID));
    }

    IEnumerator SpawnOverTime(string enemyID)
    {
    
        if (!EnemyManager.enemyDatabase.TryGetValue(enemyID, out EnemyData data))
        {
            Debug.LogError($"Cannot spawn {enemyID}: Not found in database!");
            yield break;
        }


        int countToSpawn = data.spawnCount;

        for (int i = 0; i < countToSpawn; i++)
        {
            Transform spawn = defaultSpawnPoints[Random.Range(0, defaultSpawnPoints.Length)];
            GameObject enemy = Instantiate(enemyPrefab, spawn.position, Quaternion.identity);

            var controller = enemy.GetComponent<EnemyController>();
            if (controller != null)
            {
                controller.Setup(enemyID);
            }

            yield return new WaitForSeconds(Random.Range(0.5f, 1.0f));
        }
    }

    private IEnumerator SpawnRoutine(LevelConfig config)
    {
        while (levelActive)
        {
            if (activeHumans.Count < maxActiveHumans) SpawnHuman(config);
            yield return new WaitForSeconds(config.spawnInterval);
        }
    }

    private void SpawnHuman(LevelConfig config)
    {
        Transform[] points = (config.spawnPoints != null && config.spawnPoints.Length > 0) ? config.spawnPoints : defaultSpawnPoints;
        GameObject prefab = config.humanPrefabs[Random.Range(0, config.humanPrefabs.Length)];
        GameObject human = Instantiate(prefab, points[Random.Range(0, points.Length)].position, Quaternion.identity);
        activeHumans.Add(human);
    }

    // --- Timer and Fail Logic ---

    private IEnumerator TimerRoutine(LevelConfig config)
    {
        while (currentTimeRemaining > 0f && levelActive)
        {
            currentTimeRemaining -= Time.deltaTime;
            yield return null;
        }

        if (levelActive && currentDrinksServed < config.quota)
        {
            FailLevel();
        }
    }

    private void FailLevel()
    {
        levelActive = false;
        StopSpawning();
        OnLevelFailed?.Invoke(currentLevelIndex);
        OnRunReset?.Invoke(currentLevelIndex);
        StartLevel(0);
    }

    private void StopSpawning()
    {
        if (spawnRoutine != null) StopCoroutine(spawnRoutine);
        if (timerRoutine != null) StopCoroutine(timerRoutine);
    }

    private void ClearActiveHumans()
    {
        foreach (var h in activeHumans) if (h != null) Destroy(h);
        activeHumans.Clear();
    }
}

