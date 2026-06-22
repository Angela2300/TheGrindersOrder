using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LevelLoader : MonoBehaviour
{
    public static List<LevelData> levels = new List<LevelData>();

    void Awake()
    {
        LoadLevels();
    }

    private void LoadLevels()
    {
        levels.Clear();
        string path = Application.dataPath + "/Scripting/CSVs/Levels.csv"; 
        if (!File.Exists(path))
        {
            Debug.LogError("Levels.csv not found at " + path);
            return;
        }

        string[] lines = File.ReadAllLines(path);

        for (int i = 1; i < lines.Length; i++) // skip header
        {
            string[] values = lines[i].Split(',');
            if (values.Length < 8) continue;

            LevelData level = new LevelData
            {
                levelID = int.Parse(values[0]),
                smallCount = int.Parse(values[1]),
                mediumCount = int.Parse(values[2]),
                bomberCount = int.Parse(values[3]),
                largeCount = int.Parse(values[4]),
                bossCount = int.Parse(values[5]),
                timeLimit = int.Parse(values[6]),
                customersToServe = int.Parse(values[7])
            };

            levels.Add(level);
            Debug.Log("Loaded Level " + level.levelID);
        }
    }
}
