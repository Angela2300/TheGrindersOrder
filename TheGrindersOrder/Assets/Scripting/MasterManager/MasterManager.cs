using UnityEngine;

public class MasterManager : MonoBehaviour
{
    public static MasterManager Instance { get; private set; }

    public PlayerController player;
    public WeaponSystem weaponSystem;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        AutoWireReferences();
        InitializeSystems();
    }

    void AutoWireReferences()
    {
        if (player == null)
            player = FindObjectOfType<PlayerController>();

        if (weaponSystem == null && player != null)
            weaponSystem = player.GetComponentInChildren<WeaponSystem>();

        if (weaponSystem == null)
            weaponSystem = FindObjectOfType<WeaponSystem>();
    }

    void InitializeSystems()
    {
        if (weaponSystem != null)
        {
            weaponSystem.Initialize();
            weaponSystem.SetWeapon(WeaponType.Pistol);
        }
    }
}
