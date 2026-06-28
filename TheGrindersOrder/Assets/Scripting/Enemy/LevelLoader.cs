using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    public static List<LevelData> levels = new List<LevelData>();

    [Header("CSV")]
    public TextAsset levelsCSV;

    void Awake()
    {
        LoadLevels();
    }

    private void LoadLevels()
    {
        levels.Clear();

        if (levelsCSV == null)
        {
            Debug.LogError("Levels CSV is not assigned in LevelLoader!");
            return;
        }

        string[] lines = levelsCSV.text.Split(
            new[] { "\r\n", "\r", "\n" },
            System.StringSplitOptions.RemoveEmptyEntries
        );

        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = lines[i].Split(',');

            if (values.Length < 8)
            {
                Debug.LogWarning("Skipped bad level CSV row: " + lines[i]);
                continue;
            }

            LevelData level = new LevelData
            {
                levelID = int.Parse(values[0]),
                smallCount = int.Parse(values[1]),
                smallCountdrop = int.Parse(values[2]),
                mediumCount = int.Parse(values[3]),
                mediumCountdrop = int.Parse(values[4]),
                bomberCount = int.Parse(values[5]),
                bomberCountdrop = int.Parse(values[6]),
                largeCount = int.Parse(values[7]),
                largeCountdrop = int.Parse(values[8]),
                bossCount = int.Parse(values[9]),
                timeLimit = int.Parse(values[10]),
                customersToServe = int.Parse(values[11])
            };

            levels.Add(level);
            Debug.Log("Loaded Level " + level.levelID);
        }

        Debug.Log("Total levels loaded: " + levels.Count);
    }
}
