//using UnityEngine;

//public class EnemyController : MonoBehaviour
//{
//    public string enemyID;
//    public float attackCooldown = 1.5f;
//    public EnemyManager manager;
//    public EnemyData stats;

//    private float attackTimer;
//    private Transform player;

//    public GameObject bulletPrefab;

//    private bool isAttacking = false;// Prevents overlapping attacks



//    public void Setup(string id)
//    {
//        enemyID = id.ToLower();

//        if (manager != null && EnemyManager.enemyDatabase.ContainsKey(enemyID))
//        {
//            stats = EnemyManager.enemyDatabase[enemyID];
//            Debug.Log("Enemy setup complete: " + stats.displayName);

//            var dropScript = GetComponent<EnemyDummyDropWeapon>();
//            if (dropScript != null)
//            {
//                dropScript.maxHealth = stats.health;
//                dropScript.CurrentHealth = stats.health;
//            }
//        }

//        player = GameObject.FindGameObjectWithTag("Player")?.transform;
//    }

//    private void Update()
//    {
//        if (stats == null) return;

//        if (stats.followsPlayer && player != null)
//        {
//            transform.position = Vector3.MoveTowards(
//                transform.position,
//                player.position,
//                stats.moveSpeedValue * Time.deltaTime
//            );
//        }

//        attackTimer += Time.deltaTime;
//        if (attackTimer >= attackCooldown)
//        {
//            Attack();
//            attackTimer = 0f;
//        }
//    }

//    private void Attack()
//    {
//        if (player == null) return;

//        // Ensure stats has attackRange added (see step 2 below)
//        if (Vector3.Distance(transform.position, player.position) <= stats.attackRange)
//        {
//            // If the enemy has a bullet, fire it. Otherwise, deal direct damage.
//            if (bulletPrefab != null)
//            {
//                FireBullet();
//            }
//            else
//            {
//                var playerStats = player.GetComponent<PlayerStats>();
//                if (playerStats != null)
//                {
//                    // Use damageHearts from your CSV data
//                    playerStats.TakeDamage(Mathf.CeilToInt(stats.damageHearts));
//                }
//            }

//            // Reset timer
//            attackTimer = 0f;
//        }
//    }

//    private void FireBullet()
//    {
//        if (bulletPrefab == null) return;

//        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
//        Vector2 direction = (player.position - transform.position).normalized;
//        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
//        bullet.transform.rotation = Quaternion.Euler(0, 0, angle);

//        Bullet bulletScript = bullet.GetComponent<Bullet>();
//        if (bulletScript != null)
//        {
//            bulletScript.damage = Mathf.CeilToInt(stats.damageHearts);
//        }

//        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
//        if (rb != null)
//        {
//            float speed = bulletScript != null ? bulletScript.speed : 10f;
//            rb.linearVelocity = direction * speed;
//        }
//    }

//    public void Die()
//    {
//        if (manager != null)
//            Object.FindFirstObjectByType<LevelManager>().RegisterCustomerServed(gameObject);

//        Destroy(gameObject);
//    }
//}

using UnityEngine;
using System.Collections; // Required for Coroutines

public class EnemyController : MonoBehaviour
{
    public string enemyID;
    public float attackCooldown = 1.5f;
    public EnemyManager manager;
    public EnemyData stats;

    private float attackTimer;
    private Transform player;
    private bool isAttacking = false; // Prevents overlapping attacks

    public GameObject bulletPrefab;

    public void Setup(string id)
    {
        enemyID = id.ToLower();
        if (manager != null && EnemyManager.enemyDatabase.ContainsKey(enemyID))
        {
            stats = EnemyManager.enemyDatabase[enemyID];
            var dropScript = GetComponent<EnemyDummyDropWeapon>();
            if (dropScript != null)
            {
                dropScript.maxHealth = stats.health;
                dropScript.CurrentHealth = stats.health;
            }
        }
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void Update()
    {
        if (stats == null) return;

        // Only move if NOT currently in the middle of an attack animation
        if (!isAttacking && stats.followsPlayer && player != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, stats.moveSpeedValue * Time.deltaTime);
        }

        attackTimer += Time.deltaTime;

        // Only trigger attack if we are within range and cooldown is finished
        if (!isAttacking && attackTimer >= attackCooldown)
        {
            if (player != null && Vector3.Distance(transform.position, player.position) <= stats.attackRange)
            {
                StartCoroutine(AttackRoutine());
            }
        }
    }

    private IEnumerator AttackRoutine()
    {
        isAttacking = true;

        // 1. TELEGRAPH PHASE: Pause here to create a "wind-up" feeling
        // Change the 0.7f to a higher number to make the enemy feel slower/heavier
        yield return new WaitForSeconds(0.7f);

        // 2. STRIKE PHASE: Execute the damage logic
        if (player != null && Vector3.Distance(transform.position, player.position) <= stats.attackRange)
        {
            if (bulletPrefab != null) FireBullet();
            else
            {
                var playerStats = player.GetComponent<PlayerStats>();
                if (playerStats != null)
                {
                    playerStats.TakeDamage(Mathf.CeilToInt(stats.damageHearts));
                }
            }
        }

        // 3. RECOVERY PHASE
        attackTimer = 0f;
        isAttacking = false;
    }

    private void FireBullet()
    {
        if (bulletPrefab == null) return;

        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        Vector2 direction = (player.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        bullet.transform.rotation = Quaternion.Euler(0, 0, angle);

        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null) bulletScript.damage = Mathf.CeilToInt(stats.damageHearts);

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null) rb.linearVelocity = direction * (bulletScript != null ? bulletScript.speed : 10f);
    }

    public void Die()
    {
        if (manager != null)
            Object.FindFirstObjectByType<LevelManager>().RegisterCustomerServed(gameObject);
        Destroy(gameObject);
    }
}
