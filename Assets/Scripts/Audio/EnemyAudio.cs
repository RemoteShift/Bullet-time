using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class EnemyAudio : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

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
        
        audioSource.spatialBlend = 1.0f;
        audioSource.rolloffMode = AudioRolloffMode.Logarithmic;
        audioSource.minDistance = minDistance;
        audioSource.maxDistance = maxDistance;
        audioSource.playOnAwake = false;
    }
    
    public void PlayEnemySound(AudioClip clip, float pitchMin = 0.9f, float pitchMax = 1.1f)
    {
        if (!clip || !audioSource) return;

        var globalVol = AudioManager.Instance 
            ? AudioManager.Instance.sfxVolume * AudioManager.Instance.masterVolume 
            : 1f;

        audioSource.pitch = Random.Range(pitchMin, pitchMax);
        audioSource.PlayOneShot(clip, globalVol);
        audioSource.pitch = 1f;
    }
    
    public void PlayDeathSoundDetached(AudioClip deathClip)
    {
        if (!deathClip) return;
        
        AudioSource.PlayClipAtPoint(deathClip, transform.position);
    }
}