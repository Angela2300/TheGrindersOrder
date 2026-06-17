using UnityEngine;

public class EnemyHealthTest : MonoBehaviour
{
    public float health = 50f;

    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log($"{gameObject.name} took {damage}, remaining {health}");

        if (health <= 0f)
        {
            Debug.Log($"{gameObject.name} died");
            Destroy(gameObject);
        }
    }
}
