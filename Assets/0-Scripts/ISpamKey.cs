using UnityEngine;
using UnityEngine.UI;
using PrimeTween;
using System.Collections;

public class ISpamKey : MonoBehaviour
{
    [SerializeField] private bool isPersonA = true;
    [SerializeField] private Image spriteRenderer;
    [SerializeField] private Sprite[] spriteSequence; // Animation frames
    [SerializeField] private float frameRate = 0.5f; // Time per frame

    private RectTransform rectTransform;
    private bool isShaking;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<Image>();
        }

        // Start the looping animation
        StartCoroutine(PlaySpriteSequenceLoop());
    }

    public void OnKeyPress()
    {
        if (!isShaking)
        {
            isShaking = true;
            Tween.ShakeScale(rectTransform, new Vector3(0.5f, 0.5f, 0f), 0.1f).OnComplete(() =>
            {
                isShaking = false;
            });
        }
    }

    private IEnumerator PlaySpriteSequenceLoop()
    {
        int index = 0;
        while (true) // Infinite loop
        {
            spriteRenderer.sprite = spriteSequence[index];
            index = (index + 1) % spriteSequence.Length; // Loop back to the start
            yield return new WaitForSeconds(frameRate);
        }
    }

    public bool IsPersonA()
    {
        return isPersonA;
    }
}
