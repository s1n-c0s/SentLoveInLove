using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class KeyDisplayManager : MonoBehaviour
{
    private Dictionary<int, TextMeshProUGUI> playerKeyTexts = new Dictionary<int, TextMeshProUGUI>();
    private KeyHandler keyHandler;
    
    private void Awake()
    {
        keyHandler = FindObjectOfType<KeyHandler>();
        if (keyHandler == null)
        {
            Debug.LogError("KeyDisplayManager: Could not find KeyHandler!");
        }
    }
    
    private void Update()
    {
        UpdateAllKeyDisplays();
    }
    
    public void RegisterPlayerKeyText(int playerIndex, TextMeshProUGUI keyText)
    {
        if (keyText != null)
        {
            playerKeyTexts[playerIndex] = keyText;
            Debug.Log($"KeyDisplayManager: Registered Player {playerIndex} key text: {keyText.name}");
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
            
            if (keyText != null)
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
        if (keyText != null)
        {
            KeyCode playerKey = GetPlayerKey(playerIndex);
            keyText.text = "Press: " + GetKeyDisplayName(playerKey);
        }
    }
}