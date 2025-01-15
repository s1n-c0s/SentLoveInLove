using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;

public class PackageMover : MonoBehaviour
{
    [SerializeField][Range(0f, 30f)] private float _minSpeed = 5f;
    [SerializeField][Range(0f, 30f)] private float _maxSpeed = 30f;
    [SerializeField] public GameObject targetPerson;

    private float _speed;
    private List<Node> _path = new List<Node>();
    private int _currentNodeIndex = 0;

    // Reset method to initialize the package when pooled
    public void Initialize(GameObject newTarget)
    {
        targetPerson = newTarget;

        // Reset state
        _path.Clear();
        _currentNodeIndex = 0;
        _speed = Random.Range(_minSpeed, _maxSpeed);

        if (targetPerson == null)
        {
            Debug.LogError("Target person not assigned!");
            return;
        }

        // Find path
        Node startNode = FindClosestNode(transform.position);
        Node targetNode = FindClosestNode(targetPerson.transform.position);

        if (startNode != null && targetNode != null)
        {
            _path = Pathfinding.FindPath(startNode, targetNode);
            if (_path.Count > 0)
            {
                Debug.Log($"Path found from {startNode.name} to {targetNode.name}. Starting movement.");
            }
            else
            {
                Debug.LogWarning("No path found between nodes.");
            }
        }
        else
        {
            Debug.LogError("Start or target node is null!");
        }
    }

    private void Update()
    {
        if (_path.Count > 0 && _currentNodeIndex < _path.Count)
        {
            MoveAlongPath();
        }
    }

    private void MoveAlongPath()
    {
        Node targetNode = _path[_currentNodeIndex];
        Vector3 targetPosition = targetNode.transform.position;

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, _speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            _currentNodeIndex++;

            if (_currentNodeIndex >= _path.Count)
            {
                Debug.Log($"Package reached its destination: {targetPerson.name}");
                LeanPool.Despawn(gameObject);
            }
        }
    }

    private Node FindClosestNode(Vector3 position)
    {
        Node closestNode = null;
        float closestDistance = float.MaxValue;

        foreach (Node node in FindObjectsOfType<Node>())
        {
            float distance = Vector3.Distance(position, node.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestNode = node;
            }
        }

        return closestNode;
    }
}
