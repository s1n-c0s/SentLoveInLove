using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;

public class SpawnBomb : MonoBehaviour
{
    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private float bombLifetime = 5f;
    [SerializeField] private float minDistance = 2.5f;
    [SerializeField] private float initialSpawnDelay = 3f;

    private static int currentBombCount = 0;
    private const int maxBombs = 3;
    private static List<SpawnBomb> availableTiles = new List<SpawnBomb>();  // Ensure this is cleared and repopulated
    private static List<Vector3> activeBombPositions = new List<Vector3>();

    private bool hasBomb = false;
    private Coroutine spawnCoroutine;

    private void OnDestroy()
    {
        availableTiles.Remove(this);
    }

    private void Start()
    {
        availableTiles.Add(this);
        if (GameManager.Instance.GetCurrentState() == GameManager.GameState.Playing)
        {
            spawnCoroutine = StartCoroutine(DelayedSpawn());
        }
    }

    private IEnumerator DelayedSpawn()
    {
        yield return new WaitForSeconds(initialSpawnDelay);
        SpawnOnRandomTile();
    }

    private void TrySpawnBomb()
    {
        if (GameManager.Instance.GetCurrentState() != GameManager.GameState.Playing)
            return;

        Vector3 spawnPosition = GetValidSpawnPosition();
        if (!hasBomb && currentBombCount < maxBombs && spawnPosition != Vector3.zero)
        {
            GameObject bomb = LeanPool.Spawn(bombPrefab, spawnPosition + Vector3.up, Quaternion.identity);
            if (bomb == null) return;

            hasBomb = true;
            currentBombCount++;
            activeBombPositions.Add(spawnPosition);

            StartCoroutine(HandleBombLifetime(bomb));
        }
    }

    private IEnumerator HandleBombLifetime(GameObject bomb)
    {
        yield return new WaitForSeconds(bombLifetime);

        if (bomb != null && bomb.activeInHierarchy)
        {
            LeanPool.Despawn(bomb);
        }

        hasBomb = false;
        currentBombCount--;

        activeBombPositions.RemoveAll(pos => Vector3.Distance(pos, bomb.transform.position) < 0.1f);

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
        for (int i = 0; i < 10; i++)
        {
            Vector3 randomOffset = new Vector3(Random.Range(-0.5f, 0.5f), 0f, Random.Range(-0.5f, 0.5f));
            Vector3 potentialPosition = transform.position + randomOffset;

            if (IsPositionValid(potentialPosition))
            {
                return potentialPosition;
            }
        }
        return Vector3.zero;
    }

    private static bool IsPositionValid(Vector3 position)
    {
        foreach (var bombPos in activeBombPositions)
        {
            if (Vector3.Distance(position, bombPos) < 2.5f)
            {
                return false;
            }
        }
        return true;
    }
}
