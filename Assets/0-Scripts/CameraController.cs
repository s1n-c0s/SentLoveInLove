using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private GridGenerator gridGenerator; // Reference to the GridGenerator

    [SerializeField]
    private CinemachineVirtualCamera virtualCamera; // Reference to the Cinemachine Virtual Camera

    [SerializeField]
    private float zoomOutFactor = 1.2f; // Factor to zoom out the camera

    [SerializeField]
    private float birdEyeAngle = 45f; // Angle for the bird's eye view (default: 45 degrees)

    private void Start()
    {
        InitializeSceneReferences();
    }

    public void FocusOnTargets()
    {
        if (gridGenerator == null || virtualCamera == null)
        {
            Debug.LogError("GridGenerator or Virtual Camera is not assigned!");
            return;
        }

        // Calculate the bounds of all targets in the grid
        Bounds bounds = gridGenerator.CalculateGridBounds();

        // Calculate the camera position and zoom to capture the entire grid
        Vector3 center = bounds.center;
        float size = Mathf.Max(bounds.size.x, bounds.size.z);

        // Adjust the orthographic size for the virtual camera
        virtualCamera.m_Lens.OrthographicSize = size * zoomOutFactor;

        // Move the camera in or out if the target is not visible
        float targetSize = size / 3;
        if (targetSize > virtualCamera.m_Lens.OrthographicSize)
        {
            virtualCamera.m_Lens.OrthographicSize = targetSize;
        }
        else
        {
            virtualCamera.m_Lens.OrthographicSize *= 0.9f;
        }

        // Change the camera focus to the center of the grid
        Vector3 cameraPosition = center;
        cameraPosition.z = -virtualCamera.m_Lens.OrthographicSize * (1 / Mathf.Tan(virtualCamera.m_Lens.FieldOfView * Mathf.Deg2Rad));
        cameraPosition.y = virtualCamera.m_Lens.OrthographicSize * Mathf.Sin(birdEyeAngle * Mathf.Deg2Rad);

        virtualCamera.transform.position = cameraPosition;

        virtualCamera.LookAt = new GameObject("CameraFocus").transform;
        virtualCamera.LookAt.position = center;
    }

    private void InitializeSceneReferences()
    {
        gridGenerator = FindAnyObjectByType<GridGenerator>();
        virtualCamera = FindAnyObjectByType<CinemachineVirtualCamera>();
    }
}


