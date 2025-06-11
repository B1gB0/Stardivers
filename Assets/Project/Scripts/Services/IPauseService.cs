using System;

namespace Project.Scripts.Services
{
    public interface IPauseService
    {
        void PlayGame();
        void StopGame();
        event Action OnGameStarted;
        event Action OnGamePaused;
    }
}