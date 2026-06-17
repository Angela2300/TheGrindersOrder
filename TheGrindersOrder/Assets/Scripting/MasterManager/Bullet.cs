using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 15f;
    public float damage = 10f;
    public float lifeTime = 2f;
    public GameObject hitEffect;

    void Start()
    {
        // Destroy bullet after some time so it does not stay forever
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // Bullet moves forward based on where it is facing
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Ignore the player so bullet doesn't explode immediately
        if (other.CompareTag("Player"))
            return;

        if (hitEffect != null)
        {
            Instantiate(hitEffect, transform.position, Quaternion.identity);
        }

        EnemyHealthTest enemy = other.GetComponent<EnemyHealthTest>();

        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }

        Destroy(gameObject);
    }

    void SpawnHitEffect()
    {
        if (hitEffect != null)
        {
            Instantiate(hitEffect, transform.position, transform.rotation);
        }
    }


}