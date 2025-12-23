using System;
using UnityEngine;
using YG;

namespace Project.Scripts.Services
{
    public class PauseService : IPauseService
    {
        private const int PlayTime = 1;
        private const int MinCountPause = 1;
        private const int StopTime = 0;

        private int _countPauses;

        public event Action OnGameStarted;
        public event Action OnGamePaused;

        public void PlayGameAndResetAllPauses(bool isYGGameplayStart = false)
        {
            Debug.Log("count pauses - " + _countPauses + " Before ResetAllPauses TimeScale - " + Time.timeScale);
            
            AudioListener.pause = false;
            _countPauses = 0;
            OnGameStarted?.Invoke();

            if (isYGGameplayStart)
                YG2.GameplayStart();

            Time.timeScale = PlayTime;
            
            Debug.Log("count pauses - " + _countPauses + " After ResetAllPauses TimeScale - " + Time.timeScale);
        }

        private void PlayGame(bool isYGGameplayStart = false)
        {
            Debug.Log("count pauses - " + _countPauses + " Before PlayGame TimeScale - " + Time.timeScale);
            
            switch (_countPauses)
            {
                case MinCountPause:
                    PlayGameAndResetAllPauses(true);
                    break;
                case > MinCountPause:
                    _countPauses--;
                    
                    if (_countPauses == MinCountPause)
                        PlayGameAndResetAllPauses(true);
                    break;
            }
            
            Debug.Log("count pauses - " + _countPauses + " After PlayGame TimeScale - " + Time.timeScale);
        }

        private void StopGame(bool isYGGameplayStop = false)
        {
            Debug.Log("count pauses - " + _countPauses + " Before StopGame TimeScale - " + Time.timeScale);
            
            if (Time.timeScale != StopTime)
            {
                OnGamePaused?.Invoke();

                if (isYGGameplayStop)
                    YG2.GameplayStop();

                Time.timeScale = StopTime;
            }

            _countPauses++;
            
            Debug.Log("count pauses - " + _countPauses + " After StopGame TimeScale - " + Time.timeScale);
        }

        public void OnStopGameWithoutMusic()
        {
            StopGame(true);
        }

        public void OnStopGameWithMusic()
        {
            AudioListener.pause = true;
            StopGame(true);
        }

        public void OnPlayGame()
        {
            PlayGame(true);
        }

        public void OnPlayGameAndResetAllPauses()
        {
            PlayGameAndResetAllPauses(true);
        }
    }
}