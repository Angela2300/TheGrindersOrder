using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Singleton Settings")]
    [SerializeField] private bool persistAcrossScenes = true;

    [Header("Clip Database")]
    public AudioClipDatabase clips;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Volume Settings")]
    [Range(0f, 1f)] public float masterVolume = 1f;
    [Range(0f, 1f)] public float musicVolume = 1f;
    [Range(0f, 1f)] public float sfxVolume = 1f;

    [Header("Debug")]
    public bool logMissingClipWarnings = true;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (persistAcrossScenes)
        {
            DontDestroyOnLoad(gameObject);
        }

        SetupAudioSources();
    }

    private void SetupAudioSources()
    {
        if (musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.loop = true;
            musicSource.playOnAwake = false;
        }

        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.loop = false;
            sfxSource.playOnAwake = false;
        }

        ApplyVolumes();
    }

    private void ApplyVolumes()
    {
        if (musicSource != null)
            musicSource.volume = masterVolume * musicVolume;

        if (sfxSource != null)
            sfxSource.volume = masterVolume * sfxVolume;
    }

    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        if (!ValidateClip(clip, "Music")) return;

        musicSource.clip = clip;
        musicSource.loop = loop;
        musicSource.volume = masterVolume * musicVolume;
        musicSource.Play();
    }

    public void StopMusic()
    {
        if (musicSource != null)
        {
            musicSource.Stop();
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        if (!ValidateClip(clip, "SFX")) return;

        sfxSource.pitch = 1f;
        sfxSource.PlayOneShot(clip, masterVolume * sfxVolume);
    }

    public void PlaySFX(AudioClip clip, float volume)
    {
        if (!ValidateClip(clip, "SFX")) return;

        sfxSource.pitch = 1f;
        sfxSource.PlayOneShot(clip, masterVolume * sfxVolume * Mathf.Clamp01(volume));
    }

    public void PlaySFXRandomPitch(AudioClip clip, float minPitch, float maxPitch)
    {
        if (!ValidateClip(clip, "SFX")) return;

        sfxSource.pitch = Random.Range(minPitch, maxPitch);
        sfxSource.PlayOneShot(clip, masterVolume * sfxVolume);
    }

    public void SetMasterVolume(float value)
    {
        masterVolume = Mathf.Clamp01(value);
        ApplyVolumes();
    }

    public void SetMusicVolume(float value)
    {
        musicVolume = Mathf.Clamp01(value);
        ApplyVolumes();
    }

    public void SetSFXVolume(float value)
    {
        sfxVolume = Mathf.Clamp01(value);
        ApplyVolumes();
    }

    public void PlayPlayerHurt() => PlaySFXRandomPitch(clips.playerHurt, 0.9f, 1.1f);
    public void PlayPlayerDeath() => PlaySFX(clips.playerDeath);

    public void PlayWeaponShoot() => PlaySFXRandomPitch(clips.weaponShoot, 0.95f, 1.05f);
    public void PlayWeaponReload() => PlaySFX(clips.weaponReload);

    public void PlayEnemyHit() => PlaySFXRandomPitch(clips.enemyHit, 0.9f, 1.1f);
    public void PlayEnemyDeath() => PlaySFX(clips.enemyDeath);

    public void PlayLootPickup() => PlaySFXRandomPitch(clips.lootPickup, 0.95f, 1.05f);
    public void PlayMeatPickup() => PlaySFXRandomPitch(clips.meatPickup, 0.95f, 1.05f);
    public void PlayCoinPickup() => PlaySFXRandomPitch(clips.coinPickup, 0.95f, 1.05f);

    public void PlayShopOpen() => PlaySFX(clips.shopOpen);
    public void PlayShopClose() => PlaySFX(clips.shopClose);
    public void PlayUpgradeSuccess() => PlaySFX(clips.upgradeSuccess);
    public void PlayUpgradeFail() => PlaySFX(clips.upgradeFail);
    public void PlaySellSuccess() => PlaySFX(clips.sellSuccess);
    public void PlaySellFail() => PlaySFX(clips.sellFail);

    private bool ValidateClip(AudioClip clip, string category)
    {
        if (clip == null)
        {
            if (logMissingClipWarnings)
            {
                Debug.LogWarning($"[AudioManager] Missing {category} clip. Assign it in the AudioClipDatabase.");
            }

            return false;
        }

        return true;
    }
}