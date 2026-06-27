using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public int damage = 1;
    public float speed = 10f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerStats playerStats = other.GetComponent<PlayerStats>();

        if (playerStats != null)
        {
            playerStats.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}