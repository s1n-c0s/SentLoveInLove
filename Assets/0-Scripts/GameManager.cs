using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    private Scene currentScene;
    public static GameManager Instance; // Singleton instance of the GameManager
    public GridGenerator gridGenerator; // Reference to the GridGenerator script
    public Camera mainCamera;           // Reference to the main/top-down camera
    [SerializeField] private PackageMover packageMover;
    public float zoomOutFactor = 1.5f;  // Factor to zoom out the camera (1.5 = 50% zoom-out)

    public enum GameState
    {
        SetAandB,     // State for placing A and B
        MovePackage   // State for moving the package
    }

    public GameState CurrentState { get; private set; } = GameState.SetAandB; // Default state

    private Node startNode; // Node for sender (A)
    private Node endNode;   // Node for receiver (B)
    private List<Node> path; // Path from startNode to endNode

    void Awake()
    {
        // Implement the Singleton pattern to ensure only one GameManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist GameManager across scenes
        }
        else
        {
            Destroy(gameObject);
            return; // Exit to avoid duplicate logic
        }

        // Get the current active scene
        currentScene = SceneManager.GetActiveScene();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetApp();
        }

        // Handle state-based logic
        switch (CurrentState)
        {
            case GameState.SetAandB:
                HandleSetAandB();
                break;
            case GameState.MovePackage:
                HandleMovePackage();
                break;
        }
    }

    void Start()
    {
        InitializeReferences();
        GenerateGridAndAdjustCamera();
    }

    private void InitializeReferences()
    {
        // Find the grid generator and main camera dynamically if not assigned
        if (gridGenerator == null)
            gridGenerator = FindObjectOfType<GridGenerator>();

        if (mainCamera == null)
            mainCamera = Camera.main; // Get the main camera tagged as "MainCamera"

        if (gridGenerator == null)
            Debug.LogError("GridGenerator is not assigned or missing in the scene!");

        if (mainCamera == null)
            Debug.LogError("Main Camera is not assigned or missing in the scene!");
    }

    private void HandleSetAandB()
    {
        if (Input.GetMouseButtonDown(0)) // Detect left mouse button click
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider != null && gridGenerator.TargetGroupList.Contains(hit.collider.gameObject))
                {
                    Node clickedNode = hit.collider.gameObject.GetComponent<Node>();

                    if (startNode == null)
                    {
                        startNode = clickedNode;
                        Debug.Log("Set A (Sender) at: " + clickedNode.name);
                    }
                    else if (endNode == null && clickedNode != startNode)
                    {
                        endNode = clickedNode;
                        Debug.Log("Set B (Receiver) at: " + clickedNode.name);

                        // Transition to MovePackage state
                        ChangeState(GameState.MovePackage);
                    }
                }
            }
        }
    }

    private void HandleMovePackage()
    {
        if (startNode != null && endNode != null)
        {
            Debug.Log("Calculating path and moving package...");

            // Find the path using Pathfinding logic
            path = Pathfinding.FindPath(startNode, endNode);

            if (path != null && path.Count > 0)
            {
                // Move the package along the path
                MovePackage(startNode, endNode, path);
            }
            else
            {
                Debug.LogError("No valid path found between A and B!");
            }

            // Reset states to allow re-selection
            ResetState();
        }
    }

    public void ChangeState(GameState newState)
    {
        CurrentState = newState;
        Debug.Log("Game state changed to: " + newState);
    }

    public void ResetState()
    {
        Debug.Log("Resetting state...");
        startNode = null;
        endNode = null;
        path = null;
        ChangeState(GameState.SetAandB); // Reset to SetAandB state
    }

    public void MovePackage(Node startNode, Node endNode, List<Node> path)
    {
        if (packageMover == null)
        {
            Debug.LogError("PackageMover is not assigned!");
            return;
        }

        packageMover.SendPackage(path);
    }

    private void GenerateGridAndAdjustCamera()
    {
        if (gridGenerator == null || mainCamera == null) return;

        // Generate the grid and adjust the camera at the start
        gridGenerator.GenerateGrid();
        AdjustCameraToFitGrid();
    }

    public void AdjustCameraToFitGrid()
    {
        if (gridGenerator == null || mainCamera == null)
        {
            Debug.LogError("GridGenerator or Main Camera is not assigned!");
            return;
        }

        // Calculate the bounds of all targets in the grid
        Bounds bounds = gridGenerator.CalculateGridBounds();

        // Calculate the camera position and zoom
        Vector3 center = bounds.center;
        float size = Mathf.Max(bounds.size.x, bounds.size.z) / 2f; // Largest dimension
        size *= zoomOutFactor; // Apply zoom-out factor

        if (mainCamera.orthographic)
        {
            // Adjust orthographic size
            mainCamera.orthographicSize = size;
            mainCamera.transform.position = new Vector3(center.x, center.y + 10f, center.z); // Adjust height for orthographic
        }
        else
        {
            // Adjust perspective camera position
            float distance = size / Mathf.Tan(Mathf.Deg2Rad * mainCamera.fieldOfView / 2f); // Perspective camera distance
            mainCamera.transform.position = new Vector3(center.x, distance, center.z); // Top-down view
        }

        mainCamera.transform.LookAt(center); // Ensure the camera looks at the grid center

        Debug.Log("Camera adjusted to fit the grid with zoom-out factor.");
    }

    public void ResetApp()
    {
        // Reload the current active scene
        SceneManager.LoadScene(currentScene.buildIndex);
    }

    // Dynamically update references when a new scene is loaded
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        currentScene = SceneManager.GetActiveScene();
        InitializeReferences(); // Reinitialize references
        GenerateGridAndAdjustCamera(); // Regenerate grid and adjust the camera
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
