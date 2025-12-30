using UnityEngine;

namespace Project.Scripts.UI.View
{
    public class Arrow : MonoBehaviour, IView
    {
        private const float _offsetY = 2.5f;
        private const float _offsetZ = 2f;

        private Transform _targetSpawnPosition;
        private Transform _currentTarget;

        public void Construct(Transform target)
        {
            _targetSpawnPosition = target;
        }

        private void FixedUpdate()
        {
            transform.position = new Vector3(
                _targetSpawnPosition.position.x,
                _offsetY,
                _targetSpawnPosition.position.z + _offsetZ);

            transform.LookAt(_currentTarget);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void OnLookAtTarget(Transform target)
        {
            if (target != null && target == _currentTarget)
                return;

            _currentTarget = target;
        }
    }
}