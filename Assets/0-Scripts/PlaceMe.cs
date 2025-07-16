// Alternative approach: Store references directly in PlaceMe and update them there
// Modified PlaceMe.cs

using UnityEngine;
using Lean.Pool;
using System.Collections.Generic;
using TMPro;

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
    private bool hasRotatedAfterPlacement = false;
    
    // Store the text components directly
    private TextMeshProUGUI playerAKeyText;
    private TextMeshProUGUI playerBKeyText;
    private KeyHandler keyHandler;
    
    private void Awake()
    {
        keyHandler = FindObjectOfType<KeyHandler>();
        if (keyHandler == null)
        {
            Debug.LogError("PlaceMe: Could not find KeyHandler!");
        }
    }
    
    private void Update()
    {
        if (CanPlace)
        {
            SelectLocation();
        }
        else if (PlacementComplete && !hasRotatedAfterPlacement)
        {
            foreach (Person person in placedPersons)
            {
                person.rotateLookatTogether();
            }
            hasRotatedAfterPlacement = true;
        }
        
        // Update key displays directly here
        UpdateKeyDisplays();
    }
    
    private void UpdateKeyDisplays()
    {
        if (keyHandler == null) return;
        
        if (playerAKeyText != null)
        {
            KeyCode playerAKey = keyHandler.GetPlayerATargetKey();
            playerAKeyText.text = "Press: " + GetKeyDisplayName(playerAKey);
        }
        
        if (playerBKeyText != null)
        {
            KeyCode playerBKey = keyHandler.GetPlayerBTargetKey();
            playerBKeyText.text = "Press: " + GetKeyDisplayName(playerBKey);
        }
    }
    
    private string GetKeyDisplayName(KeyCode key)
    {
        switch (key)
        {
            case KeyCode.UpArrow: return "↑";
            case KeyCode.DownArrow: return "↓";
            case KeyCode.LeftArrow: return "←";
            case KeyCode.RightArrow: return "→";
            case KeyCode.W: return "W";
            case KeyCode.A: return "A";
            case KeyCode.S: return "S";
            case KeyCode.D: return "D";
            default: return key.ToString();
        }
    }
    
    private void SelectLocation()
    {
        if (PlacementComplete)
        {
            CanPlace = false;
            return;
        }
        
        if (!Input.GetMouseButtonDown(0)) return;
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Tile"))) return;
        
        Node node = hit.collider.GetComponent<Node>();
        if (node == null || !node.isWalkable || occupiedNodes.Contains(node)) return;
        
        Vector3 centerPosition = node.transform.position;
        GameObject prefabToSpawn = isNextPrefabA ? _prefabA : _prefabB;
        
        if (prefabToSpawn == null) return;
        
        SoundFX.Instance.PlaySound("ButtonHover");
        
        GameObject spawnedObject = LeanPool.Spawn(prefabToSpawn, centerPosition, Quaternion.identity);
        spawnedObject.transform.SetParent(node.transform);
        
        Debug.Log($"{prefabToSpawn.name} placed at {centerPosition}");
        
        // Store the text component reference
        StoreTextReference(spawnedObject, isNextPrefabA);
        
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
    
    private void StoreTextReference(GameObject spawnedObject, bool isPlayerA)
    {
        TextMeshProUGUI textComponent = spawnedObject.GetComponentInChildren<TextMeshProUGUI>();
        if (textComponent != null)
        {
            if (isPlayerA)
            {
                playerAKeyText = textComponent;
                Debug.Log($"PlaceMe: Stored PlayerA key text reference to {textComponent.name}");
            }
            else
            {
                playerBKeyText = textComponent;
                Debug.Log($"PlaceMe: Stored PlayerB key text reference to {textComponent.name}");
            }
        }
        else
        {
            Debug.LogError($"PlaceMe: No TextMeshProUGUI found in children of {spawnedObject.name}");
        }
    }
    
    public List<Person> GetPlacedPersons()
    {
        return new List<Person>(placedPersons);
    }
}