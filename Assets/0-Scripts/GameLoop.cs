using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.SceneManagement;

public class GameLoop : MonoBehaviour
{
    public GridGenerator gridGenerator; // Reference to the GridGenerator
    public CameraController cameraController; // Reference to the CameraController

    private void Start()
    {
        InitializeSceneReferences();
        // SceneManager.sceneLoaded += OnSceneLoaded;
    }


    public void StartGame()
    {
        GameManager.Instance.ChangeState(GameManager.GameState.Playing);
        // Generate the grid
        gridGenerator.GenerateGrid();
        // Adjust the camera to fit the grid
        cameraController.AdjustCameraToFitGrid();
    }

    private void InitializeSceneReferences()
    {
        gridGenerator = FindAnyObjectByType<GridGenerator>();
        cameraController = GetComponent<CameraController>();
    }

    // private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    // {
    //     // Reinitialize references whenever a new scene is loaded
    //     InitializeSceneReferences();
    // }

    // private void OnDestroy()
    // {
    //     // Unsubscribe from the event to avoid memory leaks
    //     SceneManager.sceneLoaded -= OnSceneLoaded;
    // }

}
