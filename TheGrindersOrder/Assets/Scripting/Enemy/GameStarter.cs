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
        Time.timeScale = 1f;
    }


    public void ResetChosenScene()
    {

        if (string.IsNullOrEmpty(targetSceneName))
        {
            Debug.LogError("Target scene name is empty! Please assign it in the Inspector.");
            return;
        }

        SceneManager.LoadScene(targetSceneName);
        Time.timeScale = 1f;
    }

    private void StartLevel(int index)
    {
        LevelData level = LevelLoader.levels[index];
        lm.StartLevel(level);
        Debug.Log("Started Level " + (index + 1) + " for testing.");
        Time.timeScale = 1f;
    }
}