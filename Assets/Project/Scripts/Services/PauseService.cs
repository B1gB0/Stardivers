using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Project.Scripts.Services
{
    public class PauseService : IPauseService
    {
        private const float StopTime = 0f;
        private const float PlayTime = 1f;

        private EventSystem _eventSystem;
        
        public event Action OnGameStarted;
        public event Action OnGamePaused;

        public void OnStopGameWithoutMusic()
        {
            AudioListener.pause = false;
            Time.timeScale = StopTime;
            
            OnGamePaused?.Invoke();
        }

        public void OnStopGameWithMusic()
        {
            AudioListener.pause = true;
            Time.timeScale = StopTime;
            
            OnGamePaused?.Invoke();
        }

        public void OnPlayGame()
        {
            AudioListener.pause = false;
            Time.timeScale = PlayTime;

            OnGameStarted?.Invoke();
        }

        public void GetEventSystem(EventSystem eventSystem)
        {
            _eventSystem = eventSystem;
        }
        
        public void DisableEventSystem()
        {
            _eventSystem.enabled = false;
        }
        
        public void EnableEventSystem()
        {
            _eventSystem.enabled = true;
        }
    }
}