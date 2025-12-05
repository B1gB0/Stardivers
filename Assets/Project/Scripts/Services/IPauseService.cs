using System;

namespace Project.Scripts.Services
{
    public interface IPauseService
    {
        public void PlayGameAndResetAllPauses(bool isYGGameplayStart = false);
        public void PlayGame(bool isYGGameplayStart = false);
        public void StopGame(bool isYGGameplayStop = false);
        public void OnFocusWindowGame(bool isFocusGame);
        public void OnShowAdvertisement();
        public void OnCloseAdvertisement();
        public event Action OnGameStarted;
        public event Action OnGamePaused;
    }
}