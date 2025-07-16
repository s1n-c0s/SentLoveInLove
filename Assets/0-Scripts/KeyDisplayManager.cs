using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class KeyDisplayManager : MonoBehaviour
{
    private Dictionary<int, TextMeshProUGUI> playerKeyTexts = new Dictionary<int, TextMeshProUGUI>();
    private KeyHandler keyHandler;
    private bool isGamePlaying = true;
   
    private void Awake()
    {
        keyHandler = FindObjectOfType<KeyHandler>();
        if (keyHandler == null)
        {
            Debug.LogError("KeyDisplayManager: Could not find KeyHandler!");
        }
    }

    private void Start()
    {
        // Subscribe to game state changes
        GameManager.GameStateChanged += OnGameStateChanged;
    }

    private void OnDestroy()
    {
        // Unsubscribe from game state changes
        GameManager.GameStateChanged -= OnGameStateChanged;
    }

    private void OnGameStateChanged(GameManager.GameState newState)
    {
        // Disable key displays when game ends or is paused, enable for other states
        isGamePlaying = newState != GameManager.GameState.EndGame && newState != GameManager.GameState.Paused;
        
        // Hide or show all key displays based on game state
        foreach (var kvp in playerKeyTexts)
        {
            if (kvp.Value != null)
            {
                kvp.Value.gameObject.SetActive(isGamePlaying);
            }
        }
    }
   
    private void Update()
    {
        // Only update displays if game is active
        if (isGamePlaying)
        {
            UpdateAllKeyDisplays();
        }
    }
   
    public void RegisterPlayerKeyText(int playerIndex, TextMeshProUGUI keyText)
    {
        if (keyText != null)
        {
            playerKeyTexts[playerIndex] = keyText;
            Debug.Log($"KeyDisplayManager: Registered Player {playerIndex} key text: {keyText.name}");
            
            // Set initial state based on current game state
            if (GameManager.Instance != null)
            {
                var currentState = GameManager.Instance.GetCurrentState();
                isGamePlaying = currentState != GameManager.GameState.EndGame && currentState != GameManager.GameState.Paused;
                keyText.gameObject.SetActive(isGamePlaying);
            }
        }
    }
   
    public void UnregisterPlayerKeyText(int playerIndex)
    {
        if (playerKeyTexts.ContainsKey(playerIndex))
        {
            playerKeyTexts.Remove(playerIndex);
            Debug.Log($"KeyDisplayManager: Unregistered Player {playerIndex} key text");
        }
    }
   
    private void UpdateAllKeyDisplays()
    {
        if (keyHandler == null) return;
       
        foreach (var kvp in playerKeyTexts)
        {
            int playerIndex = kvp.Key;
            TextMeshProUGUI keyText = kvp.Value;
           
            if (keyText != null && keyText.gameObject.activeInHierarchy)
            {
                KeyCode playerKey = GetPlayerKey(playerIndex);
                keyText.text = "Press: " + GetKeyDisplayName(playerKey);
            }
        }
    }
   
    private KeyCode GetPlayerKey(int playerIndex)
    {
        switch (playerIndex)
        {
            case 0: return keyHandler.GetPlayerATargetKey();
            case 1: return keyHandler.GetPlayerBTargetKey();
            default: return KeyCode.None;
        }
    }
   
    private string GetKeyDisplayName(KeyCode key)
    {
        switch (key)
        {
            case KeyCode.UpArrow: return "↑";
            case KeyCode.DownArrow: return "↓";
            case KeyCode.LeftArrow: return "←";
            case KeyCode.RightArrow: return "→";
            case KeyCode.W: return "W";
            case KeyCode.A: return "A";
            case KeyCode.S: return "S";
            case KeyCode.D: return "D";
            default: return key.ToString();
        }
    }
   
    // Alternative: Update specific player's display only
    public void UpdatePlayerKeyDisplay(int playerIndex)
    {
        if (keyHandler == null || !playerKeyTexts.ContainsKey(playerIndex)) return;
       
        TextMeshProUGUI keyText = playerKeyTexts[playerIndex];
        if (keyText != null && keyText.gameObject.activeInHierarchy)
        {
            KeyCode playerKey = GetPlayerKey(playerIndex);
            keyText.text = "Press: " + GetKeyDisplayName(playerKey);
        }
    }
}