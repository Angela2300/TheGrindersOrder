using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    [Header("CSV Enemy Data")]
    public string enemyID;
    public EnemyManager manager;
    public EnemyData stats;

    [Header("General Attack")]
    public float attackCooldown = 0.5f;

    [Header("Shotgun Enemy")]
    public bool isShotgunEnemy = false;
    public GameObject shotgunBulletPrefab;
    public float shotgunBulletSpeed = 12f;
    public float shotgunBulletSpawnOffset = 1.2f;
    public int shotgunDamage = 2;
    public GameObject shotgunRangeVFX;

    [Header("Bomber Enemy")]
    public bool isBomber = false;
    public int bomberDamage = 3;
    public GameObject bomberExplosionVFX;

    [Header("Knight Enemy")]
    public bool isKnightEnemy = false;
    public int knightDamage = 4;
    public GameObject knightRangeVFX;

    [Header("Boss Enemy")]
    public bool isBossEnemy = false;
    public int bossDamage = 5;
    public GameObject bossBulletPrefab;
    public float bossBulletSpeed = 12f;
    public float bossBulletSpawnOffset = 1.2f;
    public GameObject bossExplosionVFX;
    public GameObject bossRangeVFX;

    private float attackTimer;
    private Transform player;
    private bool isAttacking = false;
    private bool hasExploded = false;

    public void Setup(string id)
    {
        enemyID = id.ToLower();

        if (manager == null)
        {
            manager = Object.FindFirstObjectByType<EnemyManager>();
        }

        if (EnemyManager.enemyDatabase.ContainsKey(enemyID))
        {
            stats = EnemyManager.enemyDatabase[enemyID];

            EnemyDummyDropWeapon dropScript = GetComponent<EnemyDummyDropWeapon>();
            if (dropScript != null)
            {
                dropScript.maxHealth = stats.health;
                dropScript.CurrentHealth = stats.health;
            }
        }
        else
        {
            Debug.LogError("Enemy ID not found: " + enemyID);
        }

        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        AutoSetEnemyType();
        SetupAllRangeVFX();
    }

    private void Start()
    {
        if (stats == null && !string.IsNullOrEmpty(enemyID))
        {
            Setup(enemyID);
        }
    }

    private void AutoSetEnemyType()
    {
        string id = enemyID.ToLower();

        if (id.Contains("shotgun"))
            isShotgunEnemy = true;

        if (id.Contains("bomber"))
            isBomber = true;

        if (id.Contains("large") || id.Contains("knight"))
            isKnightEnemy = true;

        if (id.Contains("boss"))
            isBossEnemy = true;
    }

    private void Update()
    {
        if (stats == null || player == null)
            return;

        float distance = Vector3.Distance(transform.position, player.position);

        float finalAttackRange = stats.attackRange;

        if (isShotgunEnemy)
        {
            finalAttackRange = 6f;
        }

        if (isBossEnemy)
        {
            finalAttackRange = 8f;
        }

        if (isBomber)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                player.position,
                stats.moveSpeedValue * Time.deltaTime
            );

            return;
        }

        if (isKnightEnemy)
        {
            finalAttackRange = 1.2f;
        }

        if (distance > finalAttackRange)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                player.position,
                stats.moveSpeedValue * Time.deltaTime
            );
        }

        attackTimer += Time.deltaTime;

        if (distance <= finalAttackRange && attackTimer >= attackCooldown)
        {
            attackTimer = 0f;

            if (isShotgunEnemy)
            {
                FireShotgun(shotgunBulletPrefab, shotgunBulletSpeed, shotgunBulletSpawnOffset, shotgunDamage);
            }
            else if (isBossEnemy)
            {
                FireShotgun(bossBulletPrefab, bossBulletSpeed, bossBulletSpawnOffset, bossDamage);
                SpawnVFX(bossExplosionVFX);
            }
            else if (isKnightEnemy)
            {
                DamagePlayer(player.gameObject, knightDamage);
            }
            else
            {
                DamagePlayer(player.gameObject, 1);
            }
        }
    }

    private IEnumerator AttackRoutine()
    {
        isAttacking = true;

        yield return new WaitForSeconds(0.5f);

        if (player != null && Vector3.Distance(transform.position, player.position) <= stats.attackRange)
        {
            if (isKnightEnemy)
            {
                DamagePlayer(player.gameObject, knightDamage);
            }
            else
            {
                DamagePlayer(player.gameObject, 1);
            }
        }

        isAttacking = false;
    }

    private void FireShotgun(GameObject bulletPrefab, float speed, float spawnOffset, int damage)
    {
        if (bulletPrefab == null || player == null) return;

        Vector2 baseDirection = ((Vector2)player.position - (Vector2)transform.position).normalized;
        float[] spreadAngles = { -25f, 0f, 25f };

        foreach (float spread in spreadAngles)
        {
            Vector2 direction = Quaternion.Euler(0f, 0f, spread) * baseDirection;

            GameObject bullet = Instantiate(
                bulletPrefab,
                transform.position + (Vector3)(direction * spawnOffset),
                Quaternion.identity
            );

            bullet.transform.localScale = new Vector3(0.15f, 0.15f, 1f);

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            bullet.transform.rotation = Quaternion.Euler(0f, 0f, angle);

            EnemyBullet enemyBullet = bullet.GetComponent<EnemyBullet>();

            if (enemyBullet != null)
            {
                enemyBullet.damage = damage;
                enemyBullet.speed = speed;
            }

            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

            if (rb == null)
            {
                rb = bullet.AddComponent<Rigidbody2D>();
            }

            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.gravityScale = 0f;
            rb.freezeRotation = true;
            rb.linearVelocity = direction * speed;

            Collider2D col = bullet.GetComponent<Collider2D>();

            if (col != null)
            {
                col.isTrigger = true;
            }

            Destroy(bullet, 2f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        HandleTouch(other.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HandleTouch(collision.gameObject);
    }

    private void HandleTouch(GameObject other)
    {
        if (hasExploded) return;
        if (!other.CompareTag("Player")) return;

        if (isBomber)
        {
            hasExploded = true;
            DamagePlayer(other, bomberDamage);
            SpawnVFX(bomberExplosionVFX);
            Destroy(gameObject);
            return;
        }

        if (isBossEnemy)
        {
            hasExploded = true;
            DamagePlayer(other, bossDamage);
            SpawnVFX(bossExplosionVFX);
        }
    }

    private void DamagePlayer(GameObject target, int damage)
    {
        PlayerStats playerStats = target.GetComponent<PlayerStats>();

        if (playerStats != null)
        {
            playerStats.TakeDamage(damage);
        }
    }

 

    private void SetupAllRangeVFX()
    {
        SetupRangeVFX(shotgunRangeVFX, 6f);
        SetupRangeVFX(knightRangeVFX, 1.2f);
        SetupRangeVFX(bossRangeVFX, 8f);
    }



    private void SetupRangeVFX(GameObject rangeVFX, float range)
    {
        if (rangeVFX == null) return;

        rangeVFX.SetActive(true);

        float size = range * 2f;
        rangeVFX.transform.localScale = new Vector3(size, size, 1f);
    }

    private void SpawnVFX(GameObject vfxPrefab)
    {
        if (vfxPrefab == null) return;

        GameObject vfx = Instantiate(
            vfxPrefab,
            transform.position + new Vector3(0f, 0f, -5f),
            Quaternion.identity
        );

        Destroy(vfx, 3f);
    }

    public void Die()
    {
        LevelManager levelManager = Object.FindFirstObjectByType<LevelManager>();

        if (levelManager != null)
        {
            levelManager.RegisterCustomerServed(gameObject);

            levelManager.RegisterBossKilled(enemyID);
        }

        Destroy(gameObject);
    }

}