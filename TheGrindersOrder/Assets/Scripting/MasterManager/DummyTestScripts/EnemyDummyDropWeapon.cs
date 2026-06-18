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
        if (weaponDropPrefab != null)
        {
            Instantiate(weaponDropPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
