using UnityEngine;
using UnityEngine.UI;

public class EnemyDummyDropWeapon : MonoBehaviour
{
    [Header("Health")]
    public float maxHealth = 100f;
    public float currentHealth;
    public Slider healthSlider;

    [Header("Drop Prefabs")]
    public GameObject weaponDropPrefab;
    public GameObject coinDropPrefab;
    public GameObject meatDropPrefab;

    private EnemyController controller;

    public float CurrentHealth
    {
        get { return currentHealth; }
        set
        {
            currentHealth = Mathf.Clamp(value, 0, maxHealth);
            if (healthSlider != null) healthSlider.value = currentHealth;
        }
    }

    void Start()
    {
        // 1. Assign the controller
        controller = GetComponent<EnemyController>();

        CurrentHealth = maxHealth;
        if (healthSlider != null) healthSlider.maxValue = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        CurrentHealth -= damage;
        if (CurrentHealth <= 0)
        {
            Die();
        }
    }


    void Die()
    {
        // 1. Drop Weapon (Always drops if the prefab is assigned)
        if (weaponDropPrefab != null)
        {
            Instantiate(weaponDropPrefab, transform.position, Quaternion.identity);
        }

        // Now it will correctly pull from the data assigned in Setup()
        if (controller != null && controller.stats != null)
        {
            Debug.Log("Meat count from stats: " + controller.stats.meatDropAmt);
            // Drop Meat
            for (int i = 0; i < controller.stats.meatDropAmt; i++)
            {
                Instantiate(meatDropPrefab, transform.position + new Vector3(-0.2f * i, 0, 0), Quaternion.identity);
            }

            // Drop Coins
            for (int i = 0; i < controller.stats.coinDropAmt; i++)
            {
                Instantiate(coinDropPrefab, transform.position + new Vector3(0.2f * i, 0, 0), Quaternion.identity);
            }
        }
      

        if (controller != null) controller.Die();
        Destroy(gameObject);
    }


}
