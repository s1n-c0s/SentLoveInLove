using UnityEngine;
using UnityEngine.UI;
using PrimeTween;

public class IProgressShake : MonoBehaviour
{
    private RectTransform rectTransform;
    [SerializeField] private Slider slider;
    private float previousValue;
    private bool isShaking;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        //slider = GetComponent<Slider>();

        if (slider != null)
        {
            previousValue = slider.value;
        }
    }

    private void LateUpdate()
    {
        // Check if the slider value has changed significantly
        if (Mathf.Abs(slider.value - previousValue) > 0.01f && !isShaking)
        {
            isShaking = true;
            // Restart the shake effect on every significant value change
            Tween.ShakeScale(rectTransform, new Vector3(0.4f, 0.4f, 0f), 0.4f).OnComplete(() =>
            {
                isShaking = false; // Allow shaking again after it finishes
            });

            previousValue = slider.value; // Update previous value to the current one
        }
    }
}
