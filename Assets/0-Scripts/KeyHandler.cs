using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class PlayerKeyConfig
{
    [Header("Player Settings")]
    public string playerName;
    public KeyCode keyCode;
    public int personIndex;
    public bool isPersonA;

    [Header("Runtime References")]
    [System.NonSerialized]
    public ISpamKey spamKey;
}

public class KeyHandler : MonoBehaviour
{
    [Header("Player Configurations")]
    [SerializeField]
    private PlayerKeyConfig[] playerConfigs = new PlayerKeyConfig[]
    {
        new PlayerKeyConfig
        {
            playerName = "Player A",
            keyCode = KeyCode.U,
            personIndex = 0,
            isPersonA = true
        },
        new PlayerKeyConfig
        {
            playerName = "Player B",
            keyCode = KeyCode.I,
            personIndex = 1,
            isPersonA = false
        }
    };

    [Header("References")]
    private PlaceMe placeMe;
    private List<Person> placedPersons;

    private void Start()
    {
        InitializeReferences();
    }

    private void Update()
    {
        if (!CanProcessInput()) return;

        // Handle all configured keys
        foreach (var config in playerConfigs)
        {
            if (Input.GetKeyDown(config.keyCode))
            {
                HandleKeyPress(config);
            }
        }
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

    private void HandleKeyPress(PlayerKeyConfig config)
    {
        // Increment appropriate button press count
        if (config.isPersonA)
            PlayerDataManager.Instance.IncrementButtonPressA();
        else
            PlayerDataManager.Instance.IncrementButtonPressB();

        // Spawn packages for the specified person
        SpawnPackagesForPerson(config);

        // UI feedback
        config.spamKey?.OnKeyPress();

        // Optional: Add sound effect
        // SoundFX.Instance.PlaySound("KeyPress");
    }

    private void SpawnPackagesForPerson(PlayerKeyConfig config)
    {
        if (config.personIndex >= placedPersons.Count)
        {
            Debug.LogWarning($"{config.playerName} at index {config.personIndex} not found!");
            return;
        }

        Person person = placedPersons[config.personIndex];
        Debug.Log($"{config.playerName} ({config.keyCode}): Spawning packages around {person.name}...");
        person.SpawnPackageAroundSelf();
    }

    // Public methods for runtime configuration
    public void SetPlayerKey(int playerIndex, KeyCode newKey)
    {
        if (playerIndex >= 0 && playerIndex < playerConfigs.Length)
        {
            playerConfigs[playerIndex].keyCode = newKey;
        }
    }

    public void AddPlayer(string name, KeyCode key, int personIndex, bool isPersonA)
    {
        var newConfig = new PlayerKeyConfig
        {
            playerName = name,
            keyCode = key,
            personIndex = personIndex,
            isPersonA = isPersonA
        };

        // This would require resizing the array or using a List instead
        // For simplicity, you might want to use List<PlayerKeyConfig> instead of array
    }
}