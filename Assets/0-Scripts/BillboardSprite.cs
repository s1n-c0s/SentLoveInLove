using UnityEngine;

public class BillboardSprite : MonoBehaviour
{
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void LateUpdate()
    {
        if (mainCamera == null) return;

        // Rotate the sprite to face the camera
        transform.forward = mainCamera.transform.forward;
    }
}
