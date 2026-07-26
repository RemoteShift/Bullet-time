using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [Header("Global Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource noiseSource;
    [SerializeField] private AudioSource globalSfxSource;
    [SerializeField] private AudioSource loopingSfxSource;

    [Header("Volume Controls")]
    [Range(0f, 1f)] public float masterVolume = 1f;
    [Range(0f, 1f)] public float musicVolume = 0.8f;
    [Range(0f, 1f)] public float noiseVolume = 0.8f;
    [Range(0f, 1f)] public float sfxVolume = 1f;

    protected override void Awake()
    {
        base.Awake();

        if (!musicSource) musicSource = gameObject.AddComponent<AudioSource>();
        if (!noiseSource) noiseSource = gameObject.AddComponent<AudioSource>();
        if (!globalSfxSource) globalSfxSource = gameObject.AddComponent<AudioSource>();
        if (!loopingSfxSource) loopingSfxSource = gameObject.AddComponent<AudioSource>();

        musicSource.loop = true;
        noiseSource.loop = true;
    }

    #region Global 2D SFX
    public void PlayGlobalSFX(AudioClip clip, float volumeMult = 1f, bool loop = false)
    {
        if (!clip) return;

        var calculatedVolume = volumeMult * sfxVolume * masterVolume;

        if (loop)
        {
            if (loopingSfxSource.clip == clip && loopingSfxSource.isPlaying)
            {
                loopingSfxSource.volume = calculatedVolume;
                return;
            }

            loopingSfxSource.clip = clip;
            loopingSfxSource.loop = true;
            loopingSfxSource.volume = calculatedVolume;
            loopingSfxSource.Play();
        }
        else
        {
            globalSfxSource.PlayOneShot(clip, calculatedVolume);
        }
    }

    public void PlayGlobalSFXPitched(AudioClip clip, float pitchMin = 0.9f, float pitchMax = 1.1f, float volumeMult = 1f)
    {
        if (!clip) return;
        globalSfxSource.pitch = Random.Range(pitchMin, pitchMax);
        globalSfxSource.PlayOneShot(clip, volumeMult * sfxVolume * masterVolume);
        globalSfxSource.pitch = 1f;
    }
    
    public void StopGlobalSFX()
    {
        if (!loopingSfxSource) return;
        loopingSfxSource.Stop();
        loopingSfxSource.clip = null;
        loopingSfxSource.loop = false;
    }
    #endregion

    #region Music & Noise
    public void PlayMusic(AudioClip musicClip)
    {
        if (!musicClip)
        {
            musicSource.Stop();
            return;
        }

        if (musicSource.clip == musicClip && musicSource.isPlaying) return;
        musicSource.clip = musicClip;
        musicSource.volume = musicVolume * masterVolume;
        musicSource.Play();
    }

    public void PlayNoise(AudioClip noiseClip)
    {
        if (!noiseClip)
        {
            noiseSource.Stop();
            return;
        }

        if (noiseSource.clip == noiseClip && noiseSource.isPlaying) return;
        noiseSource.clip = noiseClip;
        noiseSource.volume = noiseVolume * masterVolume;
        noiseSource.Play();
    }
    #endregion
}