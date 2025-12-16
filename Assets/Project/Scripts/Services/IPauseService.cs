using System;

namespace Project.Scripts.Services
{
    public interface IPauseService
    {
        public event Action OnGameStarted;
        public event Action OnGamePaused;
        public void PlayGameAndResetAllPauses(bool isYGGameplayStart = false);
        public void PlayGame(bool isYGGameplayStart = false);
        public void StopGame(bool isYGGameplayStop = false);
        public void OnStopGame();
        public void OnPlayGame();
        public void OnPlayGameAndResetAllPauses();
    }
}