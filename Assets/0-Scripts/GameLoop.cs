using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoop : MonoBehaviour
{
    public GridGenerator gridGenerator; // Reference to the GridGenerator
    public CameraController cameraController; // Reference to the CameraController

    [SerializeField] private PlaceMe _placeMe;

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
        // Generate the grid
        gridGenerator.GenerateGrid();
        // Adjust the camera to fit the grid
        cameraController.AdjustCameraToFitGrid();
        // Enable placement
        _placeMe.CanPlace = true;
    }
}
