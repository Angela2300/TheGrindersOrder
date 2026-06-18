using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    public Image[] weaponIcons;

    public Sprite pistolIcon;
    public Sprite smgIcon;
    public Sprite shotgunIcon;
    public Sprite launcherIcon;
    public Sprite meatIcon;

    public TMP_Text meatCountText;

    public void UpdateInventory(
        WeaponType[] weapons,
        bool[] slotUsed,
        InventorySlotType[] slotTypes,
        int meatCount
    )
    {
        for (int i = 0; i < weaponIcons.Length; i++)
        {
            if (weaponIcons[i] == null)
            {
                Debug.LogWarning("Weapon icon missing at slot " + i);
                continue;
            }

            if (!slotUsed[i])
            {
                weaponIcons[i].sprite = null;
                weaponIcons[i].enabled = false;
                continue;
            }

            weaponIcons[i].enabled = true;

            if (slotTypes[i] == InventorySlotType.Meat)
            {
                weaponIcons[i].sprite = meatIcon;

                if (meatCountText != null)
                    meatCountText.text = "x" + meatCount;

                continue;
            }

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