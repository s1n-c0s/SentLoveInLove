using UnityEngine;
using System.Collections.Generic;

public class GridGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _cubePrefab;
    [SerializeField] private int _gridWidth = 20;
    [SerializeField] private int _gridHeight = 10;
    [SerializeField] private float _spacing = 1.1f;
    [SerializeField][Range(0, 100)] private int _unwalkableChancePercent = 15; // Percentage chance for unwalkable nodes

    private List<GameObject> _targetGroupList = new List<GameObject>();
    private Node[,] _nodeGrid;
    private bool _isGridGenerated = false;

    public void GenerateGrid()
    {
        if (_cubePrefab == null)
        {
            Debug.LogError("Cube prefab is not assigned!");
            return;
        }

        if (_isGridGenerated) return;

        ClearGrid();

        _nodeGrid = new Node[_gridWidth, _gridHeight];

        for (int x = 0; x < _gridWidth; x++)
        {
            for (int z = 0; z < _gridHeight; z++)
            {
                Vector3 position = new Vector3(x * _spacing, 0, z * _spacing);
                GameObject cube = Lean.Pool.LeanPool.Spawn(_cubePrefab, position, Quaternion.identity, transform);

                Node node = cube.AddComponent<Node>();
                _nodeGrid[x, z] = node;
                node.gridPosition = new Vector2Int(x, z);
                _targetGroupList.Add(cube);
            }
        }

        ConnectNodes();
        RandomizeWalkability();
        _isGridGenerated = true;
        Debug.Log($"{_gridWidth * _gridHeight} cubes generated and nodes connected.");
    }

    public void ClearGrid()
    {
        foreach (GameObject obj in _targetGroupList)
        {
            if (obj != null)
            {
                Lean.Pool.LeanPool.Despawn(obj);
            }
        }
        _targetGroupList.Clear();
        _nodeGrid = null;
        _isGridGenerated = false;
        Debug.Log("Grid cleared.");
    }

    private void ConnectNodes()
    {
        for (int x = 0; x < _gridWidth; x++)
        {
            for (int z = 0; z < _gridHeight; z++)
            {
                Node currentNode = _nodeGrid[x, z];

                if (x > 0) currentNode.AddNeighbor(_nodeGrid[x - 1, z]); // Left
                if (x < _gridWidth - 1) currentNode.AddNeighbor(_nodeGrid[x + 1, z]); // Right
                if (z > 0) currentNode.AddNeighbor(_nodeGrid[x, z - 1]); // Down
                if (z < _gridHeight - 1) currentNode.AddNeighbor(_nodeGrid[x, z + 1]); // Up
            }
        }
    }

    private void RandomizeWalkability()
    {
        foreach (GameObject cube in _targetGroupList)
        {
            Node node = cube.GetComponent<Node>();
            // Convert percentage to a float for Random.value comparison
            float chance = _unwalkableChancePercent / 100f;
            node.isWalkable = Random.value >= chance;

            // Update color only if necessary
            // Renderer renderer = cube.GetComponentInChildren<Renderer>();
            // if (renderer != null)
            // {
            //     renderer.material.color = node.isWalkable ? Color.white : Color.red;
            // }
        }

        EnsureConnectivity();
    }

    private void EnsureConnectivity()
    {
        // Flood-fill to find connected walkable nodes
        HashSet<Node> connectedNodes = new HashSet<Node>();
        Queue<Node> queue = new Queue<Node>();

        // Find the first walkable node to start the flood-fill
        Node startNode = null;
        foreach (Node node in _nodeGrid)
        {
            if (node != null && node.isWalkable)
            {
                startNode = node;
                break;
            }
        }

        if (startNode == null) return; // No walkable nodes

        queue.Enqueue(startNode);
        connectedNodes.Add(startNode);

        while (queue.Count > 0)
        {
            Node current = queue.Dequeue();

            foreach (Node neighbor in current.GetNeighbors())
            {
                if (neighbor != null && neighbor.isWalkable && connectedNodes.Add(neighbor))
                {
                    queue.Enqueue(neighbor);
                }
            }
        }

        // Ensure all walkable nodes are connected
        foreach (Node node in _nodeGrid)
        {
            if (node != null && node.isWalkable && !connectedNodes.Contains(node))
            {
                node.isWalkable = true;

                // Renderer renderer = node.GetComponentInChildren<Renderer>();
                // if (renderer != null)
                // {
                //     renderer.material.color = Color.white;
                // }

                queue.Enqueue(node);
                connectedNodes.Add(node);
            }
        }

        EnsureWalkableNeighbors();
    }

    private void EnsureWalkableNeighbors()
    {
        foreach (Node node in _nodeGrid)
        {
            if (node == null || !node.isWalkable) continue;

            bool hasWalkableNeighbor = false;

            foreach (Node neighbor in node.GetNeighbors())
            {
                if (neighbor != null && neighbor.isWalkable)
                {
                    hasWalkableNeighbor = true;
                    break;
                }
            }

            if (!hasWalkableNeighbor)
            {
                foreach (Node neighbor in node.GetNeighbors())
                {
                    if (neighbor != null && !neighbor.isWalkable)
                    {
                        neighbor.isWalkable = true;

                        // Renderer renderer = neighbor.GetComponentInChildren<Renderer>();
                        // if (renderer != null)
                        // {
                        //     renderer.material.color = Color.white;
                        // }

                        break;
                    }
                }
            }
        }
    }

    public Bounds CalculateGridBounds()
    {
        if (_nodeGrid == null) return new Bounds(Vector3.zero, Vector3.zero);

        Vector3 min = new Vector3(float.MaxValue, 0, float.MaxValue);
        Vector3 max = new Vector3(float.MinValue, 0, float.MinValue);

        // Loop through all the nodes to calculate the bounds
        foreach (Node node in _nodeGrid)
        {
            if (node == null) continue;

            Vector3 position = node.transform.position;

            // Update min and max based on the node's position
            min = Vector3.Min(min, position);
            max = Vector3.Max(max, position);
        }

        // Create a bounds using the min and max points
        Bounds bounds = new Bounds((min + max) / 2f, max - min);
        return bounds;
    }
}

