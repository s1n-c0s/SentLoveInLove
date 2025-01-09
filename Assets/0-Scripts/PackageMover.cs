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

    public event Action OnPackageJourneyComplete;

    private List<Node> currentPath;
    private int journeyCount = 0;
    private const int maxJourneys = 4;

    public void SendPackage(List<Node> path)
    {
        if (packagePrefab == null)
        {
            Debug.LogError("Package prefab is not assigned!");
            return;
        }

        if (currentPackage == null && path.Count > 0)
        {
            currentPackage = Lean.Pool.LeanPool.Spawn(packagePrefab, path[0].transform.position, Quaternion.identity);
        }

        currentPath = path;

        if (!isMoving)
        {
            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }
            moveCoroutine = StartCoroutine(MoveAlongPath(currentPath));
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

            yield return StartCoroutine(MoveToNode(node));
        }

        journeyCount++;

        if (journeyCount < maxJourneys)
        {
            Debug.Log($"Journey {journeyCount} completed. Reversing path...");

            // Reverse the path and send the package back
            currentPath.Reverse();
            moveCoroutine = StartCoroutine(MoveAlongPath(currentPath));
        }
        else
        {
            Debug.Log("Package completed its journey 4 times!");
            OnPackageJourneyComplete?.Invoke(); // Trigger the event
        }

        isMoving = false;
    }

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
        float duration = distance / 5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            currentPackage.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        currentPackage.transform.position = targetPosition;
    }
}
