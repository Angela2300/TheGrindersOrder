using UnityEngine;

// Types of things that can occupy an inventory slot
public enum InventorySlotType
{
    Empty,
    Weapon,
    Meat
}

public class Inventory : MonoBehaviour
{
    [Header("CSV")]
    public TextAsset inventoryCSV;

    [Header("Inventory")]
    public int maxSlots = 5;

    public InventorySlotType[] slotTypes;
    public WeaponType[] weapons;
    public bool[] slotUsed;

    public WeaponSystem weaponSystem;
    public InventoryUI inventoryUI;

    public int meatCount = 0;

    void Awake()
    {
        LoadInventoryCSV();

        slotTypes = new InventorySlotType[maxSlots];
        weapons = new WeaponType[maxSlots];
        slotUsed = new bool[maxSlots];

        slotUsed[0] = true;
        slotTypes[0] = InventorySlotType.Meat;

        meatCount = 0;
    }

    void Start()
    {
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

    void LoadInventoryCSV()
    {
        if (inventoryCSV == null)
        {
            Debug.LogWarning("Inventory CSV not assigned.");
            return;
        }

        Debug.Log("Inventory CSV loaded:");
        Debug.Log(inventoryCSV.text);
    }

    public bool AddWeapon(WeaponType weaponType)
    {
        for (int i = 1; i < maxSlots; i++)
        {
            if (!slotUsed[i])
            {
                slotUsed[i] = true;
                slotTypes[i] = InventorySlotType.Weapon;
                weapons[i] = weaponType;

                weaponSystem.SetWeapon(weaponType);

                inventoryUI.UpdateInventory(
                    weapons,
                    slotUsed,
                    slotTypes,
                    meatCount
                );

                return true;
            }
        }

        Debug.Log("Weapon slots full!");
        return false;
    }

    public bool AddMeat()
    {
        int meatSlotIndex = 0;

        meatCount++;

        slotUsed[meatSlotIndex] = true;
        slotTypes[meatSlotIndex] = InventorySlotType.Meat;

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

        if (slotIndex < 0 || slotIndex >= maxSlots)
            return;

        if (!slotUsed[slotIndex])
            return;

        if (slotTypes[slotIndex] != InventorySlotType.Weapon)
            return;

        weaponSystem.SetWeapon(weapons[slotIndex]);

        Debug.Log("Equipped weapon from slot " + slotIndex + ": " + weapons[slotIndex]);
    }
}