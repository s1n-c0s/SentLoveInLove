using UnityEngine;
using TMPro;

public class INameDisplay : MonoBehaviour
{
    public int playerIndex; // 1 for Player 1, 2 for Player 2
    public TextMeshProUGUI displayText;

    private void Start()
    {
        displayText = GetComponent<TextMeshProUGUI>();
        // Load and set saved name when the scene starts
        SetName(INameData.LoadPlayerName(playerIndex));
    }

    public void SetName(string playerName)
    {
        displayText.text = playerName;
    }
}
