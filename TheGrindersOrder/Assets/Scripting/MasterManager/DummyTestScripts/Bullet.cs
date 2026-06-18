using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 15f;
    public float damage = 10f;
    public float lifeTime = 2f;

    public GameObject hitEffect;
    public float hitEffectDestroyTime = 0.3f;

    void Start()
    {
        // Remove bullet if it flies too long
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // Move bullet forward
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Do not hit the player
        if (other.CompareTag("Player"))
            return;

        // Only react to enemies or walls
        if (!other.CompareTag("Enemy") && !other.CompareTag("Wall"))
            return;

        // Spawn hit VFX and destroy it after a short time
        if (hitEffect != null)
        {
            GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
            Destroy(effect, hitEffectDestroyTime);
        }

        // Damage enemy if it has the enemy script
        EnemyDummyDropWeapon enemy = other.GetComponent<EnemyDummyDropWeapon>();

        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }

        // Destroy bullet after impact
        Destroy(gameObject);
    }
}