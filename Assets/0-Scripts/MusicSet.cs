using UnityEngine;
using System.Collections.Generic;

public class MusicSet : MonoBehaviour
{
    [System.Serializable]
    public class GameStateMusic
    {
        public GameManager.GameState state;
        public AudioClip clip;
    }

    public List<GameStateMusic> musicTracks; // List of GameState-Music pairs
    private AudioSource audioSource;
    private Dictionary<GameManager.GameState, AudioClip> musicMap;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        musicMap = new Dictionary<GameManager.GameState, AudioClip>();

        // Populate dictionary for faster lookup
        foreach (var track in musicTracks)
        {
            if (!musicMap.ContainsKey(track.state))
            {
                musicMap[track.state] = track.clip;
            }
        }
    }

    private void OnEnable()
    {
        GameManager.GameStateChanged += OnGameStateChanged;
    }

    private void OnDisable()
    {
        GameManager.GameStateChanged -= OnGameStateChanged;
    }

    private void Start()
    {
        if (GameManager.Instance != null)
        {
            OnGameStateChanged(GameManager.Instance.GetCurrentState());
        }
    }

    private void OnGameStateChanged(GameManager.GameState newState)
    {
        if (musicMap.TryGetValue(newState, out AudioClip clip))
        {
            PlayMusic(clip);
        }
    }

    private void PlayMusic(AudioClip clip)
    {
        if (audioSource.clip == clip) return; // Avoid restarting if the same music is already playing

        audioSource.clip = clip;
        audioSource.Play();
    }
}
