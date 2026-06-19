//using UnityEngine;

//public class EnemyController : MonoBehaviour
//{
//    public string enemyID;
//    private EnemyData stats;
//    private Transform target;

//    private float attackTimer = 0f;
//    public float attackCooldown = 1.5f;
//    private PlayerStats targetStats;

//    private float GetRangeValue(string tier)
//    {
//        return tier switch
//        {
//            "Short" => 1.5f,
//            "Close" => 3.0f,
//            "Explosion" => 4.0f,
//            "Large" => 6.0f,
//            "Largest" => 10.0f,
//            _ => 2.0f
//        };
//    }

//    public void Setup(string id)
//    {
//        enemyID = id;
//        if (EnemyManager.enemyDatabase.TryGetValue(enemyID, out stats))
//        {
//            InitializeEnemy(stats);
//        }
//        else
//        {
//            Debug.LogError($"Database Error: Could not find ID '{enemyID}'");
//        }
//    }
//    private bool hasHitPlayer = false;

//    void Start()
//    {
//        Debug.Log("EnemyController: Start() has been called!"); // FORCE A MESSAGE
//        // 1. Initialize stats from Database
//        if (EnemyManager.enemyDatabase.TryGetValue(enemyID, out stats))
//        {
//            InitializeEnemy(stats);
//        }
//        else
//        {
//            Debug.LogError($"Enemy ID '{enemyID}' not found in database!");
//        }

//        // 2. Find Player (Ensure the object in your scene is Tagged as "Player")
//        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
//        if (playerObj != null)
//        {
//            target = playerObj.transform;
//        }
//        else
//        {
//            Debug.LogError("Player object with tag 'Player' not found!");
//        }
//    }

//    void Update()
//    {
//        if (stats == null || target == null) return;

//        float distance = Vector3.Distance(transform.position, target.position);
//        float attackRange = GetRangeValue(stats.rangeTier);

//        // Stop moving if close
//        if (distance <= attackRange)
//        {
//            // NO MOVEMENT HERE = Enemy stops moving
//            if (attackTimer <= 0)
//            {
//                PerformAttack();
//                attackTimer = attackCooldown;
//            }
//        }
//        else
//        {
//            // Move towards target
//            if (float.TryParse(stats.moveSpeedValue, out float speed))
//            {
//                transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
//            }
//        }

//        if (attackTimer > 0) attackTimer -= Time.deltaTime;
//    }

//    void PerformAttack()
//    {
//        // Use OverlapSphere to find the player within attack range
//        float attackRange = GetRangeValue(stats.rangeTier);

//        // Changed from Physics.OverlapSphere -> Physics2D.OverlapCircleAll
//        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, attackRange);

//        foreach (var hitCollider in hitColliders)
//        {
//            PlayerStats player = hitCollider.GetComponent<PlayerStats>();
//            if (player != null)
//            {
//                int damageToDeal = GetDamageForEnemyType();
//                player.TakeDamage(damageToDeal);
//                Debug.Log($"SUCCESS: {gameObject.name} dealt {damageToDeal} damage to {hitCollider.name}!");
//                break; // Stop after hitting the player once
//            }
//        }
//        int GetDamageForEnemyType()
//        {
//            Debug.Log("Getting damage for ID: " + enemyID);

//            // Trim the string to remove accidental spaces and ignore case
//            string id = enemyID.Trim();

//            if (id.Equals("Small", System.StringComparison.OrdinalIgnoreCase)) return 1;
//            if (id.Equals("MediumV1", System.StringComparison.OrdinalIgnoreCase)) return 2;
//            if (id.Equals("Bomber", System.StringComparison.OrdinalIgnoreCase)) return 2;
//            if (id.Equals("Large", System.StringComparison.OrdinalIgnoreCase)) return 3;
//            if (id.Equals("Boss", System.StringComparison.OrdinalIgnoreCase)) return 4;

//            Debug.LogWarning("Enemy ID '" + id + "' not found! Returning 1 damage.");
//            return 1; // Default fallback
//        }

//        void OnDrawGizmosSelected()
//        {
//            if (stats != null)
//            {
//                Gizmos.color = Color.red;
//                Gizmos.DrawWireSphere(transform.position, GetRangeValue(stats.rangeTier));
//            }
//        }

//        void InitializeEnemy(EnemyData data)
//        {
//            gameObject.name = data.displayName;
//        }
//    }
//}


using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public string enemyID;
    private EnemyData stats;
    private Transform target;

    private float attackTimer = 0f;
    public float attackCooldown = 1.5f;
    private PlayerStats targetStats;

    private float GetRangeValue(string tier)
    {
        return tier switch
        {
            "Short" => 1.5f,
            "Close" => 3.0f,
            "Explosion" => 4.0f,
            "Large" => 6.0f,
            "Largest" => 10.0f,
            _ => 2.0f
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
        Debug.Log("EnemyController: Start() has been called!");
        if (EnemyManager.enemyDatabase.TryGetValue(enemyID, out stats))
        {
            InitializeEnemy(stats);
        }
        else
        {
            Debug.LogError($"Enemy ID '{enemyID}' not found in database!");
        }

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            target = playerObj.transform;
        }
        else
        {
            Debug.LogError("Player object with tag 'Player' not found!");
        }
    }

    void Update()
    {
        if (stats == null || target == null) return;

        float distance = Vector3.Distance(transform.position, target.position);
        float attackRange = GetRangeValue(stats.rangeTier);

        if (distance <= attackRange)
        {
            if (attackTimer <= 0)
            {
                PerformAttack();
                attackTimer = attackCooldown;
            }
        }
        else
        {
            if (float.TryParse(stats.moveSpeedValue, out float speed))
            {
                transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            }
        }

        if (attackTimer > 0) attackTimer -= Time.deltaTime;
    }

    void PerformAttack()
    {
        float attackRange = GetRangeValue(stats.rangeTier);
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, attackRange);

        foreach (var hitCollider in hitColliders)
        {
            PlayerStats player = hitCollider.GetComponent<PlayerStats>();
            if (player != null)
            {
                int damageToDeal = GetDamageForEnemyType();
                player.TakeDamage(damageToDeal);
                Debug.Log($"SUCCESS: {gameObject.name} dealt {damageToDeal} damage to {hitCollider.name}!");
                break;
            }
        }
    }

    int GetDamageForEnemyType()
    {
        Debug.Log("Getting damage for ID: " + enemyID);
        string id = enemyID.Trim();

        if (id.Equals("Small", System.StringComparison.OrdinalIgnoreCase)) return 1;
        if (id.Equals("MediumV1", System.StringComparison.OrdinalIgnoreCase)) return 2;
        if (id.Equals("Bomber", System.StringComparison.OrdinalIgnoreCase)) return 2;
        if (id.Equals("Large", System.StringComparison.OrdinalIgnoreCase)) return 3;
        if (id.Equals("Boss", System.StringComparison.OrdinalIgnoreCase)) return 4;

        Debug.LogWarning("Enemy ID '" + id + "' not found! Returning 1 damage.");
        return 1;
    }

    void OnDrawGizmosSelected()
    {
        if (stats != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, GetRangeValue(stats.rangeTier));
        }
    }

    void InitializeEnemy(EnemyData data)
    {
        gameObject.name = data.displayName;
    }
}