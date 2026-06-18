
using UnityEngine;
using UnityEngine.UI;

public class EnemyDummyDropWeapon : MonoBehaviour
{
    [Header("Health")]
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("UI")]
    public Slider healthSlider;

    [Header("Weapon Drop")]
    public GameObject weaponDropPrefab;

    [Header("Coin Drop")]
    public GameObject coinDropPrefab;

    [Header("Meat Drop")]
    public GameObject meatDropPrefab;

    void Start()
    {
        currentHealth = maxHealth;

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);

        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Drop weapon
        if (weaponDropPrefab != null)
        {
            Instantiate(weaponDropPrefab, transform.position, Quaternion.identity);
        }

        // Drop coin slightly to the right
        if (coinDropPrefab != null)
        {
            Instantiate(
                coinDropPrefab,
                transform.position + new Vector3(0.3f, 0f, 0f),
                Quaternion.identity
            );
        }

        // Drop meat slightly to the left
        if (meatDropPrefab != null)
        {
            Instantiate(
                meatDropPrefab,
                transform.position + new Vector3(-0.3f, 0f, 0f),
                Quaternion.identity
            );
        }

        Destroy(gameObject);
    }
}