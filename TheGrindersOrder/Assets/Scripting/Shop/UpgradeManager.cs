using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    private PlayerStats playerStats;

    private void Awake()
    {
        playerStats = Object.FindFirstObjectByType<PlayerStats>();
    }

    private void OnEnable()
    {
        ShopSystem.OnUpgradePurchased += ApplyUpgrade;
    }

    private void OnDisable()
    {
        ShopSystem.OnUpgradePurchased -= ApplyUpgrade;
    }

    private void ApplyUpgrade(string category, int newLevel, string effectType, float effectValue)
    {
        if (playerStats == null)
        {
            return;
        }

        switch (effectType)
        {
            case "max_hearts":
                playerStats.SetMaxHealth((int)effectValue);
                break;

            case "damage_reduction":
                playerStats.SetArmor(effectValue);
                break;

            case "speed_multiplier":
                playerStats.SetSpeedMultiplier(effectValue);
                break;

            case "damage_multiplier":
                playerStats.SetWeaponDamageMultiplier(effectValue);
                break;

            default:

                break;
        }

        Debug.Log($"[UpgradeManager] Applied {category} Lv {newLevel}: {effectType} = {effectValue}");
    }
}