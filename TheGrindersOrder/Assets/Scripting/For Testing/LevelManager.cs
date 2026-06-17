using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public LevelLoader levelLoader;
    public EnemySpawner enemySpawner;

    public int startLevel = 1;

    void Start()
    {
        StartLevel(startLevel);
    }

    void StartLevel(int levelID)
    {
        LevelData level = levelLoader.GetLevel(levelID);

        if (level == null)
        {
            Debug.LogError("Level not found!");
            return;
        }

        Debug.Log("Starting Level " + levelID);

        enemySpawner.StartLevel(levelID);
    }
}


