using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ITimeDisplay : MonoBehaviour
{
    [SerializeField] private GameLoop gameLoop;
    [SerializeField] private TextMeshProUGUI timeDisplay;

    private void Start()
    {
        timeDisplay = GetComponent<TextMeshProUGUI>();
        gameLoop = FindObjectOfType<GameLoop>();
    }

    private void LateUpdate()
    {
        // Calculate the remaining time
        float remainingTime = gameLoop.GameTimeLimitSeconds - gameLoop.GameTime;

        // Format the remaining time as seconds
        int seconds = Mathf.FloorToInt(remainingTime);

        // Update the timeDisplay with formatted countdown
        timeDisplay.text = string.Format("{0:00}", seconds);
    }
}

