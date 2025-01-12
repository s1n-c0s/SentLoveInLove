using UnityEngine;
using Lean.Pool;
using System.Collections.Generic;

public class PlaceMe : MonoBehaviour
{
    [SerializeField] private GameObject _prefabA;
    [SerializeField] private GameObject _prefabB;

    public bool CanPlace = false; // Flag to allow placement
    private bool isNextPrefabA = true; // Tracks whether the next prefab is A
    private int placedCount = 0; // Tracks the number of placed objects
    private const int MaxPlacedCount = 2; // Maximum number of objects allowed to be placed
    private HashSet<Node> occupiedNodes = new HashSet<Node>(); // Tracks nodes where objects are placed

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
        if (node == null || !node.isWalkable || occupiedNodes.Contains(node)) return; // Prevent placing on the same node

        Vector3 centerPosition = node.transform.position;
        GameObject prefabToSpawn = isNextPrefabA ? _prefabA : _prefabB;

        if (prefabToSpawn == null) return;

        // Spawn the object and set its parent to the node
        GameObject spawnedObject = LeanPool.Spawn(prefabToSpawn, centerPosition, Quaternion.identity);
        spawnedObject.transform.SetParent(node.transform); // Make the node its parent
        Debug.Log($"{prefabToSpawn.name} placed at {centerPosition} ");

        isNextPrefabA = !isNextPrefabA;
        placedCount++;
        occupiedNodes.Add(node); // Mark this node as occupied

        if (placedCount >= MaxPlacedCount)
        {
            CanPlace = false;
            Debug.Log("Placement limit reached. Placement disabled.");
        }
    }
}
