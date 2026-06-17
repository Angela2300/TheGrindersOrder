using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponTestDriver : MonoBehaviour
{
    public WeaponSystem weaponSystem;

    void Update()
    {
        if (weaponSystem == null) return;

        HandleWeaponSwitch();
        HandleShooting();
    }

    void HandleWeaponSwitch()
    {
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
            weaponSystem.SetWeapon(WeaponType.Pistol);

        if (Keyboard.current.digit2Key.wasPressedThisFrame)
            weaponSystem.SetWeapon(WeaponType.SMG);

        if (Keyboard.current.digit3Key.wasPressedThisFrame)
            weaponSystem.SetWeapon(WeaponType.Shotgun);

        if (Keyboard.current.digit4Key.wasPressedThisFrame)
            weaponSystem.SetWeapon(WeaponType.Launcher);
    }

    void HandleShooting()
    {
        if (Mouse.current.leftButton.isPressed)
        {
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Vector2 dir = (mouseWorld - weaponSystem.transform.position);
            weaponSystem.TryShoot(dir);
        }
    }
}
