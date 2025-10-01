using System;

namespace Project.Scripts.Services
{
    public interface IPauseService
    {
        public void PlayGame(bool isYGGameplayStart = false);
        public void StopGame(bool isYGGameplayStop = false);
        public event Action OnGameStarted;
        public event Action OnGamePaused;
    }
}