using UnityEngine;

public class IMenuParallax : MonoBehaviour
{
    public float offsetMultiplier = 1f; // How far the parallax effect should move
    public float smoothTime = 0.3f; // How smooth the movement is

    private Vector3 startPosition;
    private Vector3 velocity;

    private RectTransform rectTransform; // Reference to the RectTransform

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        if (rectTransform == null)
        {
            Debug.LogError("MenuParallax script requires a RectTransform. Please attach it to a UI element.");
            return;
        }

        // Save the starting position in local space
        startPosition = rectTransform.anchoredPosition;
    }

    private void Update()
    {
        if (rectTransform == null) return;

        // Get the mouse position normalized to viewport space (0 to 1)
        Vector2 mousePosition = Input.mousePosition;
        Vector2 viewportOffset = Camera.main.ScreenToViewportPoint(mousePosition) - new Vector3(0.5f, 0.5f); // Center the offset

        // Calculate the target position
        Vector3 targetPosition = startPosition + (Vector3)(viewportOffset * offsetMultiplier);

        // Smoothly move the RectTransform to the target position
        rectTransform.anchoredPosition = Vector3.SmoothDamp(rectTransform.anchoredPosition, targetPosition, ref velocity, smoothTime);
    }
}
