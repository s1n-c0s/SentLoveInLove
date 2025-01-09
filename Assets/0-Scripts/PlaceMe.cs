using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class PlaceMe : MonoBehaviour
{
    [SerializeField] private GameObject Aprefab; // Prefab for the sender
    [SerializeField] private GameObject Bprefab; // Prefab for the receiver
    [SerializeField] private GridGenerator gridGenerator; // Reference to the GridGenerator script
    [SerializeField] private PackageMover packageMover; // Reference to the PackageMover script

    private GameObject senderInstance;
    private GameObject receiverInstance;

    private Node senderNode;
    private Node receiverNode;

    private bool placingSender = true; // Determines whether placing sender or receiver

    void Start()
    {
        InitializeReferences();
        Debug.Log("Click on a grid tile to place the sender (Aprefab).");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Detect left mouse button click
        {
            PlaceObjectWithMouse();
        }
    }

    private void InitializeReferences()
    {
        // Dynamically assign gridGenerator if it's missing
        if (gridGenerator == null)
        {
            gridGenerator = FindObjectOfType<GridGenerator>();
            if (gridGenerator == null)
            {
                Debug.LogError("GridGenerator is not assigned or missing in the scene!");
                return;
            }
        }

        // Dynamically assign packageMover if it's missing
        if (packageMover == null)
        {
            packageMover = FindObjectOfType<PackageMover>();
            if (packageMover == null)
            {
                Debug.LogError("PackageMover is not assigned or missing in the scene!");
                return;
            }
        }

        // Check for missing prefabs
        if (Aprefab == null || Bprefab == null)
        {
            Debug.LogError("Prefabs are not assigned in the inspector!");
        }
    }

    private void PlaceObjectWithMouse()
    {
        // Raycast to detect the clicked tile
        Camera mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main Camera is not found or missing in the scene!");
            return;
        }

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // Check if the clicked object is a grid tile
            if (hit.collider != null && gridGenerator.TargetGroupList.Contains(hit.collider.gameObject))
            {
                Vector3 position = hit.collider.gameObject.transform.position;
                Node clickedNode = hit.collider.gameObject.GetComponent<Node>();

                if (placingSender)
                {
                    // Place sender (Aprefab) if not already placed
                    if (senderInstance == null)
                    {
                        senderInstance = Lean.Pool.LeanPool.Spawn(Aprefab, position, Quaternion.identity);
                        senderNode = clickedNode; // Store the sender's node
                        Debug.Log("Sender (Aprefab) placed. Now click to place the receiver (Bprefab).");
                        placingSender = false;
                    }
                }
                else
                {
                    // Place receiver (Bprefab) if not already placed
                    if (receiverInstance == null && senderInstance != null && senderInstance.transform.position != position)
                    {
                        receiverInstance = Lean.Pool.LeanPool.Spawn(Bprefab, position, Quaternion.identity);
                        receiverNode = clickedNode; // Store the receiver's node
                        Debug.Log("Receiver (Bprefab) placed. Calculating path and starting package movement...");

                        // Trigger package movement between sender and receiver
                        List<Node> path = Pathfinding.FindRandomPath(senderNode, receiverNode);
                        if (path != null && path.Count > 0)
                        {
                            packageMover.SendPackage(path);
                        }
                        else
                        {
                            Debug.LogError("No valid path found between sender and receiver!");
                        }

                        placingSender = true; // Reset to placing sender if needed
                    }
                }
            }
        }
    }

    // Handle scene reload to reinitialize references
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene reloaded. Reinitializing references...");
        InitializeReferences();
    }
}
