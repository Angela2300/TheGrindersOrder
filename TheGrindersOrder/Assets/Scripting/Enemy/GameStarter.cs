using UnityEngine;

public class GameStarter : MonoBehaviour
{
    private LevelManager lm;
    private int currentLevelIndex = 0;

    void Start()
    {
        lm = Object.FindFirstObjectByType<LevelManager>();
        if (lm != null && LevelLoader.levels != null && LevelLoader.levels.Count > 0)
        {
            StartLevel(currentLevelIndex);
        }
        else
        {
            Debug.LogWarning("No LevelManager or Levels found!");
        }
    }

    void Update()
    {
        if (lm == null) return;

        // Check if all enemies are cleared
        if (lm.activeEnemies.Count == 0 && lm.isLevelActive)
        {
            currentLevelIndex++;
            if (currentLevelIndex < LevelLoader.levels.Count)
            {
                StartLevel(currentLevelIndex);
            }
            else
            {
                Debug.Log("All test levels completed!");
            }
        }
    }

    private void StartLevel(int index)
    {
        LevelData level = LevelLoader.levels[index];
        lm.StartLevel(level);
        Debug.Log("Started Level " + (index + 1) + " for testing.");
    }
}


