using UnityEngine;

namespace Project.Scripts.UI
{
    public class PauseService
    {
        private const int PlayTime = 1;
        private const int StopTime = 0;

        public void PlayGame()
        {
            if(Time.timeScale != PlayTime)
                Time.timeScale = PlayTime;
        }

        public void StopGame()
        {
            if(Time.timeScale != StopTime)
                Time.timeScale = StopTime;
        }
    }
}