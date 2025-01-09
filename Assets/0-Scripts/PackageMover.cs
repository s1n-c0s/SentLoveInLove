using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;

public class PackageMover : MonoBehaviour
{
    [SerializeField] private GameObject packagePrefab;
    private GameObject currentPackage;
    private bool isMoving;
    private Coroutine moveCoroutine;

    // Define the event that will be called when the package completes its journey
    public event Action OnPackageJourneyComplete;

    // Send package along the provided path
    public void SendPackage(List<Node> path)
    {
        if (packagePrefab == null)
        {
            Debug.LogError("Package prefab is not assigned!");
            return;
        }

        // If no package is currently moving, spawn a new package and move it
        if (currentPackage == null && path.Count > 0)
        {
            currentPackage = Lean.Pool.LeanPool.Spawn(packagePrefab, path[0].transform.position, Quaternion.identity);
        }

        if (!isMoving)
        {
            // Start moving package along the path
            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine); // Stop any existing coroutines
            }
            moveCoroutine = StartCoroutine(MoveAlongPath(path)); // Start the movement along the path
        }
    }

    private IEnumerator MoveAlongPath(List<Node> path)
    {
        if (path == null || path.Count == 0)
        {
            Debug.LogError("Path is empty or null!");
            yield break;
        }

        isMoving = true;

        foreach (Node node in path)
        {
            if (node == null)
            {
                Debug.LogError("Node in path is null!");
                continue;
            }

            yield return StartCoroutine(MoveToNode(node)); // Call MoveToNode as a coroutine
        }

        // After reaching the final node, trigger the event
        Debug.Log("Package completed its journey!");

        // Trigger the event
        OnPackageJourneyComplete?.Invoke(); // Invoke the event

        isMoving = false;
    }

    // Move the package to the node's position more efficiently
    private IEnumerator MoveToNode(Node node)
    {
        if (node == null)
        {
            Debug.LogError("Node is null!");
            yield break;
        }

        Vector3 startPosition = currentPackage.transform.position;
        Vector3 targetPosition = node.transform.position;
        float distance = Vector3.Distance(startPosition, targetPosition);
        float duration = distance / 5f; // Assuming a speed of 5 units per second
        float elapsed = 0f;

        while (elapsed < duration)
        {
            currentPackage.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        currentPackage.transform.position = targetPosition; // Ensure the package reaches the target
    }
}
