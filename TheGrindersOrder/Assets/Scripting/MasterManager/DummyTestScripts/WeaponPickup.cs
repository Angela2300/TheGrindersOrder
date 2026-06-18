using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public WeaponType weaponType;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        WeaponSystem weaponSystem = other.GetComponentInChildren<WeaponSystem>();

        if (weaponSystem != null)
        {
            weaponSystem.SetWeapon(weaponType);
            Destroy(gameObject);
        }
    }
}

