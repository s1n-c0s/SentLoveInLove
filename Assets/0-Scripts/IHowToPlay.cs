using UnityEngine;
using PrimeTween;
using System.Collections;

public class IHowToPlay : MonoBehaviour
{
    [SerializeField] private GameObject[] panels; // List of panels to show
    [SerializeField] private GameObject progressBar;
    [SerializeField] private float showDuration = 1f; // Duration for panel show/hide

    public bool isReadyToPlayComplete = false;

    public void HideAllPanels()
    {
        foreach (var panel in panels)
        {
            panel.SetActive(false);
        }
    }

    public void ShowHowToPlayPanels()
    {
        progressBar.SetActive(false);
        HideAllPanels();

        if (panels.Length > 1)
        {
            FadeInPanel(panels[0], showDuration, () =>
            {
                FadeOutPanel(panels[0], showDuration, () =>
                {
                    FadeInPanel(panels[1], showDuration);
                });
            });
        }
        else if (panels.Length == 1)
        {
            FadeInPanel(panels[0], showDuration);
        }
    }

    public void ShowReadyToPlay()
    {
        progressBar.SetActive(true);
        // HideAllPanels();
        // panels[2].SetActive(true);
    }

    private void FadeInPanel(GameObject panel, float duration, System.Action onComplete = null)
    {
        CanvasGroup canvasGroup = panel.GetComponent<CanvasGroup>() ?? panel.AddComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        panel.SetActive(true);
        Tween.Alpha(canvasGroup, 1f, duration).OnComplete(() => onComplete?.Invoke());
    }

    private void FadeOutPanel(GameObject panel, float duration, System.Action onComplete)
    {
        CanvasGroup canvasGroup = panel.GetComponent<CanvasGroup>() ?? panel.AddComponent<CanvasGroup>();
        canvasGroup.alpha = 1;
        Tween.Alpha(canvasGroup, 0f, duration).OnComplete(() =>
        {
            panel.SetActive(false);
            onComplete?.Invoke();
        });
    }
}
