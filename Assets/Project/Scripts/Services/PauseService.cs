using System;
using UnityEditor.PackageManager;
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
            AudioListener.pause = false;
            _countPauses = 0;
            OnGameStarted?.Invoke();
            
            if (isYGGameplayStart)
                YG2.GameplayStart();
            
            Time.timeScale = PlayTime;
        }

        private void PlayGame(bool isYGGameplayStart = false)
        {
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
        }

        private void StopGame(bool isYGGameplayStop = false)
        {
            if (Time.timeScale != StopTime)
            {
                OnGamePaused?.Invoke();
            
                if (isYGGameplayStop)
                    YG2.GameplayStop();
            
                Time.timeScale = StopTime;
            }

            _countPauses++;
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