using System;

namespace Project.Scripts.Services
{
    public interface IPauseService
    {
        public event Action OnGameStarted;
        public event Action OnGamePaused;
        public void PlayGameAndResetAllPauses(bool isYGGameplayStart = false);
        public void OnStopGameWithoutMusic();
        public void OnStopGameWithMusic();
        public void OnPlayGame();
        public void OnPlayGameAndResetAllPauses();
    }
}