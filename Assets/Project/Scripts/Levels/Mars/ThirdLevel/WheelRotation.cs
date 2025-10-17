using UnityEngine;

namespace Project.Scripts.Levels.Mars.ThirdLevel
{
    public class WheelRotation : MonoBehaviour
    {
        private const float RotationSpeed = 120f;
        private const float DefaultRotation = 0f; 
        
        private bool _isInverseDirection;
        private bool _isRotate;
        private float _direction;
        
        private void Start()
        {
            _direction = _isInverseDirection ? -1f : 1f;
        }

        private void FixedUpdate()
        {
            if (_isRotate)
            {
                float rotation = _direction * RotationSpeed * Time.fixedDeltaTime;
            
                transform.Rotate(rotation, DefaultRotation, DefaultRotation, Space.Self);
            }
        }

        public void OnRotateWheel(bool isRotate)
        {
            _isRotate = isRotate;
        }
    }
}