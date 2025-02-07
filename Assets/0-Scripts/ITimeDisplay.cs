using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ITimeDisplay : MonoBehaviour
{
    [SerializeField] GameLoop gameLoop;
    [SerializeField] TextMeshProUGUI timeDisplay;

    void Start()
    {
        timeDisplay = GetComponent<TextMeshProUGUI>();
        gameLoop = FindObjectOfType<GameLoop>();
    }

    private void LateUpdate()
    {
        // timeDisplay.text = ((int)gameLoop.GameTimeLimit).ToString("00");
    }
}
