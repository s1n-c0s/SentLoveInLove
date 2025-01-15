using UnityEngine;
using Lean.Pool;
using System.Collections.Generic;

public class PlaceMe : MonoBehaviour
{
    [SerializeField] private GameObject _prefabA;
    [SerializeField] private GameObject _prefabB;

    public bool CanPlace { get; set; }
    public bool PlacementComplete => placedCount >= MaxPlacedCount;

    private bool isNextPrefabA = true;
    private int placedCount = 0;
    private const int MaxPlacedCount = 2;
    private HashSet<Node> occupiedNodes = new HashSet<Node>();
    private List<Person> placedPersons = new List<Person>();

    private void Update()
    {
        if (CanPlace)
        {
            SelectLocation();
        }
    }

    private void SelectLocation()
    {
        if (PlacementComplete) return;

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

        Person person = spawnedObject.GetComponent<Person>();
        if (person != null)
        {
            person.Initialize(node);
            placedPersons.Add(person);
        }
    }

    public List<Person> GetPlacedPersons()
    {
        return new List<Person>(placedPersons);
    }
}
