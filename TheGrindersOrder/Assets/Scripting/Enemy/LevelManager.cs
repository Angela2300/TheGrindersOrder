using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public LevelData currentLevel;
    private int customersServed = 0;
    private bool weaponDropped = false;

    public GameObject weaponPrefab;

    private void Start()
    {
        if (LevelLoader.levels.Count > 0)
        {
            StartLevel(LevelLoader.levels[0]);
        }
        else
        {
            Debug.LogError("No levels loaded from LevelLoader!");
        }
    }

    public void RegisterCustomerServed(GameObject enemyObj)
    {
        customersServed++;
        Debug.Log("Customer served: " + customersServed);

        if (!weaponDropped && customersServed >= currentLevel.customersToServe)
        {
            if (weaponPrefab != null)
            {
                Instantiate(weaponPrefab, enemyObj.transform.position, Quaternion.identity);
                Debug.Log("Weapon dropped at last enemy position.");
            }
            else
            {
                Debug.LogWarning("No weapon prefab assigned in LevelManager!");
            }

            weaponDropped = true;
            Debug.Log("Level " + currentLevel.levelID + " complete!");
        }
    }

    public void StartLevel(LevelData level)
    {
        currentLevel = level;
        customersServed = 0;
        weaponDropped = false;
        Debug.Log("Starting Level " + currentLevel.levelID);

        EnemySpawner spawner = Object.FindFirstObjectByType<EnemySpawner>();
        if (spawner == null)
        {
            Debug.LogError("EnemySpawner not found in scene!");
            return;
        }

        StartCoroutine(spawner.SpawnWave(currentLevel));
    }
}
