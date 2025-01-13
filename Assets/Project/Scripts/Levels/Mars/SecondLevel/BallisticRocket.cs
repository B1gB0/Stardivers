using System;
using System.Collections;
using UnityEngine;

namespace Project.Scripts.Levels.Mars.SecondLevel
{
    public class BallisticRocket : MonoBehaviour
    {
        private const float MaxValue = 30f;
        private const float RecoveryRate = 1f;

        [SerializeField] private float _speed = 5f;
        [SerializeField] private Transform _endPoint;
        
        private Coroutine _coroutine;
        private float _currentProgress;
        private float _maxProgress;

        public event Action<float, float> ProgressChanged;

        public event Action LaunchCompleted;

        private void Start()
        {
            _maxProgress = MaxValue;
            
            ProgressChanged?.Invoke(_currentProgress, _maxProgress);
        }

        private void OnEnable()
        {
            LaunchCompleted += OnLaunch;
        }

        private void OnDisable()
        {
            LaunchCompleted -= OnLaunch;
        }

        public void OnChangeProgress()
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
            }
            else
            {
                _coroutine = StartCoroutine(ChangeProgress());
            }
        }
        
        private void OnLaunch()
        {
            StartCoroutine(Launch());
        }

        private IEnumerator ChangeProgress()
        {
            while (_currentProgress != _maxProgress)
            {
                _currentProgress = Mathf.MoveTowards(_currentProgress, _maxProgress, RecoveryRate * Time.deltaTime);
            
                ProgressChanged?.Invoke(_currentProgress, _maxProgress);

                if (_currentProgress >= _maxProgress)
                    LaunchCompleted?.Invoke();

                yield return null;
            }
        }

        private IEnumerator Launch()
        {
            while (transform.position != _endPoint.position)
            {
                transform.position = Vector3.MoveTowards(transform.position, _endPoint.position, 
                    _speed * Time.deltaTime);
                
                if(transform.position == _endPoint.position)
                    gameObject.SetActive(false);

                yield return null;
            }
        }
    }
}
