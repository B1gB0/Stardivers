using System;
using UnityEngine.EventSystems;

namespace Project.Scripts.Services
{
    public interface IPauseService
    {
        public event Action OnGameStarted;
        public event Action OnGamePaused;
        
        public void OnStopGameWithoutMusic();
        public void OnStopGameWithMusic();
        public void OnPlayGame();
        public void GetEventSystem(EventSystem eventSystem);
        public void DisableEventSystem();
        public void EnableEventSystem();
    }
}