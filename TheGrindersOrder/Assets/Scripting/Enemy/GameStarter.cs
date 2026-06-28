using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStarter : MonoBehaviour
{
    private LevelManager lm;
    private int currentLevelIndex = 0;

    [SerializeField] private string MainMenu;
    [SerializeField] private string MainGame;

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


    public void ResetMainGameScene()
    {

        if (string.IsNullOrEmpty(MainGame))
        {
            Debug.LogError("Target scene name is empty! Please assign it in the Inspector.");
            return;
        }

        SceneManager.LoadScene(MainGame);
        Time.timeScale = 1f;
    }

    public void BacktoMainMenuScene()
    {

        if (string.IsNullOrEmpty(MainMenu))
        {
            Debug.LogError("Target scene name is empty! Please assign it in the Inspector.");
            return;
        }

        SceneManager.LoadScene(MainMenu);
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