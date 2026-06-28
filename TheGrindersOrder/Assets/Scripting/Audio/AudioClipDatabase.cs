using UnityEngine;

[System.Serializable]
public class AudioClipDatabase
{
    [Header("Music")]
    public AudioClip[] musicTracks;

    [Header("Player")]
    public AudioClip playerHurt;
    public AudioClip playerDeath;

    [Header("Weapon")]
    public AudioClip weaponShoot;
    public AudioClip weaponReload;

    [Header("Enemy")]
    public AudioClip enemyHit;
    public AudioClip enemyDeath;

    [Header("Loot")]
    public AudioClip lootPickup;
    public AudioClip meatPickup;
    public AudioClip coinPickup;

    [Header("Shop")]
    public AudioClip shopOpen;
    public AudioClip shopClose;
    public AudioClip upgradeSuccess;
    public AudioClip upgradeFail;
    public AudioClip sellSuccess;
    public AudioClip sellFail;

}