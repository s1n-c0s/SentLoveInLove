using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pathfinding
{
    // FindRandomPath creates a random path from startNode to endNode
    public static List<Node> FindRandomPath(Node startNode, Node endNode)
    {
        // List to store the path
        List<Node> path = new List<Node>();

        // Queue for breadth-first search (BFS)
        Queue<Node> queue = new Queue<Node>();
        Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();

        queue.Enqueue(startNode);
        cameFrom[startNode] = null;

        // Random number generator for randomizing path direction
        System.Random random = new System.Random();

        while (queue.Count > 0)
        {
            Node current = queue.Dequeue();

            if (current == endNode)
            {
                // Backtrack to construct the path
                Node temp = endNode;
                while (temp != null)
                {
                    path.Add(temp);
                    temp = cameFrom[temp];
                }
                path.Reverse(); // Reverse the path to get the correct order
                return path;
            }

            // Randomize the order of neighbors to make path random
            List<Node> neighbors = new List<Node>(current.GetNeighbors());
            neighbors = neighbors.OrderBy(x => random.Next()).ToList();  // Shuffle neighbors

            foreach (Node neighbor in neighbors)
            {
                if (!cameFrom.ContainsKey(neighbor))
                {
                    queue.Enqueue(neighbor);
                    cameFrom[neighbor] = current;
                }
            }
        }

        return path; // Return empty path if no path is found
    }
}
