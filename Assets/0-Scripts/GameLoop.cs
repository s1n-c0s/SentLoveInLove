using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameLoop : MonoBehaviour
{
    [SerializeField] private float gameTimeLimit = 35f;
    [SerializeField] private PlaceMe placeMe;
    [SerializeField] private DetectTargets detectTargets;

    private List<Person> placedPersons;
    private List<BillboardSprite> billboardSprites = new List<BillboardSprite>();
    private float gameTime;

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

    private void InitializeSceneReferences()
    {
        GridGenerator = FindAnyObjectByType<GridGenerator>();
        CameraController = GetComponent<CameraController>();
        placeMe = GetComponent<PlaceMe>();
        detectTargets = FindAnyObjectByType<DetectTargets>();
    }

    public void StartGame()
    {
        GameManager.Instance.ChangeState(GameManager.GameState.Playing);
        GridGenerator.GenerateGrid();
        CameraController.FocusOnTargets();
        placeMe.CanPlace = true;
    }

    private void Update()
    {
        if (GameManager.Instance.GetCurrentState() == GameManager.GameState.Playing)
        {
            if (placeMe.PlacementComplete)
            {
                DisableBillboardSprites();
                UpdateGameTime();
                detectTargets.enabled = true;
            }
            HandlePlacementInput();
        }
    }

    private void DisableBillboardSprites()
    {
        billboardSprites.AddRange(FindObjectsOfType<BillboardSprite>());
        foreach (var billboardSprite in billboardSprites)
        {
            billboardSprite.enabled = false;
        }
    }

    private void UpdateGameTime()
    {
        gameTime += Time.deltaTime;
        Debug.Log(gameTime);
        if (gameTime >= gameTimeLimit)
        {
            EndGame();
        }
    }

    private void EndGame()
    {
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
        if (placeMe.PlacementComplete)
        {
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
    }

    private void SpawnPackagesForPerson(int personIndex)
    {
        placedPersons = placeMe.GetPlacedPersons();

        if (personIndex < placedPersons.Count)
        {
            Person person = placedPersons[personIndex];
            Debug.Log($"Spawning packages around {person.name}...");
            person.SpawnPackageAroundSelf();
        }
        else
        {
            Debug.LogWarning($"Person at index {personIndex} not found!");
        }
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
