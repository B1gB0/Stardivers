using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Project.Scripts.Services;
using Reflex.Attributes;
using TMPro;
using UnityEngine;

namespace Project.Scripts.UI.View
{
    public class Timer : MonoBehaviour, IView
    {
        private const int SecondsInMinute = 60;
        private const int MinValue = 0;
        
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Transform _showPoint;
        [SerializeField] private Transform _hidePoint;

        private int _timeInSeconds;
        private bool _isPaused;
        private bool _isRunning;
        private CancellationTokenSource _cts;
        private float _accumulatedTime;

        private ITweenAnimationService _tweenAnimationService;

        public event Action IsEndAttack;

        [Inject]
        public void Construct(ITweenAnimationService tweenAnimationService)
        {
            _tweenAnimationService = tweenAnimationService;
        }

        private void Awake()
        {
            UpdateDisplay();
        }

        private void OnEnable()
        {
            IsEndAttack += StopTimer;
            OnLaunchTimer();
        }

        private void OnDisable()
        {
            IsEndAttack -= StopTimer;
            StopTimer();
        }

        private void OnDestroy()
        {
            transform.DOKill();
        }
        
        public void GetPoints(Transform showPoint, Transform hidePoint)
        {
            _showPoint = showPoint;
            _hidePoint = hidePoint;
        }

        public void Show()
        {
            gameObject.SetActive(true);
            _tweenAnimationService.AnimateMove(transform, _showPoint, _hidePoint);
        }

        public void Hide()
        {
            _tweenAnimationService.AnimateMove(transform, _showPoint, _hidePoint, true);
        }

        public void ResumeTimer()
        {
            _isPaused = false;
        }
        
        public void PauseTimer()
        {
            _isPaused = true;
        }

        public void SetTime(int seconds)
        {
            _timeInSeconds = seconds;
            UpdateDisplay();
        }
        
        private async void OnLaunchTimer()
        {
            StopTimer();
            
            _isRunning = true;
            _isPaused = false;
            _accumulatedTime = 0f;
            _cts = new CancellationTokenSource();
            
            try
            {
                await RunTimerAsync(_cts.Token);
            }
            finally
            {
                _isRunning = false;
            }
        }
        
        private void StopTimer()
        {
            _isRunning = false;
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
        }

        private void UpdateDisplay()
        {
            int minutes = _timeInSeconds / SecondsInMinute;
            int seconds = _timeInSeconds % SecondsInMinute;
            _text.text = $"{minutes:00} : {seconds:00}";
        }

        private async UniTask RunTimerAsync(CancellationToken ct)
        {
            while (_timeInSeconds > MinValue && _isRunning && !ct.IsCancellationRequested)
            {
                await UniTask.Yield(PlayerLoopTiming.Update, ct);
                
                if (_isPaused) continue;
                
                _accumulatedTime += Time.unscaledDeltaTime;

                if (!(_accumulatedTime >= 1f))
                    continue;
                
                int secondsPassed = Mathf.FloorToInt(_accumulatedTime);
                _accumulatedTime -= secondsPassed;
                    
                _timeInSeconds -= secondsPassed;
                UpdateDisplay();
                    
                if (_timeInSeconds <= MinValue)
                {
                    IsEndAttack?.Invoke();
                    StopTimer();
                    break;
                }
            }
        }
    }
}