using UnityEngine;
using Lean.Pool;
using System.Collections.Generic;

public class PlaceMe : MonoBehaviour
{
    [SerializeField] private GameObject _prefabA;
    [SerializeField] private GameObject _prefabB;
    [SerializeField] private PackageManager packageManager;

    public bool CanPlace = false;
    private bool isNextPrefabA = true;
    private int placedCount = 0;
    private const int MaxPlacedCount = 2;
    private HashSet<Node> occupiedNodes = new HashSet<Node>();
    private Dictionary<GameObject, Node> personNodes = new Dictionary<GameObject, Node>(); // Track prefab-node mapping

    private void Update()
    {
        SelectLocation();
    }

    public void SelectLocation()
    {
        if (!CanPlace || placedCount >= MaxPlacedCount) return;

        if (!Input.GetMouseButtonDown(0)) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Tile"))) return;

        Node node = hit.collider.GetComponent<Node>();
        if (node == null || !node.isWalkable || occupiedNodes.Contains(node)) return;

        Vector3 centerPosition = node.transform.position;
        GameObject prefabToSpawn = isNextPrefabA ? _prefabA : _prefabB;

        if (prefabToSpawn == null) return;

        GameObject spawnedObject = LeanPool.Spawn(prefabToSpawn, centerPosition, Quaternion.identity);
        spawnedObject.transform.SetParent(node.transform);
        Debug.Log($"{prefabToSpawn.name} placed at {centerPosition}");

        isNextPrefabA = !isNextPrefabA;
        placedCount++;
        occupiedNodes.Add(node);
        personNodes[spawnedObject] = node; // Map the spawned prefab to the node

        if (placedCount >= MaxPlacedCount)
        {
            CanPlace = false;
            Debug.Log("Placement limit reached. Placement disabled.");

            // Trigger package spawning for both persons
            SpawnPackagesForPersons();
        }
    }

    private void SpawnPackagesForPersons()
    {
        foreach (KeyValuePair<GameObject, Node> entry in personNodes)
        {
            GameObject person = entry.Key;
            Node node = entry.Value;

            Debug.Log($"Spawning packages around {person.name} at {node.transform.position}");
            SpawnPackagesAroundNode(node);
        }
    }

    private void SpawnPackagesAroundNode(Node centerNode)
    {
        Vector2Int[] directions =
        {
        new Vector2Int(0, 1),  // Top
        new Vector2Int(-1, 0), // Left
        new Vector2Int(1, 0),  // Right
        new Vector2Int(0, -1)  // Down
    };

        List<Vector3> spawnPositions = new List<Vector3>();

        foreach (Vector2Int direction in directions)
        {
            Vector2Int neighborPos = centerNode.gridPosition + direction;

            // Check if the neighbor position is within bounds and valid
            if (centerNode.TryGetNeighbor(neighborPos, out Node neighbor))
            {
                spawnPositions.Add(neighbor.transform.position);
            }
        }

        // Force spawn exactly 3 packages
        int requiredSpawns = 3;
        for (int i = 0; i < requiredSpawns; i++)
        {
            if (spawnPositions.Count > 0)
            {
                // Randomly select a position from the valid list
                int randomIndex = Random.Range(0, spawnPositions.Count);
                Vector3 spawnPosition = spawnPositions[randomIndex];
                spawnPositions.RemoveAt(randomIndex); // Remove position to avoid duplicates

                // Spawn package using the PackageManager
                packageManager.SpawnPackageAt(spawnPosition);
            }
            else
            {
                // If we run out of valid positions, retry with any position already used
                Debug.LogWarning("Not enough unique positions! Re-using positions.");
                Vector3 fallbackPosition = centerNode.transform.position + Random.insideUnitSphere * 0.5f; // Slight offset
                //fallbackPosition.y = centerNode.transform.position.y; // Ensure it's on the correct plane
                packageManager.SpawnPackageAt(fallbackPosition);
            }
        }
    }

}
