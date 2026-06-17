using UnityEngine;

public enum WeaponType
{
    Pistol, // 10 dmg
    SMG, // 5 dmg
    Shotgun, // 15 dmg
    Launcher // 30 dmg
}

public class WeaponSystem : MonoBehaviour
{
    [Header("Current State")]
    public WeaponType currentWeapon = WeaponType.Pistol;

    float currentDamage;
    int currentAmmo;
    int maxAmmo;
    float reloadTime;
    float fireRate;

    bool isReloading;
    float lastShotTime;

    void Start()
    {
        ApplyWeaponStats(currentWeapon);
    }

    public void Initialize()
    {
        ApplyWeaponStats(currentWeapon);
    }

    public void SetWeapon(WeaponType weaponType)
    {
        currentWeapon = weaponType;
        ApplyWeaponStats(currentWeapon);
    }

    void ApplyWeaponStats(WeaponType weaponType)
    {
        switch(weaponType)
        {
            case WeaponType.Pistol:
                currentDamage = 10.0f;
                maxAmmo = 6;
                reloadTime = 2.0f;
                fireRate = 0.5f;
                break;
            case WeaponType.SMG:
                currentDamage = 5.0f;
                maxAmmo = 50;
                reloadTime = 2.0f;
                fireRate = 0.2f;
                break;
            case WeaponType.Shotgun:
                currentDamage = 15.0f;
                maxAmmo = 5;
                reloadTime = 2.5f;
                fireRate = 0.8f;
                break;
            case WeaponType.Launcher:
                currentDamage = 30.0f;
                maxAmmo = 2;
                reloadTime = 3.0f;
                fireRate = 1.0f;
                break;
        }

        currentAmmo = maxAmmo;
        isReloading = false;
    }

    public void TryShoot(Vector2 direction)
    {
        if (isReloading) return;

        if (Time.time - lastShotTime < fireRate)
            return;

        if (currentAmmo <= 0)
        {
            StartReload();
            return;
        }

        Fire(direction);

        currentAmmo--;
        lastShotTime = Time.time;

        if (currentAmmo <= 0)
        {
            StartReload();
        }
    }

    void Fire(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction.normalized, 100.0f);
        if (hit.collider != null)
        {
            // RE-ENABLE ONCE ENEMY SCRIPT IS DONE
            var health = hit.collider.GetComponent<EnemyHealthTest>();
            if (health != null)
            {
                health.TakeDamage(currentDamage);
            }
        }
    }

    void StartReload()
    {
        if (!isReloading)
        {
            isReloading = true;
            Invoke(nameof(FinishReload), reloadTime);
        }
    }

    void FinishReload()
    {
        isReloading = false;
        currentAmmo = maxAmmo;
    }
}
