using UnityEngine;

using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public WeaponType[] weapons = new WeaponType[3];
    public bool[] slotUsed = new bool[3];

    public WeaponSystem weaponSystem;
    public InventoryUI inventoryUI;

    public bool AddWeapon(WeaponType weaponType)
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            if (!slotUsed[i])
            {
                weapons[i] = weaponType;
                slotUsed[i] = true;

                weaponSystem.SetWeapon(weaponType); // Equip picked weapon
                inventoryUI.UpdateInventory(weapons, slotUsed);

                return true;
            }
        }

        Debug.Log("Inventory full!");
        return false;
    }
}