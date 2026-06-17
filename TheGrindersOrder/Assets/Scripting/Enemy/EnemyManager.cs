using UnityEngine; 
using System.Collections.Generic; 
public class EnemyManager : MonoBehaviour

{
    public TextAsset csvFile;

    public static Dictionary<string, EnemyData> enemyDatabase = new Dictionary<string, EnemyData>();

    void Awake()
    {
        Debug.Log("EnemyManager Awake is running");
        LoadCSV();
    }

    void LoadCSV()
    {
        if (csvFile == null)
        {
            Debug.LogError("CSV NOT ASSIGNED!");
            return;
        }

        Debug.Log("CSV loaded: " + csvFile.name);

        string[] lines = csvFile.text.Split('\n');

        for (int i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i])) continue;

            string[] row = lines[i].Split(',');

            EnemyData data = new EnemyData
            {
              
                enemyID = row[0].Trim(),
                displayName = row[1].Trim(),
                role = row[2].Trim(),
                damageHearts = float.Parse(row[4]),
                damageLives = float.Parse(row[5]),
                health = float.Parse(row[6]),
                moveSpeedTier = row[7].Trim(),
                moveSpeedValue = row[8].Trim(),
                rangeTier = row[9].Trim(),
                followsPlayer = bool.Parse(row[10]),
                meatDropAmt = row[11].Trim(),
                coinDropAmt = row[12].Trim(),
                weaponId = row[13].Trim(),
                spawnCount = int.Parse(row[14]),
                isBomber = bool.Parse(row[15])
            };

            enemyDatabase[data.enemyID] = data;

            Debug.Log("Loaded enemy: " + data.enemyID);
        }
    }
}
