using UnityEngine;

// Types of things the player can pick up
public enum PickupType
{
    Weapon,
    Coin,
    Meat
}

public class PickupItem : MonoBehaviour
{
    // What type of pickup object?
    public PickupType pickupType;

    // Only used if this pickup is a weapon
    //"Weapon Only"
    public WeaponType weaponType;

    // Amount of coins to give
  //"Item Amount"
    public int amount = 1;

    // Called automatically when another collider enters this object
    void OnTriggerEnter2D(Collider2D other)
    {
        // Ignore anything that is not the player
        if (!other.CompareTag("Player"))
            return;

        switch (pickupType)
        {
            case PickupType.Weapon:
                {
                    Inventory inv = other.GetComponent<Inventory>(); // Using 'inv' instead
                    //if (inv != null && inv.AddWeapon(weaponType))
                    //    Destroy(gameObject);
                    //break;
                    // Check if inv is null
                    if (inv == null) {
                        Debug.LogError("Pickup Failed: No Inventory component found on Player!");
                    } else {
                        bool result = inv.AddWeapon(weaponType);
                        Debug.Log("Inventory found. AddWeapon result: " + result);
                        if (result) Destroy(gameObject);
                    }
                    break;

                }

            case PickupType.Coin:
                {
                    PlayerStats stats = other.GetComponent<PlayerStats>(); // Local to this case
                    if (stats != null)
                    {
                        stats.AddCoins(amount);
                        Destroy(gameObject);
                    }
                    break;
                }

            case PickupType.Meat:
                {
                    Inventory inv = other.GetComponent<Inventory>();
                    if (inv != null && inv.AddMeat())
                        Destroy(gameObject);
                    break;
                }
        }
    }

        // Find the Inventory script inside the player
        //Inventory inventory = other.GetComponentInChildren<Inventory>();

        // Check what type of pickup this object is
        //    switch (pickupType)
        //    {
        //        // ========================
        //        // WEAPON PICKUP
        //        // ========================
        //        case PickupType.Weapon:

        //            // Make sure the player has an inventory
        //            //if (inventory != null)
        //            //{
        //            //    // Try adding the weapon into the inventory
        //            //    bool added = inventory.AddWeapon(weaponType);

        //            //    // If successful, remove this pickup object from the world
        //            //    if (added)
        //            //        Destroy(gameObject);
        //            //}

        //            ////break;
        //            //if (inventory != null)
        //            //{
        //            //    if (inventory.AddWeapon(weaponType))
        //            //        Destroy(gameObject);
        //            //}
        //            //else
        //            //{
        //            //    Debug.LogError("Inventory script not found on Player!");
        //            //}
        //            //break;

        //        // ========================
        //        // COIN PICKUP
        //        // ========================
        //        case PickupType.Coin:

        //            // Find the PlayerStats script inside the player
        //            PlayerStats stats = other.GetComponentInChildren<PlayerStats>();

        //            // Make sure PlayerStats exists
        //            if (stats != null)
        //            {
        //                // Increase the player's coin count
        //                stats.AddCoins(amount);

        //                // Remove the coin object from the world
        //                Destroy(gameObject);
        //            }

        //            break;

        //        // ========================
        //        // MEAT PICKUP
        //        // ========================
        //        case PickupType.Meat:

        //            // Make sure the player has an inventory
        //            if (inventory != null)
        //            {
        //                // Try adding meat into the inventory
        //                bool added = inventory.AddMeat();

        //                // If successful, remove the meat object from the world
        //                if (added)
        //                    Destroy(gameObject);
        //            }

        //            break;
        //    }
        //}
    }