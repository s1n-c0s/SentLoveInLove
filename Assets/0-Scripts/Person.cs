using System.Collections.Generic;
using Lean.Pool;
using UnityEngine;

public class Person : MonoBehaviour
{
    [SerializeField] private PackageManager _packageManager;
    private Node _currentNode;

    // To keep track of nodes that already have packages
    private HashSet<Node> _occupiedNodes = new HashSet<Node>();

    public void Initialize(Node assignedNode)
    {
        _currentNode = assignedNode;
    }

    public void SpawnPackageAroundSelf()
    {
        if (_currentNode == null || _packageManager == null)
        {
            Debug.LogWarning($"{gameObject.name} cannot spawn packages because of missing data.");
            return;
        }

        Debug.Log($"Spawning a package around {gameObject.name} at {_currentNode.transform.position}");
        SpawnSinglePackageAroundNode(_currentNode);
    }

    private void SpawnSinglePackageAroundNode(Node centerNode)
    {
        Vector2Int[] directions =
        {
            new Vector2Int(0, 1),  // Top
            new Vector2Int(-1, 0), // Left
            new Vector2Int(1, 0),  // Right
            new Vector2Int(0, -1)  // Bottom
        };

        List<Node> validNodes = new List<Node>();

        foreach (Vector2Int direction in directions)
        {
            Vector2Int neighborPos = centerNode.gridPosition + direction;

            // Check if the neighbor node is valid, walkable, and not already occupied
            if (centerNode.TryGetNeighbor(neighborPos, out Node neighbor) &&
                neighbor.isWalkable &&
                !_occupiedNodes.Contains(neighbor))
            {
                validNodes.Add(neighbor);
            }
        }

        if (validNodes.Count > 0)
        {
            // Select a random node from the valid list
            Node selectedNode = validNodes[Random.Range(0, validNodes.Count)];

            // Use the PackageManager to spawn the package
            _packageManager.SpawnPackageAround(selectedNode.transform.position);

            // Mark this node as occupied
            _occupiedNodes.Add(selectedNode);

            Debug.Log($"Package spawned at {selectedNode.transform.position}");
        }
        else
        {
            Debug.LogWarning($"No available nodes around {gameObject.name} to spawn a package.");
        }
    }

    // private void OnTriggerEnter(Collider other)
    // {
    //     if (other.CompareTag("Package"))
    //     {
    //         LeanPool.Despawn(other.gameObject);
    //         Debug.Log($"Package collected by {gameObject.name}");
    //     }
    // }
}
