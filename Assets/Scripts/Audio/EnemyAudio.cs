using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class EnemyAudio : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource flatAudioSource;

    [Header("3D Sound Settings")]
    [SerializeField] private float minDistance = 2f;  // Full volume distance
    [SerializeField] private float maxDistance = 25f; // Zero volume distance

    private void Reset()
    {
        Configure3DAudioSource();
    }

    private void Awake()
    {
        Configure3DAudioSource();
    }

    private void Configure3DAudioSource()
    {
        if (!audioSource) audioSource = GetComponent<AudioSource>();
        if (!flatAudioSource) flatAudioSource = gameObject.AddComponent<AudioSource>();
        
        audioSource.spatialBlend = 1.0f;
        audioSource.rolloffMode = AudioRolloffMode.Linear;
        audioSource.minDistance = minDistance;
        audioSource.maxDistance = maxDistance;
        audioSource.playOnAwake = false;
        audioSource.volume = 1f;

        flatAudioSource.spatialBlend = 0f;
        flatAudioSource.playOnAwake = false;
        flatAudioSource.loop = false;
        flatAudioSource.volume = 1f;
    }
    
    public void PlayEnemySound(AudioClip clip, float pitchMin = 0.9f, float pitchMax = 1.1f, bool loop = false, float volumeMult = 1f)
    {
        if (!clip || !audioSource) return;

        var globalVol = AudioManager.Instance 
            ? AudioManager.Instance.sfxVolume * AudioManager.Instance.masterVolume 
            : 1f;
        var calculatedVolume = globalVol * volumeMult;

        if (loop)
        {
            if (audioSource.clip == clip && audioSource.isPlaying)
            {
                return;
            }

            audioSource.clip = clip;
            audioSource.loop = true;
            audioSource.pitch = Random.Range(pitchMin, pitchMax);
            audioSource.volume = calculatedVolume;
            audioSource.Play();
            return;
        }

        audioSource.pitch = Random.Range(pitchMin, pitchMax);
        audioSource.volume = calculatedVolume;
        audioSource.PlayOneShot(clip, calculatedVolume);
        audioSource.pitch = 1f;
    }

    public void PlayEnemySound2D(AudioClip clip, float pitchMin = 0.9f, float pitchMax = 1.1f, bool loop = false, float volumeMult = 1f)
    {
        if (!clip || !flatAudioSource) return;

        var globalVol = AudioManager.Instance
            ? AudioManager.Instance.sfxVolume * AudioManager.Instance.masterVolume
            : 1f;
        var calculatedVolume = globalVol * volumeMult;

        if (loop)
        {
            if (flatAudioSource.clip == clip && flatAudioSource.isPlaying)
            {
                return;
            }

            flatAudioSource.Stop();
            flatAudioSource.clip = clip;
            flatAudioSource.loop = true;
            flatAudioSource.pitch = Random.Range(pitchMin, pitchMax);
            flatAudioSource.volume = calculatedVolume;
            flatAudioSource.Play();
            return;
        }

        flatAudioSource.pitch = Random.Range(pitchMin, pitchMax);
        flatAudioSource.volume = calculatedVolume;
        flatAudioSource.PlayOneShot(clip, calculatedVolume);
        flatAudioSource.pitch = 1f;
    }
    
    public void PlayDeathSoundDetached(AudioClip deathClip, float volumeMult = 1f)
    {
        if (!deathClip) return;
        
        var globalVol = AudioManager.Instance 
            ? AudioManager.Instance.sfxVolume * AudioManager.Instance.masterVolume 
            : 1f;

        AudioSource.PlayClipAtPoint(deathClip, transform.position, globalVol * volumeMult);
    }

    public void StopEnemySound()
    {
        if (audioSource)
        {
            audioSource.Stop();
            audioSource.clip = null;
            audioSource.loop = false;
            audioSource.pitch = 1f;
        }

        if (flatAudioSource)
        {
            flatAudioSource.Stop();
            flatAudioSource.clip = null;
            flatAudioSource.loop = false;
            flatAudioSource.pitch = 1f;
        }
    }
}