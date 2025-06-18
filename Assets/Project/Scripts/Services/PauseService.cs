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

        public void PlayGame()
        {
            switch (_countPauses)
            {
                case MinCountPause :
                    _countPauses = 0;
                    OnGameStarted?.Invoke();
                    YG2.GameplayStart();
                    Time.timeScale = PlayTime;
                    break;
                case > MinCountPause :
                    _countPauses--;
                    break;
            }
        }

        public void StopGame()
        {
            if (Time.timeScale != StopTime)
            {
                OnGamePaused?.Invoke();
                YG2.GameplayStop();
                Time.timeScale = StopTime;
            }
            
            _countPauses++;
        }
    }
}