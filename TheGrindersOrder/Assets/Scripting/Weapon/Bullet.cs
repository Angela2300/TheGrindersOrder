using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 15f;
    public float lifeTime = 2f;

    [Header("Damage")]
    public float damage = 10f;

    [Header("Explosion (for launcher)")]
    public bool isExplosive = false;
    public float explosionRadius = 2f;
    public LayerMask damageLayers;   // set this to Enemy layer(s) in Inspector

    [Header("VFX")]
    public GameObject hitEffect;
    public float hitEffectDestroyTime = 0.3f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // Move bullet along its local up axis
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        // 1. Define what you WANT to interact with
        bool isEnemy = other.CompareTag("Enemy");
        bool isWall = other.CompareTag("Wall");
      

        // 2. Ignore everything that isn't an enemy or wall
        if (!isEnemy && !isWall)
        {
            return;
        }
        Debug.Log("Launcher bullet hit: " + other.name);

        // 4. Proceed with your existing logic...
        if (hitEffect != null)
        {
            GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
            Destroy(effect, hitEffectDestroyTime);
        }

        if (isExplosive)
        {
            Explode();
        }
        else
        {
            TryDamageTarget(other);
        }

        Destroy(gameObject);
    }

    void Explode()
    {
        // Damage everything in radius on damageLayers
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius, damageLayers);

        foreach (var hit in hits)
        {
            TryDamageTarget(hit);
        }
    }

    void TryDamageTarget(Collider2D collider)
    {
        // Current dummy enemy script
        EnemyDummyDropWeapon enemy = collider.GetComponent<EnemyDummyDropWeapon>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }

        // Later you can add other enemy health scripts here if needed
    }

    void OnDrawGizmosSelected()
    {
        if (isExplosive)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, explosionRadius);
        }
    }
}