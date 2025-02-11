using UnityEngine;
using System.Collections.Generic;

public class SoundFX : MonoBehaviour
{
    public static SoundFX Instance { get; private set; }

    [SerializeField] private AudioClip[] soundEffects; // Assign in Inspector
    [SerializeField] private int poolSize = 10; // Number of reusable AudioSources
    [SerializeField] private Vector2 pitchRange = new Vector2(0.9f, 1.1f); // Random pitch variation

    private Queue<AudioSource> audioPool = new Queue<AudioSource>();
    private Dictionary<string, AudioClip> soundDictionary;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Convert array to dictionary for fast lookup
        soundDictionary = new Dictionary<string, AudioClip>();
        foreach (var clip in soundEffects)
        {
            soundDictionary[clip.name] = clip;
        }

        // Initialize pool
        for (int i = 0; i < poolSize; i++)
        {
            CreateNewAudioSource();
        }
    }

    private void CreateNewAudioSource()
    {
        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.playOnAwake = false;
        audioPool.Enqueue(source);
    }

    public void PlaySound(string soundName, float volume = 1f)
    {
        if (!soundDictionary.TryGetValue(soundName, out AudioClip clip))
        {
            Debug.LogWarning($"Sound '{soundName}' not found in SoundEffectManager.");
            return;
        }

        if (audioPool.Count == 0)
        {
            CreateNewAudioSource();
        }

        AudioSource source = audioPool.Dequeue();
        source.clip = clip;
        source.volume = volume;
        source.pitch = Random.Range(pitchRange.x, pitchRange.y); // Apply random pitch
        source.Play();
        StartCoroutine(ReturnToPool(source, clip.length / source.pitch)); // Adjust timing for pitch changes
    }

    private System.Collections.IEnumerator ReturnToPool(AudioSource source, float delay)
    {
        yield return new WaitForSeconds(delay);
        source.Stop();
        audioPool.Enqueue(source);
    }
}
