using UnityEngine;
using PrimeTween;
using System.Collections;

public class IHowToPlay : MonoBehaviour
{
    [SerializeField] private GameObject[] panels; // List of panels to show
    [SerializeField] private GameObject progressBar;
    [SerializeField] private float showDuration = 1f; // Duration for panel show/hide
    private int currentIndex = 0;

    public void HideAllPanels()
    {
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(false);
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

    public bool isReadyToPlayComplete = false;

    public IEnumerator ShowReadyToPlay()
    {
        // Scale in the third panel (index 2) with showDuration
        if (panels.Length > 2)
        {
            yield return ScalePanel(panels[2], new Vector3(1f, 0.5f, 1f), showDuration);
            // Scale out the third panel (index 2)
            yield return ScaleOutPanel(panels[2], showDuration);
        }

        // Scale in the fourth panel (index 3)
        if (panels.Length > 3)
        {
            yield return ScalePanel(panels[3], new Vector3(0.5f, 1f, 1f), showDuration);
            // Scale out the fourth panel (index 3)
            yield return ScaleOutPanel(panels[3], showDuration);
        }

        isReadyToPlayComplete = true;
        progressBar.SetActive(true);
    }

    private IEnumerator ScalePanel(GameObject panel, Vector3 target, float duration)
    {
        CanvasGroup canvasGroup = panel.GetComponent<CanvasGroup>() ?? panel.AddComponent<CanvasGroup>();

        if (!panel.activeInHierarchy)
        {
            panel.SetActive(true); // Ensure the panel is active before scaling
        }

        bool completed = false;
        Tween.Scale(panel.transform, target, duration).OnComplete(() => completed = true);
        while (!completed) yield return null;
    }

    private IEnumerator ScaleOutPanel(GameObject panel, float duration)
    {
        bool completed = false;
        Tween.Scale(panel.transform, Vector3.zero, duration).OnComplete(() =>
        {
            panel.SetActive(false);
            completed = true;
        });
        while (!completed) yield return null;
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
