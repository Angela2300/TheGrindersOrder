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
        Debug.Log("Launcher bullet hit: " + other.name);

        // Do not hit the player
        if (other.CompareTag("Player"))
            return;

        // Only react to enemies or walls
        if (!other.CompareTag("Enemy") && !other.CompareTag("Wall"))
            return;

        // Spawn hit VFX
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
            // Single-target damage
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