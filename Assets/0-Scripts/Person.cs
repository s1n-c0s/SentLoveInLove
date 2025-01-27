using System.Collections.Generic;
using Lean.Pool;
using UnityEngine;

public class Person : MonoBehaviour
{
    [SerializeField] private List<GameObject> _playerVisuals;
    [SerializeField] private bool isPersonA;
    private int _selectedCharacterIndex;
    [SerializeField] private PackageManager _packageManager;
    private Node _currentNode;

    private void Awake()
    {
        if (CompareTag("PersonA"))
        {
            isPersonA = true;
        }
        else if (CompareTag("PersonB"))
        {
            isPersonA = false;
        }

        if (isPersonA)
        {
            _selectedCharacterIndex = PlayerDataManager.Instance.playerData.SelectCharacterA;
        }
        else
        {
            _selectedCharacterIndex = PlayerDataManager.Instance.playerData.SelectCharacterB;
        }

        if (_playerVisuals.Count > 0)
        {
            SetVisuals(_playerVisuals[_selectedCharacterIndex]);
        }
    }

    private void SetVisuals(GameObject gameObject)
    {
        foreach (var visual in _playerVisuals)
        {
            visual.SetActive(false);
        }

        gameObject.SetActive(true);
    }

    public void Initialize(Node assignedNode)
    {
        _currentNode = assignedNode;
    }

    public void SpawnPackageAroundSelf()
    {
        if (_currentNode == null || _packageManager == null)
        {
            Debug.LogError("CurrentNode or PackageManager is null.");
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

            // Check if the neighbor node is valid, walkable, and does not already have a package
            if (centerNode.TryGetNeighbor(neighborPos, out Node neighbor) &&
                neighbor.isWalkable &&
                !HasPackageOnNode(neighbor))
            {
                validNodes.Add(neighbor);
            }
        }

        if (validNodes.Count > 0)
        {
            // Select a random node from the valid list
            Node selectedNode = validNodes[Random.Range(0, validNodes.Count)];

            // Spawn the package at the selected node
            _packageManager.SpawnPackageAround(selectedNode.transform.position);

            Debug.Log($"Package spawned at {selectedNode.gridPosition}");
        }
        else
        {
            Debug.LogWarning($"No available nodes around {gameObject.name} to spawn a package.");
        }
    }

    private bool HasPackageOnNode(Node node)
    {
        Collider[] colliders = Physics.OverlapSphere(node.transform.position, 0.1f);
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Package"))
            {
                return true; // Node already has a package
            }
        }
        return false;
    }
}
