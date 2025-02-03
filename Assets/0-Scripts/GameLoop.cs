using System.Collections.Generic;
using UnityEngine;

public class GameLoop : MonoBehaviour
{
    public GridGenerator gridGenerator;
    public CameraController cameraController;

    [SerializeField] private PlaceMe _placeMe;
    private List<Person> placedPersons;

    private void InitializeSceneReferences()
    {
        gridGenerator = FindAnyObjectByType<GridGenerator>();
        cameraController = GetComponent<CameraController>();
        _placeMe = GetComponent<PlaceMe>();
    }

    private void Start()
    {
        InitializeSceneReferences();
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
        if (_placeMe.PlacementComplete)
        {
            // Random package spawning for Person A when "I" is pressed
            if (Input.GetKeyDown(KeyCode.U))
            {
                PlayerDataManager.Instance.IncrementButtonPressA();
                SpawnPackagesForPerson(0); // First person (Person A)
            }

            // Random package spawning for Person B when "U" is pressed
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
}