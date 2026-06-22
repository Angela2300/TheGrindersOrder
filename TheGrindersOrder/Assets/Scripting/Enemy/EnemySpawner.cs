using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
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

        GameObject enemyObj = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

        EnemyController controller = enemyObj.GetComponent<EnemyController>();
        if (controller != null)
        {
            controller.manager = Object.FindFirstObjectByType<EnemyManager>();
            controller.Setup(enemyId);
        }

        Object.FindFirstObjectByType<EnemyManager>().RegisterEnemy(enemyObj);

        Debug.Log("Spawning enemy: " + enemyId + " at " + spawnPoints[spawnIndex].name);
    }


    public IEnumerator SpawnWave(LevelData level)
    {
        int spawnIndex = 0;

        for (int i = 0; i < level.smallCount; i++)
        {
            SpawnEnemy("enemy_small", spawnIndex++ % spawnPoints.Length);
            yield return new WaitForSeconds(1f);
        }

        for (int i = 0; i < level.mediumCount; i++)
        {
            SpawnEnemy("enemy_medium", spawnIndex++ % spawnPoints.Length);
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
