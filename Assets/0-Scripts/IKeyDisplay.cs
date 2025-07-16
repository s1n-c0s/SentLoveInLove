using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IKeyDisplay : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI playerAKeyText;
    [SerializeField] private TextMeshProUGUI playerBKeyText;
    // [SerializeField] private Text playerANameText;
    // [SerializeField] private Text playerBNameText;

    [Header("Settings")]
    [SerializeField] private string keyPrefix = "Press: ";
    // [SerializeField] private bool showPlayerNames = true;

    private KeyHandler keyHandler;

    private void Start()
    {
        keyHandler = FindObjectOfType<KeyHandler>();

        if (keyHandler == null)
        {
            Debug.LogError("KeyDisplayUI: Could not find CombinedKeyHandler!");
            return;
        }

        // // Set player names if enabled
        // if (showPlayerNames)
        // {
        //     if (playerANameText != null)
        //         playerANameText.text = keyHandler.GetPlayerName(0);

        //     if (playerBNameText != null)
        //         playerBNameText.text = keyHandler.GetPlayerName(1);
        // }
    }

    private void Update()
    {
        if (keyHandler == null) return;

        // Update UI with current target keys
        UpdateKeyDisplay();
    }

    private void UpdateKeyDisplay()
    {
        // Update Player A key display
        if (playerAKeyText != null)
        {
            KeyCode playerAKey = keyHandler.GetPlayerATargetKey();
            playerAKeyText.text = keyPrefix + GetKeyDisplayName(playerAKey);
        }

        // Update Player B key display
        if (playerBKeyText != null)
        {
            KeyCode playerBKey = keyHandler.GetPlayerBTargetKey();
            playerBKeyText.text = keyPrefix + GetKeyDisplayName(playerBKey);
        }
    }

    private string GetKeyDisplayName(KeyCode key)
    {
        // Convert KeyCode to more readable names
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
}