using UnityEngine;

public class Popup3DCreator : MonoBehaviour
{
    [SerializeField] private GridGenerator _gridGenerator;
    [SerializeField] private GameObject _popupPrefab;
    [SerializeField] private float _popupHeightOffset = 1f;
    [SerializeField] private float _popupScaleDuration = 0.3f;
    [SerializeField] private Vector3 _targetScale = Vector3.one;

    private GameObject _currentPopup;

    void Start()
    {
        _gridGenerator = FindObjectOfType<GridGenerator>();
        ShowPopup();
    }

    void Update()
    {
        FaceCamera();
    }

    public void ShowPopup()
    {
        if (_gridGenerator == null || _popupPrefab == null)
        {
            Debug.LogError("Missing references on Popup3DCreator.");
            return;
        }

        Bounds gridBounds = _gridGenerator.CalculateGridBounds();
        Vector3 centerPosition = gridBounds.center + new Vector3(0, _popupHeightOffset + 5, 0);

        if (_currentPopup != null)
        {
            Destroy(_currentPopup);
        }

        _currentPopup = Instantiate(_popupPrefab, centerPosition, Quaternion.identity);
        _currentPopup.transform.localScale = Vector3.zero;

        StartCoroutine(AnimateScale(_currentPopup.transform, _targetScale, _popupScaleDuration));
    }

    private System.Collections.IEnumerator AnimateScale(Transform target, Vector3 targetScale, float duration)
    {
        Vector3 initialScale = target.localScale;
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            float t = timeElapsed / duration;
            target.localScale = Vector3.Lerp(initialScale, targetScale, t);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        target.localScale = targetScale;
    }

    private void FaceCamera()
    {
        if (_currentPopup == null || Camera.main == null) return;

        Vector3 directionToCamera = Camera.main.transform.position - _currentPopup.transform.position;
        directionToCamera.y = 0; // optional: keep upright and only rotate horizontally
        if (directionToCamera.sqrMagnitude > 0.01f)
        {
            _currentPopup.transform.rotation = Quaternion.LookRotation(directionToCamera);
        }
    }
}
