using System.Collections;
using UnityEngine;

public class DifferentScreensViewportFitter : MonoBehaviour
{
    public TMPro.TextMeshProUGUI _testText;

    [Header("Dependencies")]
    [SerializeField] private Camera _camera;

    [Header("Edit mode")]
    [SerializeField] private bool _executeAlways;

    private void Start()
    {
        AdjustAnchors();
    }

#if UNITY_EDITOR
    [ExecuteAlways]
    private void Update()
    {
        _testText.text = Time.time.ToString();
        if (_executeAlways)
        {
            AdjustAnchors();
        }
    }
#endif

    private void AdjustAnchors()
    {
        transform.localPosition = new Vector3(_camera.transform.position.x, _camera.transform.position.y, 0f);
        Vector2 screenSize = GetCurrentScreenSize();
        float currentScreenRatio = screenSize.x / screenSize.y;
        float desiredScreenRatio = transform.localScale.x / transform.localScale.y;

        if (currentScreenRatio > desiredScreenRatio)
        {
            float height = screenSize.y;
            transform.localScale = new Vector3(desiredScreenRatio * height, height);
        }
        else
        {
            float width = screenSize.x;
            transform.localScale = new Vector3(width, width / desiredScreenRatio);
        }
    }

    private Vector2 GetCurrentScreenSize()
    {
        Vector2 lowerLeftScreenCorner = _camera.ViewportToWorldPoint(Vector3.zero);
        Vector2 upperRightScreenCorner = _camera.ViewportToWorldPoint(new Vector3(1f, 1f));
        Vector2 screenSize = upperRightScreenCorner - lowerLeftScreenCorner;
        return screenSize;
    }

}
