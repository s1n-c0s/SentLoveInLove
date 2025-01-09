using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageMover : MonoBehaviour
{
    public GameObject packagePrefab; // Prefab for the package
    public float moveSpeed = 2f;     // Speed of the package movement

    private GameObject currentPackage;

    public void SendPackage(List<Node> path)
    {
        if (packagePrefab == null)
        {
            Debug.LogError("Package prefab is not assigned!");
            return;
        }

        // Spawn the package at the first node's position
        if (currentPackage == null && path.Count > 0)
        {
            currentPackage = Lean.Pool.LeanPool.Spawn(packagePrefab, path[0].transform.position, Quaternion.identity);
        }

        // Start moving the package along the path
        StartCoroutine(MoveAlongPath(path));
    }

    private IEnumerator MoveAlongPath(List<Node> path)
    {
        foreach (Node node in path)
        {
            yield return MoveToNode(node);
        }

        // Optional: Reverse path for return trip
        path.Reverse();
        foreach (Node node in path)
        {
            yield return MoveToNode(node);
        }

        Debug.Log("Package completed its trip!");
    }

    private IEnumerator MoveToNode(Node targetNode)
    {
        while (Vector3.Distance(currentPackage.transform.position, targetNode.transform.position) > 0.1f)
        {
            currentPackage.transform.position = Vector3.MoveTowards(
                currentPackage.transform.position,
                targetNode.transform.position,
                moveSpeed * Time.deltaTime
            );

            yield return null;
        }
    }
}
