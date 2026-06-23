using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;

public class StartMenuController : MonoBehaviour
{
    public void OnStartClick()
    {
        StartCoroutine(LoadSceneDelay("EnemySpawner"));
    }

    public void OpenOptions()
    {

        if (OptionsMenuController.Instance != null)
            OptionsMenuController.Instance.OpenOptions();
    }

    public void CloseOptions()
    {
        if (OptionsMenuController.Instance != null)
            OptionsMenuController.Instance.CloseOptions();
    }

    public void OnExitClick()
    {
        Application.Quit();
    }

    IEnumerator LoadSceneDelay(string sceneName)
    {
        yield return new WaitForSeconds(0.2f);
        SceneManager.LoadScene(sceneName);

        yield return null; // wait one frame so LevelManager initializes
        LevelManager lm = Object.FindFirstObjectByType<LevelManager>();
        if (lm != null && LevelLoader.levels != null && LevelLoader.levels.Count > 0)
        {
            lm.StartLevel(LevelLoader.levels[0]); // start first level
            Debug.Log("Game started: Level 1 running.");
        }
        else
        {
            Debug.LogWarning("LevelManager or LevelLoader not ready.");
        }
    }
}