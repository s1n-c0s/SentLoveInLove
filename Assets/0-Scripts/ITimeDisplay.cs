using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;  // Added to access the Image component

public class ITimeDisplay : MonoBehaviour
{
    [SerializeField] private GameLoop gameLoop;
    [SerializeField] private TextMeshProUGUI timeDisplay;
    [SerializeField] private Image timeFillImage; // Reference to the Image component

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

        // Calculate the fill amount based on the remaining time
        float fillAmount = remainingTime / gameLoop.GameTimeLimitSeconds;

        // Update the fillAmount of the timeFillImage (progress bar)
        if (timeFillImage != null)
        {
            timeFillImage.fillAmount = fillAmount;
        }
    }
}
