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

            List<Node> neighbors = new List<Node>(current.GetNeighbors());

            // Randomize the order of neighbors for a more "random" path
            Shuffle(neighbors);

            foreach (Node neighbor in neighbors)
            {
                if (!cameFrom.ContainsKey(neighbor) && neighbor.isWalkable)
                {
                    queue.Enqueue(neighbor);
                    cameFrom[neighbor] = current;
                }
            }
        }

        return path; // Return empty path if no path is found
    }

    // Utility method to shuffle the list
    private static void Shuffle(List<Node> list)
    {
        System.Random rng = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            Node value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
