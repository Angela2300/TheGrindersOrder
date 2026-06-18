using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Image[] slotImages;

    public Sprite emptySlotSprite;
    public Sprite pistolSprite;
    public Sprite smgSprite;
    public Sprite shotgunSprite;
    public Sprite launcherSprite;

    public void UpdateInventory(WeaponType[] weapons, bool[] slotUsed)
    {
        for (int i = 0; i < slotImages.Length; i++)
        {
            if (!slotUsed[i])
            {
                slotImages[i].sprite = emptySlotSprite;
                continue;
            }

            switch (weapons[i])
            {
                case WeaponType.Pistol:
                    slotImages[i].sprite = pistolSprite;
                    break;

                case WeaponType.SMG:
                    slotImages[i].sprite = smgSprite;
                    break;

                case WeaponType.Shotgun:
                    slotImages[i].sprite = shotgunSprite;
                    break;

                case WeaponType.Launcher:
                    slotImages[i].sprite = launcherSprite;
                    break;
            }
        }
    }
}