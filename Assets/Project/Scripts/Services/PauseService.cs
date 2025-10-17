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

        public void PlayGame(bool isYGGameplayStart = false)
        {
            switch (_countPauses)
            {
                case MinCountPause :
                    _countPauses = 0;
                    OnGameStarted?.Invoke();
                    
                    if(isYGGameplayStart)
                        YG2.GameplayStart();
                    
                    Time.timeScale = PlayTime;
                    break;
                case > MinCountPause :
                    _countPauses--;
                    break;
            }
        }

        public void StopGame(bool isYGGameplayStop = false)
        {
            if (Time.timeScale != StopTime)
            {
                OnGamePaused?.Invoke();
                
                if(isYGGameplayStop)
                    YG2.GameplayStop();
                
                Time.timeScale = StopTime;
            }
            
            _countPauses++;
        }
    }
}