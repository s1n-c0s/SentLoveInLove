using System.Collections.Generic;
using UnityEngine;

public class Pathfinding
{
    public static List<Node> FindPath(Node startNode, Node endNode)
    {
        Queue<Node> queue = new Queue<Node>();
        Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
        List<Node> path = new List<Node>();

        queue.Enqueue(startNode);
        cameFrom[startNode] = null;

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
                path.Reverse();
                return path;
            }

            foreach (Node neighbor in current.GetNeighbors())
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
