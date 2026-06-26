using UnityEngine;

public enum WeaponType
{
    Pistol,
    SMG,
    Shotgun,
    Launcher
}

public class WeaponSystem : MonoBehaviour
{
    [Header("Current State")]
    public WeaponType currentWeapon = WeaponType.Pistol;

    [Header("Bullet Setup")]
    public GameObject bulletPrefab;
    public GameObject launcherBulletPrefab;
    public Transform firePoint;

    [Header("Weapon CSV")]
    public TextAsset weaponsCSV;

    float currentDamage;
    int currentAmmo;
    int maxAmmo;
    float reloadTime;
    float fireRate;

    bool isReloading;
    float lastShotTime;

    private PlayerStats playerStats;

    public float CurrentDamage => currentDamage;
    public int CurrentAmmo => currentAmmo;
    public int MaxAmmo => maxAmmo;
    public bool IsReloading => isReloading;

    [Header("Player Weapon Prefabs")]
    public GameObject pistolPrefab;
    public GameObject smgPrefab;
    public GameObject shotgunPrefab;
    public GameObject launcherPrefab;

    public Transform weaponHolder;
    GameObject currentWeaponVisual;

    void Start()
    {
        playerStats = GetComponentInParent<PlayerStats>();

        ApplyWeaponStats(currentWeapon);
        UpdatePlayerVisual();
    }

    public void Initialize()
    {
        playerStats = GetComponentInParent<PlayerStats>();
        ApplyWeaponStats(currentWeapon);
    }

    public void ManualReload()
    {
        if (currentAmmo < maxAmmo && !isReloading)
            StartReload();
    }

    public void SetWeapon(WeaponType weaponType)
    {
        Debug.Log("Picked up weapon: " + weaponType);

        currentWeapon = weaponType;
        ApplyWeaponStats(currentWeapon);
        UpdatePlayerVisual();
    }

    void ApplyWeaponStats(WeaponType weaponType)
    {
        if (weaponsCSV == null)
        {
            Debug.LogWarning("WeaponSystem: weaponsCSV is not assigned.");
            return;
        }

        string[] lines = weaponsCSV.text.Split('\n');
        string targetID = "weapon_" + weaponType.ToString().ToLower();

        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i].Trim();
            if (string.IsNullOrWhiteSpace(line)) continue;

            string[] values = line.Split(',');
            if (values.Length < 7) continue;

            if (values[0] == targetID)
            {
                if (float.TryParse(values[3], out float dmg)) currentDamage = dmg;
                if (int.TryParse(values[4], out int ammo)) maxAmmo = ammo;
                if (float.TryParse(values[5], out float rTime)) reloadTime = rTime;
                if (float.TryParse(values[6], out float fRate)) fireRate = fRate;
                break;
            }
        }

        switch (weaponType)
        {
            case WeaponType.Launcher:
                bulletPrefab = launcherBulletPrefab;
                break;
        }

        currentAmmo = maxAmmo;
        isReloading = false;
    }

    public void TryShoot(Vector2 direction)
    {
        if (isReloading) return;
        if (Time.time - lastShotTime < fireRate) return;

        if (currentAmmo <= 0)
        {
            StartReload();
            return;
        }

        Fire(direction);
        currentAmmo--;
        lastShotTime = Time.time;

        if (currentAmmo <= 0)
            StartReload();
    }

    void UpdatePlayerVisual()
    {
        if (currentWeaponVisual != null)
            Destroy(currentWeaponVisual);

        GameObject prefabToSpawn = currentWeapon switch
        {
            WeaponType.Pistol => pistolPrefab,
            WeaponType.SMG => smgPrefab,
            WeaponType.Shotgun => shotgunPrefab,
            WeaponType.Launcher => launcherPrefab,
            _ => null
        };

        if (prefabToSpawn != null)
        {
            currentWeaponVisual = Instantiate(
                prefabToSpawn,
                weaponHolder.position,
                weaponHolder.rotation,
                weaponHolder
            );
        }
    }

    void Fire(Vector2 direction)
    {
        switch (currentWeapon)
        {
            case WeaponType.Pistol:
                FireSingle();
                break;

            case WeaponType.SMG:
                FireSpread(1, 8f, true);
                break;

            case WeaponType.Shotgun:
                FireSpread(5, 30f, false);
                break;

            case WeaponType.Launcher:
                FireLauncher();
                break;
        }
    }

    float GetFinalDamage()
    {
        if (playerStats == null)
            playerStats = GetComponentInParent<PlayerStats>();

        if (playerStats == null)
            return currentDamage;

        return currentDamage * playerStats.weaponDamageMultiplier;
    }

    void FireSingle()
    {
        GameObject bulletObject = Instantiate(
            bulletPrefab,
            firePoint.position,
            firePoint.rotation
        );

        Bullet bullet = bulletObject.GetComponent<Bullet>();

        if (bullet != null)
        {
            bullet.damage = GetFinalDamage();
            bullet.speed = 20f;
            bullet.lifeTime = 1.8f;
            bullet.hitEffectDestroyTime = 0.2f;
        }
    }

    void FireSpread(int bulletCount, float spreadAngle, bool randomSpread)
    {
        for (int i = 0; i < bulletCount; i++)
        {
            float angleOffset = randomSpread
                ? Random.Range(-spreadAngle, spreadAngle)
                : Mathf.Lerp(-spreadAngle, spreadAngle, i / (float)(bulletCount - 1));

            Quaternion spreadRotation =
                firePoint.rotation * Quaternion.Euler(0, 0, angleOffset);

            GameObject bulletObject = Instantiate(
                bulletPrefab,
                firePoint.position,
                spreadRotation
            );

            Bullet bullet = bulletObject.GetComponent<Bullet>();

            if (bullet != null)
            {
                bullet.damage = GetFinalDamage();
                bullet.speed = 16f;
                bullet.lifeTime = 1.2f;
                bullet.hitEffectDestroyTime = 0.25f;
            }
        }
    }

    void FireLauncher()
    {
        GameObject bulletObject = Instantiate(
            bulletPrefab,
            firePoint.position,
            firePoint.rotation
        );

        Bullet bullet = bulletObject.GetComponent<Bullet>();

        if (bullet != null)
        {
            bullet.damage = GetFinalDamage();
            bullet.speed = 12f;
            bullet.lifeTime = 2.5f;
            bullet.hitEffectDestroyTime = 0.35f;
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