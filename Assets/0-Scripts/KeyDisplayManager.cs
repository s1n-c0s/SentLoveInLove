using UnityEngine;
using TMPro;
using System.Collections.Generic;
using PrimeTween;

public class KeyDisplayManager : MonoBehaviour
{
    private Dictionary<int, TextMeshProUGUI> playerKeyTexts = new Dictionary<int, TextMeshProUGUI>();
    private Dictionary<int, string> lastKeyTexts = new Dictionary<int, string>();
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
            lastKeyTexts[playerIndex] = "";
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
            lastKeyTexts.Remove(playerIndex);
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
                string newKeyText = "Press: " + GetKeyDisplayName(playerKey);
                
                // Check if the key has changed
                if (lastKeyTexts.ContainsKey(playerIndex) && lastKeyTexts[playerIndex] != newKeyText)
                {
                    // Update the text
                    keyText.text = newKeyText;
                    lastKeyTexts[playerIndex] = newKeyText;
                    
                    // Play bounce animation
                    PlayBounceAnimation(keyText);
                }
                else if (!lastKeyTexts.ContainsKey(playerIndex))
                {
                    // First time setting the text
                    keyText.text = newKeyText;
                    lastKeyTexts[playerIndex] = newKeyText;
                    PlayBounceAnimation(keyText);
                }
            }
        }
    }
    
    private void PlayBounceAnimation(TextMeshProUGUI keyText)
    {
        // Stop any existing animation on this object
        Tween.StopAll(keyText.transform);
        
        // Bounce animation using Prime Tween
        Tween.Scale(keyText.transform, Vector3.one * 1.2f, 0.1f, Ease.OutQuad)
            .OnComplete(() => {
                Tween.Scale(keyText.transform, Vector3.one, 0.1f, Ease.InQuad);
            });
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
            case KeyCode.UpArrow: return "Up";
            case KeyCode.DownArrow: return "Down";
            case KeyCode.LeftArrow: return "Left";
            case KeyCode.RightArrow: return "Right";
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
            string newKeyText = "Press: " + GetKeyDisplayName(playerKey);
            
            // Check if the key has changed
            if (lastKeyTexts.ContainsKey(playerIndex) && lastKeyTexts[playerIndex] != newKeyText)
            {
                keyText.text = newKeyText;
                lastKeyTexts[playerIndex] = newKeyText;
                PlayBounceAnimation(keyText);
            }
        }
    }
}