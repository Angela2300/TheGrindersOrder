using UnityEngine;

public enum PickupType
{
    Weapon,
    Coin,
    Meat
}

public class PickupItem : MonoBehaviour
{
    public PickupType pickupType;

    [Header("Weapon Only")]
    public WeaponType weaponType;

    [Header("Item Amount")]
    public int amount = 1;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        Inventory inventory = other.GetComponentInChildren<Inventory>();

        switch (pickupType)
        {
            case PickupType.Weapon:

                if (inventory != null)
                {
                    bool added = inventory.AddWeapon(weaponType);

                    if (added)
                        Destroy(gameObject);
                }

                break;

            case PickupType.Coin:

                PlayerStats stats = other.GetComponentInChildren<PlayerStats>();

                if (stats != null)
                {
                    stats.AddCoins(amount);
                    Destroy(gameObject);
                }

                break;

            case PickupType.Meat:

                if (inventory != null)
                {
                    bool added = inventory.AddMeat();

                    if (added)
                        Destroy(gameObject);
                }

                break;
        }
    }
}