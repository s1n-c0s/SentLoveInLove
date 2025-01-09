using UnityEngine;
using System.Collections.Generic;

public class GridGenerator : MonoBehaviour
{
    public GameObject cubePrefab;      // Cube prefab for grid generation
    public int gridSize = 10;          // Number of cubes along one axis
    public float spacing = 1.1f;       // Spacing between cubes
    private List<GameObject> targetGroupList = new List<GameObject>();
    private Node[,] nodeGrid;          // 2D array to store Node references

    public List<GameObject> TargetGroupList => targetGroupList; // Public read-only access to the list

    private bool isGridGenerated = false; // Flag to track grid generation

    public void GenerateGrid()
    {
        if (cubePrefab == null)
        {
            Debug.LogError("Cube prefab is not assigned!");
            return;
        }

        // Only generate grid if not already generated
        if (!isGridGenerated)
        {
            ClearGrid(); // Clear existing grid before generating a new one

            nodeGrid = new Node[gridSize, gridSize]; // Initialize the 2D node array

            for (int x = 0; x < gridSize; x++)
            {
                for (int z = 0; z < gridSize; z++)
                {
                    Vector3 position = new Vector3(x * spacing, 0, z * spacing);
                    GameObject cube = Lean.Pool.LeanPool.Spawn(cubePrefab, position, Quaternion.identity, transform);

                    Node node = cube.AddComponent<Node>(); // Add the Node script to the cube
                    nodeGrid[x, z] = node;                // Store reference to the node
                    node.gridPosition = new Vector2Int(x, z); // Save grid coordinates in the Node
                    targetGroupList.Add(cube);            // Add cube to the list
                }
            }

            // Connect the nodes to their neighbors
            ConnectNodes();

            isGridGenerated = true; // Mark grid as generated
            Debug.Log($"{gridSize * gridSize} cubes generated and nodes connected.");
        }
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
        nodeGrid = null;         // Clear the node grid
        isGridGenerated = false; // Reset grid generation flag
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

    private void ConnectNodes()
    {
        for (int x = 0; x < gridSize; x++)
        {
            for (int z = 0; z < gridSize; z++)
            {
                Node currentNode = nodeGrid[x, z];

                // Add neighbors in all 4 cardinal directions
                if (x > 0) currentNode.AddNeighbor(nodeGrid[x - 1, z]); // Left
                if (x < gridSize - 1) currentNode.AddNeighbor(nodeGrid[x + 1, z]); // Right
                if (z > 0) currentNode.AddNeighbor(nodeGrid[x, z - 1]); // Down
                if (z < gridSize - 1) currentNode.AddNeighbor(nodeGrid[x, z + 1]); // Up
            }
        }
        Debug.Log("Nodes successfully connected to their neighbors.");
    }
}
