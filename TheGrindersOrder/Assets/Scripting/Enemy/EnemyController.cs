using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public string enemyID;
    public float attackCooldown = 1.5f;

    [HideInInspector] public EnemyManager manager;
    [HideInInspector] public EnemyData stats;

    private float attackTimer;
    private Transform player;

    public GameObject bulletPrefab;

    private int bossAttackPhase = 0;

    public void Setup(string id)
    {
        enemyID = id.ToLower();

        if (manager != null && EnemyManager.enemyDatabase.ContainsKey(enemyID))
        {
            stats = EnemyManager.enemyDatabase[enemyID];
            Debug.Log("Enemy setup complete: " + stats.displayName);

            var dropScript = GetComponent<EnemyDummyDropWeapon>();
            if (dropScript != null)
            {
                dropScript.maxHealth = stats.health;
                dropScript.CurrentHealth = stats.health;
            }
        }
        else
        {
            Debug.LogWarning("Enemy setup failed: no data for ID " + enemyID);
        }

        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void Update()
    {
        if (stats == null) return;

        if (stats.followsPlayer && player != null)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                player.position,
                stats.moveSpeedValue * Time.deltaTime
            );
        }

        attackTimer += Time.deltaTime;
        if (attackTimer >= attackCooldown)
        {
            Attack();
            attackTimer = 0f;
        }
    }

    private void Attack()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
        string tier = stats.rangeTier.ToLower();

        if (tier == "largest")
        {
            switch (bossAttackPhase)
            {
                case 0: PerformCloseAttack(distance); attackCooldown = 1.5f; break;
                case 1: PerformShortAttack(distance); attackCooldown = 2.5f; break;
                case 2: PerformExplosionAttack(distance); attackCooldown = 4f; break;
                case 3: PerformLargeAttack(distance); attackCooldown = 5f; break;
            }
            bossAttackPhase = (bossAttackPhase + 1) % 4;
            return;
        }

        if (tier == "close") { PerformCloseAttack(distance); attackCooldown = 1.5f; }
        else if (tier == "short") { PerformShortAttack(distance); attackCooldown = 2.5f; }
        else if (tier == "explosion") { PerformExplosionAttack(distance); attackCooldown = 4f; }
        else if (tier == "large") { PerformLargeAttack(distance); attackCooldown = 5f; }
    }

    private void PerformCloseAttack(float distance)
    {
        if (distance > 2f) return;
        var playerStats = player.GetComponent<PlayerStats>();
        if (playerStats != null)
        {
            playerStats.TakeDamage(Mathf.RoundToInt(stats.damageHearts));
            Debug.Log($"{stats.displayName} dealt {stats.damageHearts} damage (close attack).");
        }
    }

    private void PerformShortAttack(float distance)
    {
        if (distance > 4f) return;
        FireBullet();
    }

    private void PerformExplosionAttack(float distance)
    {
        if (distance > 2.5f) return;
        var playerStats = player.GetComponent<PlayerStats>();
        if (playerStats != null)
        {
            playerStats.TakeDamage(Mathf.RoundToInt(stats.damageHearts * 2));
            Debug.Log($"{stats.displayName} dealt {stats.damageHearts * 2} damage (explosion attack).");
        }
    }

    private void PerformLargeAttack(float distance)
    {
        if (distance > 8f) return;
        FireBullet();
    }

    private void FireBullet()
    {
        if (bulletPrefab == null) return;

        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        Vector2 direction = (player.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        bullet.transform.rotation = Quaternion.Euler(0, 0, angle);

        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null) bulletScript.damage = stats.damageHearts;

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
