using UnityEngine;

public class EnemyController : MonoBehaviour

{
    public string enemyID;
    private EnemyData stats;
    private Transform target;

    //public void InitializeWithID(string id)
    //{
    //    enemyID = id;
    //    if (EnemyManager.enemyDatabase.TryGetValue(enemyID, out stats))
    //    {
    //        InitializeEnemy(stats);
    //    }
    //    else
    //    {
    //        Debug.LogError($"Enemy ID '{enemyID}' not found in database!");
    //    }
    //}

    public void Setup(string id)
    {
        enemyID = id;
        if (EnemyManager.enemyDatabase.TryGetValue(enemyID, out stats))
        {
            InitializeEnemy(stats);
        }
        else
        {
            Debug.LogError($"Database Error: Could not find ID '{enemyID}'");
        }
    }

    void Start()
    {
       
        if (EnemyManager.enemyDatabase.TryGetValue(enemyID, out stats))
        {
            InitializeEnemy(stats);
        }
        else
        {
            Debug.LogError($"Enemy ID '{enemyID}' not found in database!");
        }

        GameObject playerObj = GameObject.Find("Hi Im a Dummy Player");

        if (playerObj != null)
        {
            target = playerObj.transform;
        }
        else
        {
            Debug.LogError("Player object not found in scene!");
        }
    }

    void Update()
    {
        //if (target == null) return;
        if (stats == null || target == null) return;
    

        if (float.TryParse(stats.moveSpeedValue, out float speed))
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                target.position,
                speed * Time.deltaTime
            );
        }

    }

    void InitializeEnemy(EnemyData data)
    {
        gameObject.name = data.displayName;
        Debug.Log($"Enemy {data.displayName} initialized with {data.health} HP.");
    }
}
