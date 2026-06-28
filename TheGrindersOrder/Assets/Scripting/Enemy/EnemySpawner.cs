using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public Transform[] spawnPoints;

    public void SpawnEnemy(string enemyId, int spawnIndex)
    {
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("No spawn points assigned!");
            return;
        }

        spawnIndex = Mathf.Clamp(spawnIndex, 0, spawnPoints.Length - 1);
        Vector3 spawnPos = spawnPoints[spawnIndex].position;

        GameObject prefab = EnemyManager.GetPrefabForEnemy(enemyId);

        if (prefab == null)
        {
            Debug.LogWarning("No prefab found for enemy ID: " + enemyId);
            return;
        }

        GameObject enemyObj = Instantiate(prefab, spawnPos, Quaternion.identity);

        EnemyController controller = enemyObj.GetComponent<EnemyController>();

        if (controller != null)
        {
            controller.manager = Object.FindFirstObjectByType<EnemyManager>();
            controller.Setup(enemyId);
        }

        EnemyManager enemyManager = Object.FindFirstObjectByType<EnemyManager>();

        if (enemyManager != null)
        {
            enemyManager.RegisterEnemy(enemyObj);
        }

        LevelManager levelManager = Object.FindFirstObjectByType<LevelManager>();

        if (levelManager != null)
        {
            levelManager.RegisterEnemy(enemyObj);
        }

        Debug.Log("Spawning enemy: " + enemyId + " at " + spawnPoints[spawnIndex].name);
    }


    public IEnumerator SpawnWave(LevelData level)
    {
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("SpawnWave stopped: No spawn points assigned!");
            yield break;
        }

        Debug.Log("SpawnWave started");

        int spawnIndex = 0;

        for (int i = 0; i < level.smallCount; i++)
        {
            SpawnEnemy("enemy_small", spawnIndex++ % spawnPoints.Length);
            yield return new WaitForSeconds(1f);
        }

        for (int i = 0; i < level.mediumCount; i++)
        {
            SpawnEnemy("enemy_medium_shotgun", spawnIndex++ % spawnPoints.Length);
            yield return new WaitForSeconds(1f);
        }

        for (int i = 0; i < level.bomberCount; i++)
        {
            SpawnEnemy("enemy_medium_bomber", spawnIndex++ % spawnPoints.Length);
            yield return new WaitForSeconds(1f);
        }

        for (int i = 0; i < level.largeCount; i++)
        {
            SpawnEnemy("enemy_large", spawnIndex++ % spawnPoints.Length);
            yield return new WaitForSeconds(1f);
        }

        for (int i = 0; i < level.bossCount; i++)
        {
            SpawnEnemy("boss_bartender", spawnIndex++ % spawnPoints.Length);
            yield return new WaitForSeconds(2f);
        }
    }
}
//using UnityEngine;
//using System.Collections;

//public class EnemySpawner : MonoBehaviour
//{
//    public Transform[] spawnPoints; // drag spawn point objects here in Inspector

//    public void SpawnEnemy(string enemyId, int spawnIndex)
//    {
//        if (spawnPoints == null || spawnPoints.Length == 0)
//        {
//            Debug.LogError("No spawn points assigned!");
//            return;
//        }

//        spawnIndex = Mathf.Clamp(spawnIndex, 0, spawnPoints.Length - 1);
//        Vector3 spawnPos = spawnPoints[spawnIndex].position;

//        GameObject prefab = EnemyManager.GetPrefabForEnemy(enemyId);
//        if (prefab == null)
//        {
//            Debug.LogWarning("No prefab found for enemy ID: " + enemyId);
//            return;
//        }

//        GameObject enemyObj = Instantiate(prefab, spawnPos, Quaternion.identity);

//        EnemyController controller = enemyObj.GetComponent<EnemyController>();
//        if (controller != null)
//        {
//            controller.manager = Object.FindFirstObjectByType<EnemyManager>();
//            controller.Setup(enemyId);
//        }

//        // Register with EnemyManager
//        Object.FindFirstObjectByType<EnemyManager>().RegisterEnemy(enemyObj);

//        // Register with LevelManager so timer/customers logic works
//        Object.FindFirstObjectByType<LevelManager>().RegisterEnemy(enemyObj);

//        Debug.Log("Spawning enemy: " + enemyId + " at " + spawnPoints[spawnIndex].name);
//    }

//    public IEnumerator SpawnWave(LevelData level)
//    {
//        int spawnIndex = 0;

//        for (int i = 0; i < level.smallCount; i++)
//        {
//            SpawnEnemy("enemy_small", spawnIndex++ % spawnPoints.Length);
//            yield return new WaitForSeconds(1f);
//        }

//        for (int i = 0; i < level.mediumCount; i++)
//        {
//            SpawnEnemy("enemy_medium_shotgun", spawnIndex++ % spawnPoints.Length);
//            yield return new WaitForSeconds(1f);
//        }

//        for (int i = 0; i < level.bomberCount; i++)
//        {
//            SpawnEnemy("enemy_medium_bomber", spawnIndex++ % spawnPoints.Length);
//            yield return new WaitForSeconds(1f);
//        }

//        for (int i = 0; i < level.largeCount; i++)
//        {
//            SpawnEnemy("enemy_large", spawnIndex++ % spawnPoints.Length);
//            yield return new WaitForSeconds(1f);
//        }

//        for (int i = 0; i < level.bossCount; i++)
//        {
//            SpawnEnemy("boss_bartender", spawnIndex++ % spawnPoints.Length);
//            yield return new WaitForSeconds(2f);
//        }
//    }
//}


