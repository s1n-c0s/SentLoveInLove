using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class GameLoop : MonoBehaviour
{
    [Header("Gameplay")]
    [SerializeField] private float gameTimeLimit = 35f;
    public float GameTimeLimitSeconds => gameTimeLimit;

    [Header("References")]
    [SerializeField] private PlaceMe placeMe;
    [SerializeField] private DetectTargets detectTargets;
    [SerializeField] private IHowToPlay howToPlay;

    [Header("State")]
    private List<BillboardSprite> billboardSprites = new List<BillboardSprite>();
    private float gameTime;
    public float GameTime => gameTime;
    private bool isGameRunning;

    //[Header("Scene References")]
    public GridGenerator GridGenerator { get; private set; }
    public CameraController CameraController { get; private set; }

    [Header("UI Feedback")]
    [SerializeField] private GameObject fxFirework;

    private void Awake()
    {
        InitializeSceneReferences();
    }

    private void Start()
    {
        gameTime = 0f;
        detectTargets.enabled = false;
    }

    private void Update()
    {
        if (GameManager.Instance.GetCurrentState() != GameManager.GameState.Playing) return;

        if (placeMe.PlacementComplete)
        {
            if (!isGameRunning)
            {
                DisableBillboardSprites();
                SetGame();
            }
        }

        if (isGameRunning)
        {
            gameTime += Time.deltaTime;
            if (gameTime >= gameTimeLimit)
            {
                EndGame();
            }
        }
    }

    private void InitializeSceneReferences()
    {
        GridGenerator = FindObjectOfType<GridGenerator>();
        CameraController = GetComponent<CameraController>();
        placeMe = GetComponent<PlaceMe>();
        detectTargets = FindObjectOfType<DetectTargets>();
    }

    public void StartGame()
    {
        GameManager.Instance.ChangeState(GameManager.GameState.Playing);
        GridGenerator.GenerateGrid();
        CameraController.FocusOnTargets();

        howToPlay = FindObjectOfType<IHowToPlay>();
        howToPlay.ShowHowToPlayPanels();
        placeMe.CanPlace = true;
        fxFirework.SetActive(false);
    }

    private void SetGame()
    {
        placeMe.CanPlace = false;
        howToPlay.HideAllPanels();
        howToPlay.ShowReadyToPlay();
        detectTargets.enabled = true;

        StartCoroutine(UpdateGameTime());
        SoundFX.Instance.PlaySound("Clock");
    }

    private void DisableBillboardSprites()
    {
        if (billboardSprites.Count == 0)
            billboardSprites.AddRange(FindObjectsOfType<BillboardSprite>());

        foreach (var billboardSprite in billboardSprites)
        {
            billboardSprite.enabled = false;
        }
    }

    private IEnumerator UpdateGameTime()
    {
        isGameRunning = true;
        while (gameTime < gameTimeLimit)
        {
            yield return null;
        }

        if (isGameRunning)
        {
            EndGame();
        }
    }

    private void EndGame()
    {
        // SoundFX.Instance.PlaySound("Clock");
        isGameRunning = false;
        GameManager.Instance.ChangeState(GameManager.GameState.EndGame);
        EnableBillboardSprites();
        SwitchToEndCamera();
        fxFirework.SetActive(true);

        // Determine winner
        int winner = PlayerDataManager.Instance.GetWinner();
        EndFXPlayer[] endFXPlayers = FindObjectsOfType<EndFXPlayer>(); // Get both players' EndFXPlayer

        foreach (var fxPlayer in endFXPlayers)
        {
            fxPlayer.PlayAreaFX(); // Activate general area FX

            if (winner == 0 && fxPlayer.CompareTag("PersonA")) // Player A wins
            {
                // Debug.Log("Player A Wins!");
                fxPlayer.PlayCrownFX();
            }
            else if (winner == 1 && fxPlayer.CompareTag("PersonB")) // Player B wins
            {
                // Debug.Log("Player B Wins!");
                fxPlayer.PlayCrownFX();
            }
            else if (winner == -1) // Tie case
            {
                // Debug.Log("It's a Tie!");
                fxPlayer.PlayHeartFX(); // Activate CrownFX for both players
            }
        }
    }

    private void EnableBillboardSprites()
    {
        foreach (var billboardSprite in billboardSprites)
        {
            billboardSprite.enabled = true;
        }
    }

    public void SwitchToEndCamera()
    {
        CameraController.SwitchToCamera(1);
    }

    public void SwitchToMainCamera()
    {
        CameraController.SwitchToCamera(0);
    }
}