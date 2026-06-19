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

    //Updated Bullet
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

    public float CurrentDamage => currentDamage;
    public int CurrentAmmo => currentAmmo;
    public int MaxAmmo => maxAmmo;
    public bool IsReloading => isReloading;

    [Header("Player Weapon Prefabs")]
    public GameObject pistolPrefab;
    public GameObject smgPrefab;
    public GameObject shotgunPrefab;
    public GameObject launcherPrefab;

    public Transform weaponHolder; // Child object where weapons appear

    GameObject currentWeaponVisual;

    public void ManualReload()
    {
        if (currentAmmo < maxAmmo && !isReloading)
            StartReload();
    }

    void Start()
    {
        ApplyWeaponStats(currentWeapon);
        UpdatePlayerVisual();
    }

    public void Initialize()
    {
        ApplyWeaponStats(currentWeapon);
    }

    public void SetWeapon(WeaponType weaponType)
    {
        Debug.Log("Picked up weapon: " + weaponType);

        currentWeapon = weaponType;
        ApplyWeaponStats(currentWeapon);
        UpdatePlayerVisual(); // Update weapon shown on player
    }

    //Updated
    void ApplyWeaponStats(WeaponType weaponType)
    {
        if (weaponsCSV == null)
        {
            Debug.LogWarning("WeaponSystem: weaponsCSV is not assigned.");
            return;
        }

        // Split CSV into rows
        string[] lines = weaponsCSV.text.Split('\n');

        // Convert enum name to match csv
        string targetID = "weapon_" + weaponType.ToString().ToLower();

        // Skip row 0 because it is the header
        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i].Trim();
            if (string.IsNullOrWhiteSpace(line))
                continue;

            string[] values = line.Split(',');
            if (values.Length < 7)
                continue;

            if (values[0] == targetID)
            {
                float dmg, rTime, fRate;
                int ammo;

                if (float.TryParse(values[3], out dmg)) currentDamage = dmg;
                if (int.TryParse(values[4], out ammo)) maxAmmo = ammo;
                if (float.TryParse(values[5], out rTime)) reloadTime = rTime;
                if (float.TryParse(values[6], out fRate)) fireRate = fRate;

                break;
            }
        }

        // choose bullet prefab
        switch (weaponType)
        {
            case WeaponType.Launcher:
                bulletPrefab = launcherBulletPrefab;
                break;
            default:
                bulletPrefab = bulletPrefab;
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

    void UpdatePlayerVisual()
    {
        // Remove old weapon model
        if (currentWeaponVisual != null)
        {
            Destroy(currentWeaponVisual);
        }

        GameObject prefabToSpawn = null;

        switch (currentWeapon)
        {
            case WeaponType.Pistol:
                prefabToSpawn = pistolPrefab;
                break;

            case WeaponType.SMG:
                prefabToSpawn = smgPrefab;
                break;

            case WeaponType.Shotgun:
                prefabToSpawn = shotgunPrefab;
                break;

            case WeaponType.Launcher:
                prefabToSpawn = launcherPrefab;
                break;
        }

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

    // For Pistol & SMG
    void FireSingle()
    {
        GameObject bulletObject = Instantiate(
            bulletPrefab,
            firePoint.position,
            firePoint.rotation
        );

        Bullet bullet = bulletObject.GetComponent<Bullet>();

        if (bullet != null)
            bullet.damage = currentDamage;
    }

    //For ShotGun 
    void FireSpread(int bulletCount, float spreadAngle, bool randomSpread)
    {
        for (int i = 0; i < bulletCount; i++)
        {
            float angleOffset;

            if (randomSpread)
            {
                angleOffset = Random.Range(-spreadAngle, spreadAngle);
            }
            else
            {
                angleOffset = Mathf.Lerp(
                    -spreadAngle,
                    spreadAngle,
                    i / (float)(bulletCount - 1)
                );
            }

            Quaternion spreadRotation =
                firePoint.rotation * Quaternion.Euler(0, 0, angleOffset);

            GameObject bulletObject = Instantiate(
                bulletPrefab,
                firePoint.position,
                spreadRotation
            );

            Bullet bullet = bulletObject.GetComponent<Bullet>();

            if (bullet != null)
                bullet.damage = currentDamage;
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
            bullet.damage = currentDamage;
    
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