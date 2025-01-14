using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    private static System.Random random = new System.Random();

    public static List<Node> FindPath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Dictionary<float, Node> openSet = new Dictionary<float, Node>();
        Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
        HashSet<Node> closedSet = new HashSet<Node>();

        openSet.Add(0, startNode);

        while (openSet.Count > 0)
        {
            // Get the node with the lowest priority
            float currentKey = openSet.Keys.Min();
            Node current = openSet[currentKey];
            openSet.Remove(currentKey);

            if (current == endNode)
            {
                // Reconstruct the path
                while (current != startNode)
                {
                    path.Add(current);
                    current = cameFrom[current];
                }
                path.Add(startNode);
                path.Reverse(); // Reverse the path to get the correct order
                return path;
            }

            closedSet.Add(current);

            // Randomize neighbors and explore them
            List<Node> neighbors = new List<Node>(current.GetNeighbors());
            neighbors = neighbors.OrderBy(x => random.Next()).ToList(); // Shuffle neighbors

            foreach (Node neighbor in neighbors)
            {
                // Skip if neighbor is already in the cameFrom dictionary or is not walkable
                if (cameFrom.ContainsKey(neighbor) || !neighbor.isWalkable) continue;

                // Calculate a probabilistic priority for the neighbor
                float priority = random.Next(0, 100) + GetHeuristic(neighbor, endNode);

                // Ensure the priority is unique by adding a small unique value
                while (openSet.ContainsKey(priority))
                {
                    priority += 0.0001f;
                }

                // Add to openSet if not already in it
                if (!openSet.ContainsValue(neighbor))
                {
                    openSet.Add(priority, neighbor);
                    cameFrom[neighbor] = current;
                }
            }
        }

        return path; // Return empty path if no path is found
    }

    // Heuristic function (Euclidean distance for simplicity)
    private static float GetHeuristic(Node a, Node b)
    {
        return Vector3.Distance(a.transform.position, b.transform.position);
    }
}
