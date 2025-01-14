using System.Collections.Generic;
using UnityEngine;

public class PackageMover : MonoBehaviour
{
    //public bool _fromPersonA { get; set; }
    [SerializeField][Range(10f, 30f)] private float _minSpeed = 10f;
    [SerializeField][Range(10f, 30f)] private float _maxSpeed = 30f;
    [SerializeField] public GameObject targetPerson;

    private float _speed;
    private List<Node> _path = new List<Node>();
    private int _currentNodeIndex = 0;

    private void Start()
    {
        // Set a random speed within the specified range
        _speed = Random.Range(_minSpeed, _maxSpeed);

        // Determine target person
        //targetPerson = GameObject.FindGameObjectWithTag(_fromPersonA ? "PersonB" : "PersonA");

        if (targetPerson == null)
        {
            Debug.LogError("Target person not found!");
            return;
        }

        // Find the starting and ending nodes
        Node startNode = FindClosestNode(transform.position);
        Node targetNode = FindClosestNode(targetPerson.transform.position);

        if (startNode != null && targetNode != null)
        {
            // Get the path using a pathfinding algorithm
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
        // Move along the path if it exists
        if (_path.Count > 0 && _currentNodeIndex < _path.Count)
        {
            MoveAlongPath();
        }
    }

    private void MoveAlongPath()
    {
        Node targetNode = _path[_currentNodeIndex];
        Vector3 targetPosition = targetNode.transform.position;

        // Move towards the current target node
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, _speed * Time.deltaTime);

        // Check if we've reached the target node
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            _currentNodeIndex++;

            // If we've reached the final node, destroy the package
            if (_currentNodeIndex >= _path.Count)
            {
                Debug.Log($"Package reached its destination: {targetPerson.name}");
                Destroy(gameObject);
            }
        }
    }

    private Node FindClosestNode(Vector3 position)
    {
        // Find the closest node to the given position
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
