using UnityEngine;

public class LogoRotate : MonoBehaviour
{
    public RectTransform imageTransform;
    public float rotationSpeed = 15f;

    private Quaternion targetRotation;

    private void OnEnable()
    {
        targetRotation = imageTransform.localRotation;
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        targetRotation = Quaternion.Euler(0, 0, targetRotation.eulerAngles.z + rotationSpeed * Time.deltaTime);
        imageTransform.localRotation = targetRotation;
    }
}


