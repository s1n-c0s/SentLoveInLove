using UnityEngine;
using System.Collections.Generic;

public class GridGenerator : MonoBehaviour
{
    [SerializeField] private GameObject cubePrefab; // Assign your cube prefab in the Inspector (LeanPool-compatible)
    [SerializeField] private Camera mainCamera;    // Assign the main/top-down camera
    [SerializeField] private int gridSize = 10;    // Number of cubes along one axis
    [SerializeField] private float spacing = 1.1f; // Adjust spacing between cubes
    [SerializeField] private float zoomOutFactor = 1.5f; // Factor to zoom out (1.5 means 50% further away)
    private List<GameObject> targetGroupList = new List<GameObject>();

    void Start()
    {
        GenerateGrid();
        AdjustCameraToFitTargets();
    }

    void GenerateGrid()
    {
        if (cubePrefab == null)
        {
            Debug.LogError("Cube prefab is not assigned!");
            return;
        }

        for (int x = 0; x < gridSize; x++)
        {
            for (int z = 0; z < gridSize; z++)
            {
                Vector3 position = new Vector3(x * spacing, 0, z * spacing);
                GameObject cube = Lean.Pool.LeanPool.Spawn(cubePrefab, position, Quaternion.identity, transform);
                targetGroupList.Add(cube);
            }
        }

        Debug.Log($"{gridSize * gridSize} cubes generated and added to the target group list using Lean Pool.");
    }

    void AdjustCameraToFitTargets()
    {
        if (mainCamera == null)
        {
            Debug.LogError("Main Camera is not assigned!");
            return;
        }

        // Calculate the bounds of all cubes in the target group
        Bounds bounds = new Bounds(targetGroupList[0].transform.position, Vector3.zero);
        foreach (GameObject target in targetGroupList)
        {
            bounds.Encapsulate(target.transform.position);
        }

        // Calculate the camera position and zoom
        Vector3 center = bounds.center;
        float size = Mathf.Max(bounds.size.x, bounds.size.z) / 2f; // Largest dimension
        size *= zoomOutFactor; // Apply the zoom-out factor

        float distance = size / Mathf.Tan(Mathf.Deg2Rad * mainCamera.fieldOfView / 2f); // Distance for perspective

        // Adjust camera position and orthographic size
        mainCamera.transform.position = new Vector3(center.x, distance, center.z); // Top-down position
        mainCamera.transform.LookAt(center); // Look at the center of the grid

        // Optionally set orthographic size for orthographic cameras
        if (mainCamera.orthographic)
        {
            mainCamera.orthographicSize = size;
        }

        Debug.Log("Camera adjusted to fit all generated cubes with zoom-out.");
    }
}
