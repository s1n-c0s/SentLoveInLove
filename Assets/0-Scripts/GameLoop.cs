using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameLoop : MonoBehaviour
{
    [SerializeField] private float gameTimeLimit = 35f;
    public GridGenerator gridGenerator;
    public CameraController cameraController;
    [SerializeField] private PlaceMe _placeMe;
    [SerializeField] private DetectTargets dectectTarget;

    private List<Person> placedPersons;
    private float gameTime;

    private void Awake()
    {
        InitializeSceneReferences();
    }

    private void Start()
    {
        gameTime = 0f;
        dectectTarget.enabled = false;
    }

    private void InitializeSceneReferences()
    {
        gridGenerator = FindAnyObjectByType<GridGenerator>();
        cameraController = GetComponent<CameraController>();
        _placeMe = GetComponent<PlaceMe>();
        dectectTarget = FindAnyObjectByType<DetectTargets>();
    }

    public void StartGame()
    {
        GameManager.Instance.ChangeState(GameManager.GameState.Playing);
        gridGenerator.GenerateGrid();
        cameraController.FocusOnTargets();
        _placeMe.CanPlace = true;
    }

    private void Update()
    {
        if (GameManager.Instance.GetCurrentState() == GameManager.GameState.Playing)
        {
            if (_placeMe.PlacementComplete)
            {
                UpdateGameTime();
                dectectTarget.enabled = true;
            }
            HandlePlacementInput();
        }
    }

    private void UpdateGameTime()
    {
        gameTime += Time.deltaTime;
        Debug.Log(gameTime);
        if (gameTime >= gameTimeLimit)
        {
            GameManager.Instance.ChangeState(GameManager.GameState.EndGame);
            SwitchToEndCamera();
        }
    }

    private void HandlePlacementInput()
    {
        if (_placeMe.PlacementComplete)
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
        placedPersons = _placeMe.GetPlacedPersons();

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
        cameraController.SwitchToCamera(1); // Switch to endVirtualCamera
    }

    public void SwitchToMainCamera()
    {
        cameraController.SwitchToCamera(0); // Switch to mainVirtualCamera
    }
}