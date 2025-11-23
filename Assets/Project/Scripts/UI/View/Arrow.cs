using UnityEngine;

namespace Project.Scripts.UI.View
{
    public class Arrow : MonoBehaviour, IView
    {
        private Transform _targetSpawnPosition;
        private Transform _currentTarget;

        public void Construct(Transform target)
        {
            _targetSpawnPosition = target;
        }
        
        private void FixedUpdate()
        {
            transform.position = new Vector3(_targetSpawnPosition.position.x, _targetSpawnPosition.position.y,
                _targetSpawnPosition.position.z);
            
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
            _currentTarget = target;
        }
    }
}