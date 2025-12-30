using UnityEngine;

namespace Project.Scripts.UI.Camera
{
    public class CameraConstantSize : MonoBehaviour
    {
        private const float VerticalFovFactor = 2f;

        [SerializeField] [Range(0f, 1f)] private float _widthOrHeight;
        [SerializeField] private Vector2 _defaultResolution = new(1920, 1080);

        private UnityEngine.Camera _componentCamera;

        private float _initialSize;
        private float _targetAspect;

        private float _initialFov;
        private float _horizontalFov = 120f;

        private void Start()
        {
            _componentCamera = GetComponent<UnityEngine.Camera>();
            _initialSize = _componentCamera.orthographicSize;

            _targetAspect = _defaultResolution.x / _defaultResolution.y;

            _initialFov = _componentCamera.fieldOfView;
            _horizontalFov = CalcVerticalFov(_initialFov, 1 / _targetAspect);
        }

        private void Update()
        {
            if (_componentCamera.orthographic)
            {
                float constantWidthSize = _initialSize * (_targetAspect / _componentCamera.aspect);
                _componentCamera.orthographicSize = Mathf.Lerp(constantWidthSize, _initialSize, _widthOrHeight);
            }
            else
            {
                float constantWidthFov = CalcVerticalFov(_horizontalFov, _componentCamera.aspect);
                _componentCamera.fieldOfView = Mathf.Lerp(constantWidthFov, _initialFov, _widthOrHeight);
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