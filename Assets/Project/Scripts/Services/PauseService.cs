using UnityEngine;
using YG;

namespace Project.Scripts.Services
{
    public class PauseService : IPauseService
    {
        private const int PlayTime = 1;
        private const int MinCountPause = 1;
        private const int StopTime = 0;

        private int countPauses;

        public void PlayGame()
        {
            switch (countPauses)
            {
                case MinCountPause :
                    countPauses = 0;
                    Time.timeScale = PlayTime;
                    YandexGame.GameplayStart();
                    break;
                case > MinCountPause :
                    countPauses--;
                    break;
            }
        }

        public void StopGame()
        {
            if (Time.timeScale != StopTime)
            {
                Time.timeScale = StopTime;
                YandexGame.GameplayStop();
            }
            
            countPauses++;
        }
    }
}