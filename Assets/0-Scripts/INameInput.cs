using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class INameInput : MonoBehaviour
{
    [Header("Player 1 UI References")]
    public TMP_InputField p1InputField; // Player 1's InputField
    public TextMeshProUGUI p1DisplayText; // Player 1's name display text

    [Header("Player 2 UI References")]
    public TMP_InputField p2InputField; // Player 2's InputField
    public TextMeshProUGUI p2DisplayText; // Player 2's name display text

    private const int MaxNameLength = 12; // Maximum number of characters allowed for a name
    private const int MaxThaiNameLength = 12; // Maximum number of Thai characters allowed for a name

    private void Start()
    {
        // Set character limit for input fields
        p1InputField.characterLimit = MaxNameLength;
        p2InputField.characterLimit = MaxNameLength;

        // Load saved names for Player 1 and Player 2
        string player1Name = INameData.LoadPlayerName(1);
        string player2Name = INameData.LoadPlayerName(2);

        // Update UI with loaded names or default messages
        p1DisplayText.text = !string.IsNullOrEmpty(player1Name) ? player1Name : "Player 1";
        p2DisplayText.text = !string.IsNullOrEmpty(player2Name) ? player2Name : "Player 2";

        // Add listeners for end edit events
        p1InputField.onEndEdit.AddListener(OnPlayer1NameSubmitted);
        p2InputField.onEndEdit.AddListener(OnPlayer2NameSubmitted);

        // Add listeners for value changed events to handle Thai character input
        p1InputField.onValueChanged.AddListener(OnPlayer1NameChanged);
        p2InputField.onValueChanged.AddListener(OnPlayer2NameChanged);
    }

    // Called when Player 1 finishes typing their name
    public void OnPlayer1NameSubmitted(string playerName)
    {
        playerName = playerName.Trim();

        if (string.IsNullOrEmpty(playerName))
        {
            Debug.LogWarning("Player 1's name cannot be empty!");
            return;
        }

        INameData.SavePlayerName(1, playerName);
        p1DisplayText.text = playerName;
        Debug.Log("Player 1's name saved: " + playerName);
    }

    // Called when Player 2 finishes typing their name
    public void OnPlayer2NameSubmitted(string playerName)
    {
        playerName = playerName.Trim();

        if (string.IsNullOrEmpty(playerName))
        {
            Debug.LogWarning("Player 2's name cannot be empty!");
            return;
        }

        INameData.SavePlayerName(2, playerName);
        p2DisplayText.text = playerName;
        Debug.Log("Player 2's name saved: " + playerName);
    }

    // Called when Player 1's name is changed
    public void OnPlayer1NameChanged(string playerName)
    {
        p1InputField.text = LimitThaiCharacters(playerName, MaxThaiNameLength);
    }

    // Called when Player 2's name is changed
    public void OnPlayer2NameChanged(string playerName)
    {
        p2InputField.text = LimitThaiCharacters(playerName, MaxThaiNameLength);
    }

    // Limit the number of Thai characters in the input
    private string LimitThaiCharacters(string input, int maxLength)
    {
        int thaiCharCount = 0;
        foreach (char c in input)
        {
            if (IsThaiCharacter(c))
            {
                thaiCharCount++;
                if (thaiCharCount > maxLength)
                {
                    return input.Substring(0, input.Length - 1);
                }
            }
        }
        return input;
    }

    // Check if a character is a Thai character
    private bool IsThaiCharacter(char c)
    {
        return c >= 0x0E00 && c <= 0x0E7F;
    }
}
