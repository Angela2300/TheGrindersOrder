using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public WeaponType weaponType;

    void OnTriggerEnter2D(Collider2D other)
    {
        // Only player can pick up weapon
        if (!other.CompareTag("Player"))
            return;

        WeaponSystem weaponSystem = other.GetComponent<WeaponSystem>();

        if (weaponSystem != null)
        {
            // Change player's weapon
            weaponSystem.SetWeapon(weaponType);

            // Remove pickup from ground
            Destroy(gameObject);
        }
    }
}