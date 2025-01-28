using UnityEngine;
using UnityEngine.UI;

public class INameData : MonoBehaviour
{
    private const string P1NameKey = "P1Name";
    private const string P2NameKey = "P2Name";
    private const int MaxNameLength = 12; // Maximum number of characters allowed for a name

    // Saves the player's name based on the player index (1 or 2)
    public static void SavePlayerName(int playerIndex, string playerName)
    {
        if (playerName.Length > MaxNameLength)
        {
            playerName = playerName.Substring(0, MaxNameLength);
            Debug.LogWarning($"Player {playerIndex}'s name was too long and has been truncated to: {playerName}");
        }

        if (playerIndex == 1)
        {
            PlayerPrefs.SetString(P1NameKey, playerName);
        }
        else if (playerIndex == 2)
        {
            PlayerPrefs.SetString(P2NameKey, playerName);
        }
        PlayerPrefs.Save();
    }

    // Loads the player's name based on the player index (1 or 2)
    public static string LoadPlayerName(int playerIndex)
    {
        if (playerIndex == 1)
        {
            return PlayerPrefs.GetString(P1NameKey, "");
        }
        else if (playerIndex == 2)
        {
            return PlayerPrefs.GetString(P2NameKey, "");
        }
        return "";
    }

    // Sets the character limit for the InputField
    public static void SetInputFieldCharacterLimit(InputField inputField)
    {
        inputField.characterLimit = MaxNameLength;
    }
}
