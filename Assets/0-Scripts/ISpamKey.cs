using UnityEngine;
using UnityEngine.UI;
using PrimeTween;

[RequireComponent(typeof(Image), typeof(RectTransform))]
public class ISpamKey : MonoBehaviour
{
    [SerializeField] private bool isPersonA = true;
    [SerializeField] private Image spriteRenderer;
    [SerializeField] private Sprite[] spriteSequence;
    [SerializeField] private float frameRate = 0.5f; // Time per frame

    private RectTransform rectTransform;
    private bool isShaking;
    private float nextFrameTime;
    private int frameIndex;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        spriteRenderer ??= GetComponent<Image>(); // Assign if not set in Inspector
    }

    private void OnEnable()
    {
        frameIndex = 0;
        nextFrameTime = Time.time + frameRate;
        spriteRenderer.sprite = spriteSequence[frameIndex]; // Ensure correct frame on reactivation
    }

    private void Update()
    {
        if (Time.time >= nextFrameTime)
        {
            nextFrameTime += frameRate;
            frameIndex = (frameIndex + 1) % spriteSequence.Length;
            spriteRenderer.sprite = spriteSequence[frameIndex];
        }
    }

    public void OnKeyPress()
    {
        if (isShaking) return;

        isShaking = true;
        Tween.ShakeScale(rectTransform, new Vector3(0.5f, 0.5f, 0f), 0.1f).OnComplete(() =>
        {
            isShaking = false;
        });
    }

    public bool IsPersonA() => isPersonA;
}
