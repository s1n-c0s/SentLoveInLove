using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IPausePanel : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button quitButton;

    private GameSetting gameSetting;

    private void Start()
    {
        gameSetting = FindObjectOfType<GameSetting>();

        resumeButton.onClick.AddListener(ResumeGame);
        restartButton.onClick.AddListener(RestartGame);
        quitButton.onClick.AddListener(Quit);
    }

    private void ResumeGame()
    {
        GameManager.Instance.TogglePause();
    }

    private void RestartGame()
    {
        gameSetting.ResetApp();
    }

    private void Quit()
    {
        gameSetting.ExitApp();
    }
}
