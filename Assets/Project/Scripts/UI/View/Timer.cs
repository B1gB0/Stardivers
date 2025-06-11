using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Project.Scripts.UI.View
{
    public class Timer : MonoBehaviour, IView
    {
        private const int SecondsInMinute = 60;
        private const int MinValue = 0;
        
        [SerializeField] private TMP_Text _text;

        private int _timeInSeconds;
        private bool _isPaused;
        private bool _isRunning;
        private CancellationTokenSource _cts;
        private float _accumulatedTime;

        public event Action IsEndAttack;

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

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void ResumeTimer()
        {
            _isPaused = false;
        }
        
        public void PauseTimer()
        {
            _isPaused = true;
        }
        
        public async void OnLaunchTimer()
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
            catch (OperationCanceledException)
            {
                // Таймер был остановлен
            }
            finally
            {
                _isRunning = false;
            }
        }
        
        public void SetTime(int seconds)
        {
            _timeInSeconds = seconds;
            UpdateDisplay();
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
                // Ждем до следующего кадра
                await UniTask.Yield(PlayerLoopTiming.Update, ct);
                
                if (_isPaused) continue;
                
                // Накопление времени с учетом реального времени
                _accumulatedTime += Time.unscaledDeltaTime;
                
                // Проверяем, прошла ли целая секунда
                if (_accumulatedTime >= 1f)
                {
                    // Вычисляем сколько целых секунд прошло
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
}