
using UnityEngine;

public enum InventorySlotType
{
    Empty,
    Weapon,
    Meat
}

public class Inventory : MonoBehaviour
{
    public int maxSlots = 5;

    public InventorySlotType[] slotTypes;
    public WeaponType[] weapons;
    public bool[] slotUsed;

    public WeaponSystem weaponSystem;
    public InventoryUI inventoryUI;
    public int meatCount = 0;



    void Awake()
    {
        slotTypes = new InventorySlotType[maxSlots];
        weapons = new WeaponType[maxSlots];
        slotUsed = new bool[maxSlots];

        // Slot 1 is always reserved for meat
        slotUsed[0] = true;
        slotTypes[0] = InventorySlotType.Meat;
        meatCount = 0;
    }

    void Start()
    {
        // Show meat icon at the start as x0
        if (inventoryUI != null)
        {
            inventoryUI.UpdateInventory(weapons, slotUsed, slotTypes, meatCount);
        }
    }

    public bool AddWeapon(WeaponType weaponType)
    {
        // Start from slot 2 because slot 1 is only for meat
        for (int i = 1; i < maxSlots; i++)
        {
            if (!slotUsed[i])
            {
                slotUsed[i] = true;
                slotTypes[i] = InventorySlotType.Weapon;
                weapons[i] = weaponType;

                weaponSystem.SetWeapon(weaponType);
                inventoryUI.UpdateInventory(weapons, slotUsed, slotTypes, meatCount);

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

        inventoryUI.UpdateInventory(weapons, slotUsed, slotTypes, meatCount);

        return true;
    }

    public void EquipWeaponFromSlot(int slotIndex)
    {
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


//using UnityEngine;

//public enum InventoryItemType
//{
//    Empty,
//    Weapon,
//    Meat,
//    Medkit,
//    ShopItem
//}

//[System.Serializable]
//public class InventorySlot
//{
//    public InventoryItemType itemType = InventoryItemType.Empty;
//    public WeaponType weaponType;
//    public int amount = 0;
//}

//public class Inventory : MonoBehaviour
//{
//    public int maxSlots = 5;

//    public InventorySlot[] slots;

//    public WeaponSystem weaponSystem;
//    public InventoryUI inventoryUI;

//    void Awake()
//    {
//        slots = new InventorySlot[maxSlots];

//        for (int i = 0; i < maxSlots; i++)
//        {
//            slots[i] = new InventorySlot();
//        }
//    }

//    void Start()
//    {
//        inventoryUI.UpdateInventory(slots);
//    }

//    public bool AddWeapon(WeaponType weaponType)
//    {
//        for (int i = 0; i < maxSlots; i++)
//        {
//            if (slots[i].itemType == InventoryItemType.Empty)
//            {
//                slots[i].itemType = InventoryItemType.Weapon;
//                slots[i].weaponType = weaponType;
//                slots[i].amount = 1;

//                weaponSystem.SetWeapon(weaponType);
//                inventoryUI.UpdateInventory(slots);

//                return true;
//            }
//        }

//        Debug.Log("Inventory full!");
//        return false;
//    }

//    public bool AddMeat()
//    {
//        // If meat already exists, stack it
//        for (int i = 0; i < maxSlots; i++)
//        {
//            if (slots[i].itemType == InventoryItemType.Meat)
//            {
//                slots[i].amount++;
//                inventoryUI.UpdateInventory(slots);
//                return true;
//            }
//        }

//        // If no meat yet, put meat into empty slot
//        for (int i = 0; i < maxSlots; i++)
//        {
//            if (slots[i].itemType == InventoryItemType.Empty)
//            {
//                slots[i].itemType = InventoryItemType.Meat;
//                slots[i].amount = 1;

//                inventoryUI.UpdateInventory(slots);
//                return true;
//            }
//        }

//        Debug.Log("Inventory full!");
//        return false;
//    }

//    public bool AddMedkit()
//    {
//        for (int i = 0; i < maxSlots; i++)
//        {
//            if (slots[i].itemType == InventoryItemType.Medkit)
//            {
//                slots[i].amount++;
//                inventoryUI.UpdateInventory(slots);
//                return true;
//            }
//        }

//        for (int i = 0; i < maxSlots; i++)
//        {
//            if (slots[i].itemType == InventoryItemType.Empty)
//            {
//                slots[i].itemType = InventoryItemType.Medkit;
//                slots[i].amount = 1;

//                inventoryUI.UpdateInventory(slots);
//                return true;
//            }
//        }

//        Debug.Log("Inventory full!");
//        return false;
//    }

//    public void EquipWeaponFromSlot(int slotIndex)
//    {
//        if (slotIndex < 0 || slotIndex >= maxSlots)
//            return;

//        if (slots[slotIndex].itemType != InventoryItemType.Weapon)
//            return;

//        weaponSystem.SetWeapon(slots[slotIndex].weaponType);
//    }
//}