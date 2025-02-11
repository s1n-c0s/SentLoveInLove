using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IMenuPanel : MonoBehaviour
{
    [SerializeField] private Button PlayButton;
    [SerializeField] private Button quitButton;

    private GameLoop _gameLoop;
    private GameSetting gameSetting;

    private void Start()
    {
        gameSetting = FindObjectOfType<GameSetting>();
        _gameLoop = FindObjectOfType<GameLoop>();

        // resumeButton.onClick.AddListener(ResumeGame);
        // restartButton.onClick.AddListener(RestartGame);
        PlayButton.onClick.AddListener(PlayGame);
        quitButton.onClick.AddListener(Quit);
    }


    private void PlayGame()
    {
        _gameLoop.StartGame();
        // GameManager.Instance.ChangeState(GameManager.GameState.Playing);
    }
    // private void ResumeGame()
    // {
    //     GameManager.Instance.TogglePause();
    // }


    // private void RestartGame()
    // {
    //     gameSetting.ResetApp();
    // }

    private void Quit()
    {
        gameSetting.ExitApp();
    }
}
