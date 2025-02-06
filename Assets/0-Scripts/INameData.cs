using UnityEngine;

public class INameData : MonoBehaviour
{
    private const string P1NameKey = "P1Name";
    private const string P2NameKey = "P2Name";
    private const int MaxNameLength = 12;
    private const string DefaultP1Name = "Player 1";
    private const string DefaultP2Name = "Player 2";

    public static void SavePlayerName(int playerIndex, string playerName)
    {
        playerName = playerName.Length > MaxNameLength ? playerName.Substring(0, MaxNameLength) : playerName;

        string key = playerIndex == 1 ? P1NameKey : P2NameKey;
        PlayerPrefs.SetString(key, playerName);
        PlayerPrefs.Save();
    }

    public static string LoadPlayerName(int playerIndex)
    {
        string key = playerIndex == 1 ? P1NameKey : P2NameKey;
        string defaultName = playerIndex == 1 ? DefaultP1Name : DefaultP2Name;
        string playerName = PlayerPrefs.GetString(key, defaultName); // Return default if no saved name
        // ClearPlayerName(key); // Clear the name upon reloading
        return playerName;
    }

    private void OnDestroy()
    {
        ClearPlayerName(P1NameKey);
        ClearPlayerName(P2NameKey);
    }

    private static void ClearPlayerName(string key)
    {
        PlayerPrefs.DeleteKey(key);
        PlayerPrefs.Save();
    }
}

