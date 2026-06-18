using UnityEngine;
using UnityEngine.UI;

public class EnemyDummyDropWeapon : MonoBehaviour
{
    // Maximum health of the enemy
    [Header("Health")]
    public float maxHealth = 100f;

    // Current health during gameplay
    private float currentHealth;

    // Health bar slider shown above the enemy
    [Header("UI")]
    public Slider healthSlider;

    // Weapon that will spawn when the enemy dies
    [Header("Weapon Drop")]
    public GameObject weaponDropPrefab;

    // Coin that will spawn when the enemy dies
    [Header("Coin Drop")]
    public GameObject coinDropPrefab;

    // Meat that will spawn when the enemy dies
    [Header("Meat Drop")]
    public GameObject meatDropPrefab;

    void Start()
    {
        // Start with full health
        currentHealth = maxHealth;

        // Set up the health bar values
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }

    // Called whenever this enemy takes damage
    public void TakeDamage(float damage)
    {
        // Reduce health
        currentHealth -= damage;

        // Prevent health from going below zero
        currentHealth = Mathf.Max(currentHealth, 0);

        // Update the health bar
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

        // If health reaches zero, kill the enemy
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Handles what happens when the enemy dies
    void Die()
    {
        // =====================
        // Drop weapon
        // =====================
        if (weaponDropPrefab != null)
        {
            Instantiate(
                weaponDropPrefab,
                transform.position,
                Quaternion.identity
            );
        }

        // =====================
        // Drop coin slightly to the right
        // =====================
        if (coinDropPrefab != null)
        {
            Instantiate(
                coinDropPrefab,
                transform.position + new Vector3(0.3f, 0f, 0f),
                Quaternion.identity
            );
        }

        // =====================
        // Drop meat slightly to the left
        // =====================
        if (meatDropPrefab != null)
        {
            Instantiate(
                meatDropPrefab,
                transform.position + new Vector3(-0.3f, 0f, 0f),
                Quaternion.identity
            );
        }

        // Remove the enemy from the scene
        Destroy(gameObject);
    }
}