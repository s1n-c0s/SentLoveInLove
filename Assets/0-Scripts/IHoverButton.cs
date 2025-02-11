using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using PrimeTween;

public class IHoverButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 originalScale;
    private Vector3 hoverScale;
    private RectTransform rectTransform;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        originalScale = rectTransform.localScale;
        hoverScale = originalScale * 1.2f; // Scale up by 20%
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (rectTransform.localScale != hoverScale)
        {
            Tween.Scale(rectTransform, hoverScale, 0.2f);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (rectTransform.localScale != originalScale)
        {
            Tween.Scale(rectTransform, originalScale, 0.2f);
        }
    }
}
