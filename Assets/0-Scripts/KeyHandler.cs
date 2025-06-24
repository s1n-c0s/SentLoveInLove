using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class PlayerKeyConfig
{
    [Header("Player Settings")]
    public string playerName;
    public KeyCode[] availableKeys;
    public int personIndex;
    public bool isPersonA;
    
    [Header("Current State")]
    [SerializeField] private KeyCode currentRandomKey;
    public KeyCode CurrentRandomKey => currentRandomKey;
    
    [Header("Runtime References")]
    [System.NonSerialized]
    public ISpamKey spamKey;
    
    public void SetRandomKey()
    {
        if (availableKeys.Length > 0)
        {
            int randomIndex = Random.Range(0, availableKeys.Length);
            currentRandomKey = availableKeys[randomIndex];
        }
    }
    
    public bool IsCorrectKey(KeyCode pressedKey)
    {
        return pressedKey == currentRandomKey;
    }
}

public class KeyHandler : MonoBehaviour
{
    [Header("Player Configurations")]
    [SerializeField] private PlayerKeyConfig[] playerConfigs = new PlayerKeyConfig[]
    {
        new PlayerKeyConfig 
        { 
            playerName = "Player A", 
            availableKeys = new KeyCode[] { KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D },
            personIndex = 0, 
            isPersonA = true 
        },
        new PlayerKeyConfig 
        { 
            playerName = "Player B", 
            availableKeys = new KeyCode[] { KeyCode.UpArrow, KeyCode.LeftArrow, KeyCode.DownArrow, KeyCode.RightArrow },
            personIndex = 1, 
            isPersonA = false 
        }
    };
    
    [Header("References")]
    private PlaceMe placeMe;
    private List<Person> placedPersons;
    
    [Header("Debug Info")]
    [SerializeField] private bool showDebugInfo = true;

    private void Start()
    {
        InitializeReferences();
        GenerateRandomKeysForAllPlayers();
    }

    private void Update()
    {
        if (!CanProcessInput()) return;

        // Check input for all players
        foreach (var config in playerConfigs)
        {
            CheckPlayerInput(config);
        }
    }

    private void CheckPlayerInput(PlayerKeyConfig config)
    {
        // Check all available keys for this player
        foreach (KeyCode key in config.availableKeys)
        {
            if (Input.GetKeyDown(key))
            {
                if (config.IsCorrectKey(key))
                {
                    // Correct key pressed!
                    HandleCorrectKeyPress(config, key);
                }
                else
                {
                    // Wrong key pressed
                    HandleWrongKeyPress(config, key);
                }
                break; // Only process one key press per frame per player
            }
        }
    }

    private void HandleCorrectKeyPress(PlayerKeyConfig config, KeyCode pressedKey)
    {
        if (showDebugInfo)
            Debug.Log($"{config.playerName} pressed correct key: {pressedKey}");
        
        // Increment appropriate button press count
        if (config.isPersonA)
            PlayerDataManager.Instance.IncrementButtonPressA();
        else
            PlayerDataManager.Instance.IncrementButtonPressB();
        
        // Spawn packages for the specified person
        SpawnPackagesForPerson(config);
        
        // UI feedback
        config.spamKey?.OnKeyPress();
        
        // Generate new random key for this player
        config.SetRandomKey();
        
        if (showDebugInfo)
            Debug.Log($"{config.playerName} new target key: {config.CurrentRandomKey}");
        
        // Optional: Add sound effect for correct key
        // SoundFX.Instance.PlaySound("CorrectKey");
    }

    private void HandleWrongKeyPress(PlayerKeyConfig config, KeyCode pressedKey)
    {
        if (showDebugInfo)
            Debug.Log($"{config.playerName} pressed wrong key: {pressedKey}. Expected: {config.CurrentRandomKey}");
        
        // Optional: Add penalty or wrong key feedback
        // SoundFX.Instance.PlaySound("WrongKey");
        
        // Optional: You could add penalties here like:
        // - Reduce score
        // - Add delay
        // - Show wrong key UI feedback
    }

    private void InitializeReferences()
    {
        placeMe = FindObjectOfType<PlaceMe>();
        
        // Find spam keys for each player
        ISpamKey[] spamKeys = FindObjectsOfType<ISpamKey>();
        
        foreach (var config in playerConfigs)
        {
            foreach (var spamKey in spamKeys)
            {
                if (spamKey.IsPersonA() == config.isPersonA)
                {
                    config.spamKey = spamKey;
                    break;
                }
            }
        }
    }

    private void GenerateRandomKeysForAllPlayers()
    {
        foreach (var config in playerConfigs)
        {
            config.SetRandomKey();
            if (showDebugInfo)
                Debug.Log($"{config.playerName} initial target key: {config.CurrentRandomKey}");
        }
    }

    private bool CanProcessInput()
    {
        // Check if game is in playing state
        if (GameManager.Instance.GetCurrentState() != GameManager.GameState.Playing)
            return false;

        // Check if placement is complete
        if (placeMe == null || !placeMe.PlacementComplete)
            return false;

        // Update placed persons list
        placedPersons = placeMe.GetPlacedPersons();
        
        return placedPersons != null;
    }

    private void SpawnPackagesForPerson(PlayerKeyConfig config)
    {
        if (config.personIndex >= placedPersons.Count)
        {
            Debug.LogWarning($"{config.playerName} at index {config.personIndex} not found!");
            return;
        }

        Person person = placedPersons[config.personIndex];
        Debug.Log($"{config.playerName}: Spawning packages around {person.name}...");
        person.SpawnPackageAroundSelf();
    }

    // Public methods for accessing current target keys (for UI display)
    public KeyCode GetCurrentTargetKey(int playerIndex)
    {
        if (playerIndex >= 0 && playerIndex < playerConfigs.Length)
        {
            return playerConfigs[playerIndex].CurrentRandomKey;
        }
        return KeyCode.None;
    }

    public KeyCode GetPlayerATargetKey()
    {
        return GetCurrentTargetKey(0);
    }

    public KeyCode GetPlayerBTargetKey()
    {
        return GetCurrentTargetKey(1);
    }

    // Method to manually generate new random keys (if needed)
    public void GenerateNewRandomKeys()
    {
        GenerateRandomKeysForAllPlayers();
    }

    // Method to get player name by index
    public string GetPlayerName(int playerIndex)
    {
        if (playerIndex >= 0 && playerIndex < playerConfigs.Length)
        {
            return playerConfigs[playerIndex].playerName;
        }
        return "Unknown Player";
    }
}