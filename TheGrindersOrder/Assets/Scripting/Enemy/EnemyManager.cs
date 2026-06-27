using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static Dictionary<string, EnemyData> enemyDatabase = new Dictionary<string, EnemyData>();

    [Header("CSV")]
    public TextAsset enemiesCSV; // Drag Enemies.csv here

    [Header("Prefab References")]
    public GameObject smallPrefab;
    public GameObject mediumShotgunPrefab;
    public GameObject mediumBomberPrefab;
    public GameObject largePrefab;
    public GameObject bossPrefab;

    private static EnemyManager instance;

    private void Awake()
    {
        instance = this;
        LoadEnemyDatabase();
    }

    private void LoadEnemyDatabase()
    {
        enemyDatabase.Clear();

        // Do not use Application.dataPath here.
        // It works in Unity Editor but can fail in Build & Run.
        if (enemiesCSV == null)
        {
            Debug.LogError("Enemies CSV is not assigned in EnemyManager!");
            return;
        }

        string content = enemiesCSV.text;

        string[] lines = content.Split(
            new[] { "\r\n", "\r", "\n" },
            System.StringSplitOptions.RemoveEmptyEntries
        );

        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = lines[i].Split(',');

            if (values.Length < 17)
            {
                Debug.LogWarning("Skipped bad enemy CSV row: " + lines[i]);
                continue;
            }

            string id = values[0].Trim().ToLower();

            EnemyData data = new EnemyData();

            data.enemyID = id;
            data.displayName = values[1].Trim();
            data.role = values[2].Trim();
            data.attackType = values[3].Trim();
            data.damageHearts = float.Parse(values[4]);
            data.attackRange = float.Parse(values[5]);
            data.damageLives = int.Parse(values[6]);
            data.health = int.Parse(values[7]);
            data.moveSpeedTier = values[8].Trim();
            data.moveSpeedValue = float.Parse(values[9]);
            data.rangeTier = values[10].Trim();
            data.followsPlayer = values[11].Trim().ToLower() == "true";
            data.meatDropAmt = int.Parse(values[12]);
            data.coinDropAmt = int.Parse(values[13]);
            data.weaponId = values[14].Trim().ToLower();

            data.spawnCount = 0;
            data.isBomber = false;

            for (int j = 15; j < values.Length; j++)
            {
                string value = values[j].Trim().ToLower();

                if (int.TryParse(value, out int spawn))
                {
                    data.spawnCount = spawn;
                }

                if (value == "true" || value == "false")
                {
                    data.isBomber = value == "true";
                }
            }

            if (!enemyDatabase.ContainsKey(id))
            {
                enemyDatabase.Add(id, data);
                Debug.Log("Loaded enemy ID: " + id);
            }
        }
    }

    public void RegisterEnemy(GameObject enemy)
    {
        EnemyController controller = enemy.GetComponent<EnemyController>();

        if (controller != null)
        {
            controller.manager = this;
        }
    }

    public static GameObject GetPrefabForEnemy(string enemyID)
    {
        if (instance == null) return null;

        switch (enemyID)
        {
            case "enemy_small":
                return instance.smallPrefab;

            case "enemy_medium_shotgun":
                return instance.mediumShotgunPrefab;

            case "enemy_medium_bomber":
                return instance.mediumBomberPrefab;

            case "enemy_large":
                return instance.largePrefab;

            case "boss_bartender":
                return instance.bossPrefab;

            default:
                return null;
        }
    }
}

//using System.Collections.Generic;
//using UnityEngine;
//using System.IO;

//public class EnemyManager : MonoBehaviour
//{
//    public static Dictionary<string, EnemyData> enemyDatabase = new Dictionary<string, EnemyData>();

//    // Prefab references
//    public GameObject smallPrefab;
//    public GameObject mediumShotgunPrefab;
//    public GameObject mediumBomberPrefab;
//    public GameObject largePrefab;
//    public GameObject bossPrefab;

//    private static EnemyManager instance;

//    private void Awake()
//    {
//        instance = this;
//        LoadEnemyDatabase();
//    }

//    private void LoadEnemyDatabase()
//    {
//        enemyDatabase.Clear();
//        string path = Application.dataPath + "/Scripting/CSVs/Enemies.csv";

//        if (!File.Exists(path))
//        {
//            Debug.LogError("Enemies.csv not found at " + path);
//            return;
//        }

//        // Open with FileShare.ReadWrite to prevent Sharing Violation errors
//        using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
//        using (var reader = new StreamReader(stream))
//        {
//            string content = reader.ReadToEnd();
//            string[] lines = content.Split(new[] { "\r\n", "\r", "\n" }, System.StringSplitOptions.RemoveEmptyEntries);

//            // Start from 1 to skip the header row
//            for (int i = 1; i < lines.Length; i++)
//            {
//                string[] values = lines[i].Split(',');
//                if (values.Length < 17) continue;

//                string id = values[0].Trim().ToLower();

//                // Create and populate the object
//                EnemyData data = new EnemyData();
//                data.enemyID = id;
//                data.displayName = values[1].Trim();
//                data.role = values[2].Trim();
//                data.attackType = values[3].Trim();
//                data.damageHearts = float.Parse(values[4]);
//                data.attackRange = float.Parse(values[5]);
//                data.damageLives = int.Parse(values[6]);
//                data.health = int.Parse(values[7]);
//                data.moveSpeedTier = values[8].Trim();
//                data.moveSpeedValue = float.Parse(values[9]);
//                data.rangeTier = values[10].Trim();
//                data.followsPlayer = values[11].Trim().ToLower() == "true";
//                data.meatDropAmt = int.Parse(values[12]);
//                data.coinDropAmt = int.Parse(values[13]);
//                data.weaponId = values[14].Trim().ToLower();
//                data.spawnCount = string.IsNullOrWhiteSpace(values[15]) ? 0 : int.Parse(values[15]);
//                data.isBomber = values[16].Trim().ToLower() == "true";

//                if (!enemyDatabase.ContainsKey(id))
//                {
//                    enemyDatabase.Add(id, data);
//                    Debug.Log("Loaded enemy ID: [" + id + "]");
//                }
//            }
//        }
//    }

//    public void RegisterEnemy(GameObject enemy)
//    {
//        var controller = enemy.GetComponent<EnemyController>();
//        if (controller != null)
//        {
//            controller.manager = this;
//        }
//    }

//    public static GameObject GetPrefabForEnemy(string enemyID)
//    {
//        if (instance == null) return null;

//        switch (enemyID)
//        {
//            case "enemy_small": return instance.smallPrefab;
//            case "enemy_medium_shotgun": return instance.mediumShotgunPrefab;
//            case "enemy_medium_bomber": return instance.mediumBomberPrefab;
//            case "enemy_large": return instance.largePrefab;
//            case "boss_bartender": return instance.bossPrefab;
//            default: return null;
//        }
//    }
//}