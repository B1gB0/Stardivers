using System;
using System.Collections;
using UnityEngine;

namespace Project.Scripts.Levels.Mars.SecondLevel
{
    public class BallisticRocket : MonoBehaviour
    {
        private const float MaxValue = 30f;
        private const float RecoveryRate = 1f;
        
        [SerializeField] private float _speedRising = 5f;
        [SerializeField] private float _speedLaunching = 5f;

        [SerializeField] private Transform _startPoint;
        [SerializeField] private Transform _endPoint;
        [SerializeField] private Transform _launchPad;
        
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
            while (_currentProgress < _maxProgress)
            {
                _currentProgress = Mathf.MoveTowards(_currentProgress, _maxProgress, 
                    RecoveryRate * Time.deltaTime);
                
                _launchPad.position = Vector3.MoveTowards(_launchPad.position, _startPoint.position, 
                    _speedRising * Time.deltaTime);
                
                ProgressChanged?.Invoke(_currentProgress, _maxProgress);

                yield return null;
            }
            
            LaunchCompleted?.Invoke();
        }

        private IEnumerator Launch()
        {
            while (transform.position != _endPoint.position)
            {
                transform.position = Vector3.MoveTowards(transform.position, _endPoint.position,
                    _speedLaunching * Time.deltaTime);
                
                yield return null;
            }
            
            gameObject.SetActive(false);
        }
    }
}
