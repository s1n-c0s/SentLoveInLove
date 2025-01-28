using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class MenuParallax : MonoBehaviour
{
    [Header("Parallax Settings")]
    [Tooltip("Multiplier for how far the parallax effect should move.")]
    public float offsetMultiplier = 1f;

    [Tooltip("Smooth time for parallax movement.")]
    public float smoothTime = 0.3f;

    private RectTransform rectTransform; // Cached reference to the RectTransform
    private Vector3 startPosition;      // Starting position in local space
    private Vector3 velocity;           // Velocity for SmoothDamp

    private void Awake()
    {
        // Cache the RectTransform component and initialize the starting position
        rectTransform = GetComponent<RectTransform>();
        startPosition = rectTransform.anchoredPosition;
    }

    private void LateUpdate()
    {
        // Calculate normalized viewport offset (-0.5 to 0.5 centered)
        Vector2 viewportOffset = Camera.main.ScreenToViewportPoint(Input.mousePosition) - new Vector3(0.5f, 0.5f);

        // Calculate the target position with parallax effect
        Vector3 targetPosition = startPosition + (Vector3)(viewportOffset * offsetMultiplier);

        // Smoothly move to the target position
        rectTransform.anchoredPosition = Vector3.SmoothDamp(rectTransform.anchoredPosition, targetPosition, ref velocity, smoothTime);
    }
}
