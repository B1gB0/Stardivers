using UnityEngine;

namespace Project.Scripts.UI
{
    public class PauseService
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
                    break;
                case > MinCountPause :
                    countPauses--;
                    break;
            }
        }

        public void StopGame()
        {
            if (Time.timeScale != StopTime) Time.timeScale = StopTime;
            countPauses++;
        }
    }
}