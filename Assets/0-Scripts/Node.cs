using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public List<Node> neighbors = new List<Node>(); // List of neighboring nodes
    public bool isWalkable = true;                 // Determines if this node is walkable
    public Vector2Int gridPosition;

    // Optional: To debug and visualize connections
    private void OnDrawGizmos()
    {
        if (neighbors.Count > 0)
        {
            Gizmos.color = Color.green;
            foreach (Node neighbor in neighbors)
            {
                if (neighbor != null)
                {
                    Gizmos.DrawLine(transform.position, neighbor.transform.position);
                }
            }
        }
    }

    public List<Node> GetNeighbors()
    {
        return neighbors;
    }

    // Add a neighbor to this node
    public void AddNeighbor(Node node)
    {
        if (!neighbors.Contains(node))
        {
            neighbors.Add(node);
        }
    }

    // Remove a neighbor from this node
    public void RemoveNeighbor(Node node)
    {
        if (neighbors.Contains(node))
        {
            neighbors.Remove(node);
        }
    }
}
