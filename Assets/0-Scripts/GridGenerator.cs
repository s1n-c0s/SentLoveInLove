using UnityEngine;
using System.Collections.Generic;

public class GridGenerator : MonoBehaviour
{
    public GameObject cubePrefab;      // Cube prefab for grid generation
    public int gridSize = 10;          // Number of cubes along one axis
    public float spacing = 1.1f;       // Spacing between cubes
    private List<GameObject> targetGroupList = new List<GameObject>();

    public List<GameObject> TargetGroupList => targetGroupList; // Public read-only access to the list

    public void GenerateGrid()
    {
        if (cubePrefab == null)
        {
            Debug.LogError("Cube prefab is not assigned!");
            return;
        }

        ClearGrid(); // Clear existing grid before generating a new one

        for (int x = 0; x < gridSize; x++)
        {
            for (int z = 0; z < gridSize; z++)
            {
                Vector3 position = new Vector3(x * spacing, 0, z * spacing);
                GameObject cube = Lean.Pool.LeanPool.Spawn(cubePrefab, position, Quaternion.identity, transform);
                targetGroupList.Add(cube); // Add each cube to the list
            }
        }

        Debug.Log($"{gridSize * gridSize} cubes generated.");
    }

    public void ClearGrid()
    {
        foreach (GameObject obj in targetGroupList)
        {
            if (obj != null)
            {
                Lean.Pool.LeanPool.Despawn(obj); // Despawn objects using Lean Pool
            }
        }
        targetGroupList.Clear(); // Clear the list
        Debug.Log("Grid cleared.");
    }

    public Bounds CalculateGridBounds()
    {
        if (targetGroupList == null || targetGroupList.Count == 0)
        {
            Debug.LogError("No targets found in the grid!");
            return new Bounds(Vector3.zero, Vector3.zero);
        }

        // Calculate bounds of all objects in the grid
        Bounds bounds = new Bounds(targetGroupList[0].transform.position, Vector3.zero);
        foreach (GameObject target in targetGroupList)
        {
            bounds.Encapsulate(target.transform.position);
        }

        return bounds;
    }
}
