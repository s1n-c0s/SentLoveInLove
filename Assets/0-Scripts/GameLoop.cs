using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using PrimeTween;
using System.Collections;

public class GameLoop : MonoBehaviour
{
    [SerializeField] private float gameTimeLimit = 35f;
    public float GameTimeLimitSeconds => gameTimeLimit;
    [SerializeField] private PlaceMe placeMe;
    [SerializeField] private DetectTargets detectTargets;
    [SerializeField] private IHowToPlay howToPlay;

    private List<Person> placedPersons = new List<Person>();
    private List<BillboardSprite> billboardSprites = new List<BillboardSprite>();
    private float gameTime;
    public float GameTime => gameTime;
    private bool isGameRunning;

    public GridGenerator GridGenerator { get; private set; }
    public CameraController CameraController { get; private set; }

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

        HandlePlacementInput();

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
    }

    private void SetGame()
    {
        placeMe.CanPlace = false;
        howToPlay.HideAllPanels();
        howToPlay.ShowReadyToPlay();
        detectTargets.enabled = true;

        StartCoroutine(UpdateGameTime());
    }

    private void DisableBillboardSprites()
    {
        // Cache all billboard sprites once to improve performance
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
        isGameRunning = false;
        GameManager.Instance.ChangeState(GameManager.GameState.EndGame);
        EnableBillboardSprites();
        SwitchToEndCamera();
    }

    private void EnableBillboardSprites()
    {
        foreach (var billboardSprite in billboardSprites)
        {
            billboardSprite.enabled = true;
        }
    }

    private void HandlePlacementInput()
    {
        if (!placeMe.PlacementComplete || placedPersons == null) return;

        if (Input.GetKeyDown(KeyCode.U))
        {
            PlayerDataManager.Instance.IncrementButtonPressA();
            SpawnPackagesForPerson(0); // First person (Person A)
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            PlayerDataManager.Instance.IncrementButtonPressB();
            SpawnPackagesForPerson(1); // Second person (Person B)
        }
    }

    private void SpawnPackagesForPerson(int personIndex)
    {
        placedPersons = placeMe.GetPlacedPersons();
        if (personIndex >= placedPersons.Count)
        {
            Debug.LogWarning($"Person at index {personIndex} not found!");
            return;
        }

        Person person = placedPersons[personIndex];
        Debug.Log($"Spawning packages around {person.name}...");
        person.SpawnPackageAroundSelf();
    }

    public void SwitchToEndCamera()
    {
        CameraController.SwitchToCamera(1); // Switch to endVirtualCamera
    }

    public void SwitchToMainCamera()
    {
        CameraController.SwitchToCamera(0); // Switch to mainVirtualCamera
    }
}
