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
        FaceMouse();
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

    private void FaceMouse()
    {
        if (_currentPopup == null || Camera.main == null) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, _currentPopup.transform.position);

        if (groundPlane.Raycast(ray, out float distance))
        {
            Vector3 hitPoint = ray.GetPoint(distance);
            Vector3 directionToMouse = hitPoint - _currentPopup.transform.position;
            directionToMouse.y = 0; // optional: rotate only on Y-axis

            if (directionToMouse.sqrMagnitude > 0.01f)
            {
                _currentPopup.transform.rotation = Quaternion.LookRotation(directionToMouse);
            }
        }
    }
}
