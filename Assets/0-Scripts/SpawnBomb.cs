using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;

public class SpawnBomb : MonoBehaviour
{
    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private float bombLifetime = 5f; // Time before a bomb despawns
    [SerializeField] private float minDistance = 2.5f; // Minimum distance between bombs
    [SerializeField] private float initialSpawnDelay = 3f; // Delay before first spawn

    private static int currentBombCount = 0;
    private const int maxBombs = 3;
    private static List<SpawnBomb> availableTiles = new List<SpawnBomb>();
    private static List<Vector3> activeBombPositions = new List<Vector3>();

    private bool hasBomb = false;

    private void Awake()
    {
        availableTiles.Add(this);
    }

    private void OnDestroy()
    {
        availableTiles.Remove(this);
    }

    private void Start()
    {
        StartCoroutine(DelayedSpawn());
    }

    private IEnumerator DelayedSpawn()
    {
        yield return new WaitForSeconds(initialSpawnDelay);
        SpawnOnRandomTile();
    }

    private void TrySpawnBomb()
    {
        Vector3 spawnPosition = GetValidSpawnPosition();
        if (!hasBomb && currentBombCount < maxBombs && spawnPosition != Vector3.zero)
        {
            GameObject bomb = LeanPool.Spawn(bombPrefab, spawnPosition + Vector3.up, Quaternion.identity);
            hasBomb = true;
            currentBombCount++;
            activeBombPositions.Add(spawnPosition);

            // Schedule despawn and respawn
            StartCoroutine(HandleBombLifetime(bomb));
        }
    }

    private IEnumerator HandleBombLifetime(GameObject bomb)
    {
        yield return new WaitForSeconds(bombLifetime);

        LeanPool.Despawn(bomb);
        hasBomb = false;
        currentBombCount--;
        activeBombPositions.Remove(transform.position);

        // Spawn new bomb at a random available tile
        SpawnOnRandomTile();
    }

    private static void SpawnOnRandomTile()
    {
        if (currentBombCount >= maxBombs || availableTiles.Count == 0) return;

        List<SpawnBomb> freeTiles = availableTiles.FindAll(tile => !tile.hasBomb);
        if (freeTiles.Count > 0)
        {
            freeTiles[Random.Range(0, freeTiles.Count)].TrySpawnBomb();
        }
    }

    private Vector3 GetValidSpawnPosition()
    {
        for (int i = 0; i < 10; i++) // Try 10 times to find a valid position
        {
            Vector3 randomOffset = new Vector3(
                Random.Range(-0.5f, 0.5f), // Slight random offset
                0f,
                Random.Range(-0.5f, 0.5f)
            );

            Vector3 potentialPosition = transform.position + randomOffset;

            if (IsPositionValid(potentialPosition))
            {
                return potentialPosition;
            }
        }
        return Vector3.zero; // No valid position found
    }

    private static bool IsPositionValid(Vector3 position)
    {
        foreach (var bombPos in activeBombPositions)
        {
            if (Vector3.Distance(position, bombPos) < 2.5f) // Ensure bombs are spread apart
            {
                return false;
            }
        }
        return true;
    }
}
