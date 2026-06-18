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
        //SoundEffectPlayer.Instance.PlaySound(SoundEffectPlayer.Instance.buttonClickSFX);

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
    }
}