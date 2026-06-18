using UnityEngine;

public class EnemyController : MonoBehaviour

{
    public string enemyID;
    private EnemyData stats;
    private Transform target;

    private float attackTimer = 0f;
    public float attackCooldown = 1.5f; // Seconds between attacks

    // Mapping range tiers to distance values
    private float GetRangeValue(string tier)
    {
        return tier switch
        {
            "Short" => 1.5f,
            "Close" => 3.0f,
            "Explosion" => 4.0f,
            "Large" => 6.0f,
            "Largest" => 10.0f,
            _ => 2.0f // Default
        };
    }

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

        //GameObject playerObj = GameObject.Find("Player");
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

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
        if (stats == null || target == null) return;

        float distance = Vector3.Distance(transform.position, target.position);
        float attackRange = GetRangeValue(stats.rangeTier);

        //    if (float.TryParse(stats.moveSpeedValue, out float speed))
        //    {
        //        transform.position = Vector3.MoveTowards(
        //            transform.position,
        //            target.position,
        //            speed * Time.deltaTime
        //        );
        //    }
        //}

        // Movement logic
        if (distance > attackRange)
        {
            if (float.TryParse(stats.moveSpeedValue, out float speed))
            {
                transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            }
        }
        else
        {
            // Attack logic
            if (attackTimer <= 0)
            {
                //PerformAttack();
                attackTimer = attackCooldown;
            }
        }

        if (attackTimer > 0) attackTimer -= Time.deltaTime;

    }

    //void PerformAttack()
    //{
    //    // Find the PlayerStats component on the target
    //    PlayerStats player = target.GetComponent<PlayerStats>();
    //    if (player != null)
    //    {
    //        // Pass damage values from CSV
    //        player.TakeDamage((int)stats.damageHearts, (int)stats.damageLives);
    //        Debug.Log($"{stats.displayName} attacked for {stats.damageHearts} Hearts!");
    //    }
    //}


    void InitializeEnemy(EnemyData data)
    {
        gameObject.name = data.displayName;
        Debug.Log($"Enemy {data.displayName} initialized with {data.health} HP.");
    }
}
