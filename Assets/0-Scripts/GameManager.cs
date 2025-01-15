using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private GameLoop _gameLoop;
    private UIManager _uiManager;

    public enum GameState
    {
        MainMenu,
        Playing,
        Paused,
        EndGame
    }

    private GameState currentState;

    public delegate void OnGameStateChanged(GameState newState);
    public static event OnGameStateChanged GameStateChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return; // Prevent further execution in this instance
        }

        // Initialize references for the first scene
        InitializeSceneReferences();
    }

    private void Start()
    {
        // Subscribe to sceneLoaded event to reinitialize references after a scene loads
        SceneManager.sceneLoaded += OnSceneLoaded;

        // Set the initial game state
        ChangeState(GameState.MainMenu);
    }

    private void OnDestroy()
    {
        // Unsubscribe from the event to avoid memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Reinitialize references whenever a new scene is loaded
        InitializeSceneReferences();
    }

    public void InitializeSceneReferences()
    {
        _gameLoop = FindObjectOfType<GameLoop>();
        _uiManager = FindObjectOfType<UIManager>();
    }

    public void ChangeState(GameState newState)
    {
        currentState = newState;

        switch (currentState)
        {
            case GameState.MainMenu:
                Debug.Log("Entering MainMenu state");
                break;
            case GameState.Playing:
                Debug.Log("Entering Playing state");
                HandlePlayingState();
                break;
            case GameState.Paused:
                Debug.Log("Entering Paused state");
                break;
            case GameState.EndGame:
                Debug.Log("Entering EndGame state");
                break;
        }

        GameStateChanged?.Invoke(currentState);
    }

    private void HandlePlayingState()
    {
        if (_uiManager != null)
        {
            _uiManager.HideAllPanels();
        }
    }

    public void TogglePause()
    {
        if (currentState == GameState.Playing)
        {
            ChangeState(GameState.Paused);
        }
        else if (currentState == GameState.Paused)
        {
            ChangeState(GameState.Playing);
        }
    }

    public GameState GetCurrentState()
    {
        return currentState;
    }
}
