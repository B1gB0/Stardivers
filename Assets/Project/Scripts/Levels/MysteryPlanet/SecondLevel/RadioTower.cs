using System;
using System.Collections;
using Project.Scripts.ECS.EntityActors;
using UnityEngine;

namespace Project.Scripts.Levels.MysteryPlanet.SecondLevel
{
    public class RadioTower : MonoBehaviour
    {
        private const float MaxValue = 60f;
        private const float MinValue = 0f;
        private const float RecoveryRate = 1f;

        [SerializeField] private float _speedRising = 2.5f;

        [SerializeField] private Transform _endPoint;
        [SerializeField] private Transform _handleOfDish;

        private Coroutine _coroutine;
        private float _currentProgress;
        private float _maxProgress;

        public event Action<float, float> ProgressChanged;

        public event Action InstallationDishCompleted;

        private void Start()
        {
            _maxProgress = MaxValue;

            ProgressChanged?.Invoke(_currentProgress, _maxProgress);
        }

        public void OnChangeProgress(PlayerActor player)
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
            }
            else
            {
                _coroutine = StartCoroutine(ChangeProgress(player));
            }
        }

        public void OnStopChangeProgress()
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
            }
        }

        private IEnumerator ChangeProgress(PlayerActor player)
        {
            while (_currentProgress < _maxProgress)
            {
                if (player.Health.TargetHealth <= MinValue)
                {
                    OnStopChangeProgress();
                }

                _currentProgress = Mathf.MoveTowards(
                    _currentProgress,
                    _maxProgress,
                    RecoveryRate * Time.deltaTime);

                Vector3 currentHandlePosition = _handleOfDish.position;

                float positionY = Mathf.MoveTowards(
                    currentHandlePosition.y,
                    _endPoint.position.y,
                    _speedRising * Time.deltaTime);

                _handleOfDish.position = new Vector3(currentHandlePosition.x, positionY, currentHandlePosition.z);

                ProgressChanged?.Invoke(_currentProgress, _maxProgress);

                yield return null;
            }

            InstallationDishCompleted?.Invoke();
        }
    }
}