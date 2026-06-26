using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    public string enemyID;
    public float attackCooldown = 1.5f;
    public EnemyManager manager;

    // This is your main data source, as referenced in your Setup() method
    public EnemyData stats;

    private float attackTimer;
    private Transform player;
    private bool isAttacking = false;

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

        if (!isAttacking && stats.followsPlayer && player != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, stats.moveSpeedValue * Time.deltaTime);
        }

        attackTimer += Time.deltaTime;

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
        yield return new WaitForSeconds(0.7f);

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