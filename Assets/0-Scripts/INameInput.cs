using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerNameInput : MonoBehaviour
{
    [Header("Player 1 UI References")]
    public TMP_InputField p1InputField; // Player 1's InputField
    public TextMeshProUGUI p1DisplayText;     // Player 1's name display text

    [Header("Player 2 UI References")]
    public TMP_InputField p2InputField; // Player 2's InputField
    public TextMeshProUGUI p2DisplayText;     // Player 2's name display text

    private const int MaxNameLength = 12; // Maximum number of characters allowed for a name

    private void Start()
    {
        // Set character limit for input fields
        p1InputField.characterLimit = MaxNameLength;
        p2InputField.characterLimit = MaxNameLength;

        // Load saved names for Player 1 and Player 2
        string player1Name = INameData.LoadPlayerName(1);
        string player2Name = INameData.LoadPlayerName(2);

        // Update UI with loaded names or default messages
        p1DisplayText.text = !string.IsNullOrEmpty(player1Name)
            ? "Welcome back, Player 1: " + player1Name + "!"
            : "Player 1, please enter your name:";

        p2DisplayText.text = !string.IsNullOrEmpty(player2Name)
            ? "Welcome back, Player 2: " + player2Name + "!"
            : "Player 2, please enter your name:";
    }

    // Called when Player 1 submits their name
    public void OnPlayer1NameSubmitted()
    {
        string playerName = p1InputField.text.Trim();

        if (string.IsNullOrEmpty(playerName))
        {
            Debug.LogWarning("Player 1's name cannot be empty!");
            return;
        }

        INameData.SavePlayerName(1, playerName);
        p1DisplayText.text = "Hello, Player 1: " + playerName + "!";
        Debug.Log("Player 1's name saved: " + playerName);
    }

    // Called when Player 2 submits their name
    public void OnPlayer2NameSubmitted()
    {
        string playerName = p2InputField.text.Trim();

        if (string.IsNullOrEmpty(playerName))
        {
            Debug.LogWarning("Player 2's name cannot be empty!");
            return;
        }

        INameData.SavePlayerName(2, playerName);
        p2DisplayText.text = "Hello, Player 2: " + playerName + "!";
        Debug.Log("Player 2's name saved: " + playerName);
    }
}
