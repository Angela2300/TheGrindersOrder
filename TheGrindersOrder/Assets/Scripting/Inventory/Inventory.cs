using UnityEngine;


//Yet to reference to csv


// Types of things that can occupy an inventory slot
public enum InventorySlotType
{
    Empty,
    Weapon,
    Meat
}

public class Inventory : MonoBehaviour
{
    // Maximum number of inventory slots
    public int maxSlots = 5;

    // Stores what type of item is in each slot
    public InventorySlotType[] slotTypes;

    // Stores the weapon inside each slot
    public WeaponType[] weapons;

    // Keeps track of whether a slot is occupied
    public bool[] slotUsed;

    // Reference to the player's weapon system
    public WeaponSystem weaponSystem;

    // Reference to the inventory UI
    public InventoryUI inventoryUI;

    // Amount of meat currently owned
    public int meatCount = 0;

    void Awake()
    {
        // Create arrays based on maxSlots
        slotTypes = new InventorySlotType[maxSlots];
        weapons = new WeaponType[maxSlots];
        slotUsed = new bool[maxSlots];

        // Reserve slot 1 (index 0) for meat
        slotUsed[0] = true;
        slotTypes[0] = InventorySlotType.Meat;

        // Start with zero meat
        meatCount = 0;
    }

    void Start()
    {
        // Update the inventory UI when the game starts
        // This allows the meat icon to appear as x0 immediately
        if (inventoryUI != null)
        {
            inventoryUI.UpdateInventory(
                weapons,
                slotUsed,
                slotTypes,
                meatCount
            );
        }
    }

    public bool AddWeapon(WeaponType weaponType)
    {
        // Start from slot 2 because slot 1 is reserved for meat
        for (int i = 1; i < maxSlots; i++)
        {
            // Find the first empty slot
            if (!slotUsed[i])
            {
                // Mark this slot as occupied
                slotUsed[i] = true;

                // Mark this slot as containing a weapon
                slotTypes[i] = InventorySlotType.Weapon;

                // Store the weapon inside the slot
                weapons[i] = weaponType;

                // Equip the weapon immediately
                weaponSystem.SetWeapon(weaponType);

                // Update the inventory UI
                inventoryUI.UpdateInventory(
                    weapons,
                    slotUsed,
                    slotTypes,
                    meatCount
                );

                return true;
            }
        }

        // No empty weapon slots available
        Debug.Log("Weapon slots full!");
        return false;
    }

    public bool AddMeat()
    {
        // Slot 1 (index 0) is always the meat slot
        int meatSlotIndex = 0;

        // Increase meat count
        meatCount++;

        // Ensure slot 1 is marked as occupied
        slotUsed[meatSlotIndex] = true;

        // Set slot 1 to be a meat slot
        slotTypes[meatSlotIndex] = InventorySlotType.Meat;

        // Refresh the inventory UI so the count updates
        inventoryUI.UpdateInventory(
            weapons,
            slotUsed,
            slotTypes,
            meatCount
        );

        return true;
    }

    public void EquipWeaponFromSlot(int slotIndex)
    {
        Debug.Log("Working button");

        // Prevent invalid slot numbers
        if (slotIndex < 0 || slotIndex >= maxSlots)
            return;

        // Stop if the slot is empty
        if (!slotUsed[slotIndex])
            return;

        // Only weapons can be equipped
        if (slotTypes[slotIndex] != InventorySlotType.Weapon)
            return;

        // Change the player's current weapon
        weaponSystem.SetWeapon(weapons[slotIndex]);

        Debug.Log(
            "Equipped weapon from slot "
            + slotIndex +
            ": " +
            weapons[slotIndex]
        );
    }
}