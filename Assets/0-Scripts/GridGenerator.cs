using UnityEngine;
using Lean.Pool;

public class GridGenerator : MonoBehaviour
{
    public GameObject cubePrefab; // Assign your cube prefab in the Inspector
    public int gridSize = 100;    // Number of cubes along one axis
    public float spacing = 1.1f; // Adjust spacing between cubes

    void Start()
    {
        GenerateGrid();
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
            for (int y = 0; y < gridSize; y++)
            {
                Vector3 position = new Vector3(x * spacing, y * spacing, 0);
                LeanPool.Spawn(cubePrefab, position, Quaternion.identity, transform); // Use Lean Pool for spawning
            }
        }

        Debug.Log($"{gridSize * gridSize} cubes generated using Lean Pool.");
    }
}
