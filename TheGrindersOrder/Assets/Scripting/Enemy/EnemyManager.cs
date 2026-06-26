//using System.Collections.Generic;
//using UnityEngine;
//using System.IO;

//public class EnemyManager : MonoBehaviour
//{
//    public static Dictionary<string, EnemyData> enemyDatabase = new Dictionary<string, EnemyData>();

//    // Prefab references (drag in Inspector)
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

//        string[] lines = File.ReadAllLines(path);

//        for (int i = 1; i < lines.Length; i++)
//        {
//            string line = lines[i];
//            if (string.IsNullOrWhiteSpace(line)) continue;

//            string[] values = line.Split(',');
//            if (values.Length < 17) continue;

//            string id = values[0].Trim().ToLower();

//            EnemyData data = new EnemyData
//            {
//                enemyID = id,
//                displayName = values[1].Trim(),
//                role = values[2].Trim(),
//                attackType = values[3].Trim(),
//                damageHearts = float.Parse(values[4]),
//                attackRange = float.Parse(values[5]),
//                damageLives = int.Parse(values[6]),
//                health = int.Parse(values[7]),
//                moveSpeedTier = values[8].Trim(),
//                moveSpeedValue = float.Parse(values[9]),
//                rangeTier = values[10].Trim(),
//                followsPlayer = values[11].Trim().ToLower() == "true",
//                meatDropAmt = int.Parse(values[12]),
//                coinDropAmt = int.Parse(values[13]),
//                weaponId = values[1].Trim().ToLower(),
//                spawnCount = string.IsNullOrWhiteSpace(values[15]) ? 0 : int.Parse(values[15]),
//                isBomber = values[16].Trim().ToLower() == "true"
//            };

//            if (!enemyDatabase.ContainsKey(id))
//            {
//                enemyDatabase.Add(id, data);
//                Debug.Log("Loaded enemy ID: [" + id + "]");
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
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class EnemyManager : MonoBehaviour
{
    public static Dictionary<string, EnemyData> enemyDatabase = new Dictionary<string, EnemyData>();

    // Prefab references
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
        string path = Application.dataPath + "/Scripting/CSVs/Enemies.csv";

        if (!File.Exists(path))
        {
            Debug.LogError("Enemies.csv not found at " + path);
            return;
        }

        // Open with FileShare.ReadWrite to prevent Sharing Violation errors
        using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        using (var reader = new StreamReader(stream))
        {
            string content = reader.ReadToEnd();
            string[] lines = content.Split(new[] { "\r\n", "\r", "\n" }, System.StringSplitOptions.RemoveEmptyEntries);

            // Start from 1 to skip the header row
            for (int i = 1; i < lines.Length; i++)
            {
                string[] values = lines[i].Split(',');
                if (values.Length < 17) continue;

                string id = values[0].Trim().ToLower();

                // Create and populate the object
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
                data.spawnCount = string.IsNullOrWhiteSpace(values[15]) ? 0 : int.Parse(values[15]);
                data.isBomber = values[16].Trim().ToLower() == "true";

                if (!enemyDatabase.ContainsKey(id))
                {
                    enemyDatabase.Add(id, data);
                    Debug.Log("Loaded enemy ID: [" + id + "]");
                }
            }
        }
    }

    public void RegisterEnemy(GameObject enemy)
    {
        var controller = enemy.GetComponent<EnemyController>();
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
            case "enemy_small": return instance.smallPrefab;
            case "enemy_medium_shotgun": return instance.mediumShotgunPrefab;
            case "enemy_medium_bomber": return instance.mediumBomberPrefab;
            case "enemy_large": return instance.largePrefab;
            case "boss_bartender": return instance.bossPrefab;
            default: return null;
        }
    }
}