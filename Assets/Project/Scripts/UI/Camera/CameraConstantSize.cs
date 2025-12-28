using UnityEngine;

namespace Project.Scripts.UI.Camera
{
    public class CameraConstantSize : MonoBehaviour
    {
        private const float VerticalFovFactor = 2f;
        
        [SerializeField] [Range(0f, 1f)] private float _widthOrHeight;
        [SerializeField] private Vector2 _defaultResolution = new (1920, 1080);

        private UnityEngine.Camera _componentCamera;
    
        private float initialSize;
        private float targetAspect;

        private float initialFov;
        private float horizontalFov = 120f;

        private void Start()
        {
            _componentCamera = GetComponent<UnityEngine.Camera>();
            initialSize = _componentCamera.orthographicSize;

            targetAspect = _defaultResolution.x / _defaultResolution.y;

            initialFov = _componentCamera.fieldOfView;
            horizontalFov = CalcVerticalFov(initialFov, 1 / targetAspect);
        }

        private void Update()
        {
            if (_componentCamera.orthographic)
            {
                float constantWidthSize = initialSize * (targetAspect / _componentCamera.aspect);
                _componentCamera.orthographicSize = Mathf.Lerp(constantWidthSize, initialSize, _widthOrHeight);
            }
            else
            {
                float constantWidthFov = CalcVerticalFov(horizontalFov, _componentCamera.aspect);
                _componentCamera.fieldOfView = Mathf.Lerp(constantWidthFov, initialFov, _widthOrHeight);
            }
        }

        private float CalcVerticalFov(float hFovInDeg, float aspectRatio)
        {
            float hFovInRads = hFovInDeg * Mathf.Deg2Rad;

            float vFovInRads = VerticalFovFactor *
                               Mathf.Atan(Mathf.Tan(hFovInRads / VerticalFovFactor) / aspectRatio);

            return vFovInRads * Mathf.Rad2Deg;
        }
    }
}
