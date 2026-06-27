using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStarter : MonoBehaviour
{
    private LevelManager lm;
    private int currentLevelIndex = 0;

    [SerializeField] private string targetSceneName;

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

    public void ResetChosenScene()
    {
        // Checks if you forgot to type a scene name
        if (string.IsNullOrEmpty(targetSceneName))
        {
            Debug.LogError("Target scene name is empty! Please assign it in the Inspector.");
            return;
        }

        // Reloads the designated scene from scratch
        SceneManager.LoadScene(targetSceneName);
    }

    private void StartLevel(int index)
    {
        LevelData level = LevelLoader.levels[index];
        lm.StartLevel(level);
        Debug.Log("Started Level " + (index + 1) + " for testing.");
    }
}


