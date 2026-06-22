using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class EnemyManager : MonoBehaviour
{
    public static Dictionary<string, EnemyData> enemyDatabase = new Dictionary<string, EnemyData>();

    private void Awake()
    {
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

        string[] lines = File.ReadAllLines(path);

        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i];
            if (string.IsNullOrWhiteSpace(line)) continue;

            string[] values = line.Split(',');
            if (values.Length < 17) continue;

            string id = values[0].Trim().ToLower();

            EnemyData data = new EnemyData
            {
                enemyID = id,
                displayName = values[1].Trim(),
                role = values[2].Trim(),
                attackType = values[3].Trim(),
                damageHearts = float.Parse(values[4]),
                damageLives = int.Parse(values[5]),
                health = int.Parse(values[6]),
                moveSpeedTier = values[7].Trim(),
                moveSpeedValue = float.Parse(values[8]),
                rangeTier = values[9].Trim(),
                followsPlayer = values[10].Trim().ToLower() == "true",
                meatDropAmt = int.Parse(values[11]),
                coinDropAmt = int.Parse(values[12]),
                weaponId = values[13].Trim().ToLower(),
                spawnCount = string.IsNullOrWhiteSpace(values[15]) ? 0 : int.Parse(values[15]),
                isBomber = values[16].Trim().ToLower() == "true"
            };

            if (!enemyDatabase.ContainsKey(id))
            {
                enemyDatabase.Add(id, data);
                Debug.Log("Loaded enemy ID: [" + id + "]");
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
}
