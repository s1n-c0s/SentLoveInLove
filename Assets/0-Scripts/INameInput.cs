using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class INameInput : MonoBehaviour
{
    [Header("Player 1 UI References")]
    public TMP_InputField p1InputField;

    [Header("Player 2 UI References")]
    public TMP_InputField p2InputField;

    private const int MaxNameLength = 12;

    private void Start()
    {
        p1InputField.characterLimit = MaxNameLength;
        p2InputField.characterLimit = MaxNameLength;

        p1InputField.onEndEdit.AddListener(playerName => OnPlayerNameSubmitted(1, playerName));
        p2InputField.onEndEdit.AddListener(playerName => OnPlayerNameSubmitted(2, playerName));
    }

    private void OnPlayerNameSubmitted(int playerIndex, string playerName)
    {
        playerName = playerName.Trim();
        if (string.IsNullOrEmpty(playerName))
        {
            Debug.LogWarning($"Player {playerIndex}'s name cannot be empty!");
            return;
        }
        INameData.SavePlayerName(playerIndex, playerName);
        
        // Find the respective INameDisplay and update it
        foreach (INameDisplay display in FindObjectsOfType<INameDisplay>())
        {
            if (display.playerIndex == playerIndex)
            {
                display.SetName(playerName);
                break;
            }
        }
    }
}
