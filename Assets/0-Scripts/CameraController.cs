using UnityEngine;
using Cinemachine;
//using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{
    public GridGenerator gridGenerator; // Reference to the GridGenerator
    public CinemachineVirtualCamera virtualCamera; // Reference to the Cinemachine Virtual Camera
    public float zoomOutFactor = 1.2f; // Factor to zoom out the camera

    private void Start()
    {
        InitializeSceneReferences();
        //SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void AdjustCameraToFitGrid()
    {
        if (gridGenerator == null || virtualCamera == null)
        {
            Debug.LogError("GridGenerator or Virtual Camera is not assigned!");
            return;
        }

        // Calculate the bounds of all targets in the grid
        Bounds bounds = gridGenerator.CalculateGridBounds();

        // Calculate the camera position and zoom
        Vector3 center = bounds.center;
        float size = Mathf.Max(bounds.size.x, bounds.size.z) / 2f; // Largest dimension
        size *= zoomOutFactor; // Apply zoom-out factor

        // Adjust the orthographic size for the virtual camera
        virtualCamera.m_Lens.OrthographicSize = size;

        // Set the virtual camera position
        virtualCamera.transform.position = new Vector3(center.x, size + 5f, center.z); // Adjust height for a better view

        // Ensure the camera looks at the grid center
        virtualCamera.transform.LookAt(center);

        // Debug.Log("Camera adjusted to fit the grid with zoom-out factor.");
    }


    private void InitializeSceneReferences()
    {
        gridGenerator = FindAnyObjectByType<GridGenerator>();
        virtualCamera = FindAnyObjectByType<CinemachineVirtualCamera>();
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