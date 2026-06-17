using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour

{
    public TextAsset csvFile;
    public List<LevelData> levels = new List<LevelData>();

    void Awake()
    {
        LoadCSV();
    }

    void LoadCSV()
    {
        string[] lines = csvFile.text.Split('\n');

        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i];
            if (string.IsNullOrWhiteSpace(line)) continue;

            string[] values = line.Split(',');

            LevelData level = new LevelData();

            level.levelID = int.Parse(values[0]);
            level.smallCount = int.Parse(values[1]);
            level.mediumCount = int.Parse(values[2]);
            level.bomberCount = int.Parse(values[3]);
            level.largeCount = int.Parse(values[4]);
            level.bossCount = int.Parse(values[5]);
            level.timeLimit = float.Parse(values[6]);
            level.customersToServe = int.Parse(values[7]);

            levels.Add(level);

            Debug.Log($"Loaded Level {level.levelID}: small={level.smallCount}");
        }
    }

    public LevelData GetLevel(int id)
    {
        return levels.Find(l => l.levelID == id);
    }
}









