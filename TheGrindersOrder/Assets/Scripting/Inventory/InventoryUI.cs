using UnityEngine;
using UnityEngine.UI;
using TMPro;

using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class InventoryUI : MonoBehaviour
{
    // The ItemIcon images inside the inventory slots
    public Image[] weaponIcons;

    // Sprites to display for each item
    public Sprite pistolIcon;
    public Sprite smgIcon;
    public Sprite shotgunIcon;
    public Sprite launcherIcon;
    public Sprite meatIcon;

    // Text that shows how much meat the player has
    public TMP_Text meatCountText;

    // Updates the inventory UI whenever something changes
    public void UpdateInventory(
        WeaponType[] weapons,
        bool[] slotUsed,
        InventorySlotType[] slotTypes,
        int meatCount
    )
    {
        // Loop through every inventory slot
        for (int i = 0; i < weaponIcons.Length; i++)
        {
            // Safety check in case an ItemIcon was not assigned in the Inspector
            if (weaponIcons[i] == null)
            {
                Debug.LogWarning("Weapon icon missing at slot " + i);
                continue;
            }

            // If this slot is empty
            if (!slotUsed[i])
            {
                // Remove the sprite and hide the icon
                weaponIcons[i].sprite = null;
                weaponIcons[i].enabled = false;

                // Skip the rest and move on to the next slot
                continue;
            }

            // Show the icon because something is in this slot
            weaponIcons[i].enabled = true;

            // Check if this slot contains meat
            if (slotTypes[i] == InventorySlotType.Meat)
            {
                // Show the meat image
                weaponIcons[i].sprite = meatIcon;

                // Update the amount of meat displayed
                if (meatCountText != null)
                    meatCountText.text = "x" + meatCount;

                // Skip the weapon section below because this slot is meat
                continue;
            }

            // If the slot contains a weapon,
            // display the correct weapon sprite
            switch (weapons[i])
            {
                case WeaponType.Pistol:
                    weaponIcons[i].sprite = pistolIcon;
                    break;

                case WeaponType.SMG:
                    weaponIcons[i].sprite = smgIcon;
                    break;

                case WeaponType.Shotgun:
                    weaponIcons[i].sprite = shotgunIcon;
                    break;

                case WeaponType.Launcher:
                    weaponIcons[i].sprite = launcherIcon;
                    break;
            }
        }
    }
}

//using UnityEngine;
//using UnityEngine.UI;
//using TMPro;

//public class InventoryUI : MonoBehaviour
//{
//    public Image[] itemIcons;
//    public TMP_Text[] countTexts;

//    public Sprite pistolIcon;
//    public Sprite smgIcon;
//    public Sprite shotgunIcon;
//    public Sprite launcherIcon;

//    public Sprite meatIcon;
//    public Sprite medkitIcon;
//    public Sprite shopItemIcon;

//    public void UpdateInventory(InventorySlot[] slots)
//    {
//        for (int i = 0; i < itemIcons.Length; i++)
//        {
//            if (slots[i].itemType == InventoryItemType.Empty)
//            {
//                itemIcons[i].sprite = null;
//                itemIcons[i].enabled = false;
//                countTexts[i].text = "";
//                continue;
//            }

//            itemIcons[i].enabled = true;

//            if (slots[i].itemType == InventoryItemType.Meat)
//            {
//                itemIcons[i].sprite = meatIcon;
//                countTexts[i].text = "x" + slots[i].amount;
//            }

//            if (slots[i].itemType == InventoryItemType.Medkit)
//            {
//                itemIcons[i].sprite = medkitIcon;
//                countTexts[i].text = "x" + slots[i].amount;
//            }

//            if (slots[i].itemType == InventoryItemType.ShopItem)
//            {
//                itemIcons[i].sprite = shopItemIcon;
//                countTexts[i].text = "x" + slots[i].amount;
//            }

//            if (slots[i].itemType == InventoryItemType.Weapon)
//            {
//                countTexts[i].text = "";

//                switch (slots[i].weaponType)
//                {
//                    case WeaponType.Pistol:
//                        itemIcons[i].sprite = pistolIcon;
//                        break;

//                    case WeaponType.SMG:
//                        itemIcons[i].sprite = smgIcon;
//                        break;

//                    case WeaponType.Shotgun:
//                        itemIcons[i].sprite = shotgunIcon;
//                        break;

//                    case WeaponType.Launcher:
//                        itemIcons[i].sprite = launcherIcon;
//                        break;
//                }
//            }
//        }
//    }
//}