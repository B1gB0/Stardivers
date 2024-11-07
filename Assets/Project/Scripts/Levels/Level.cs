using Project.Scripts.ECS.System;
using Project.Scripts.UI.Panel;
using UnityEngine;

namespace Project.Scripts.Levels
{
    public abstract class Level : MonoBehaviour
    {
        protected const float MinValue = 0f;
        
        [field: SerializeField] public bool IsLaunchedPlayerCapsule { get; private set; }
        
        [SerializeField] private float Delay = 10f;
        
        protected Timer Timer;
        protected GameInitSystem GameInitSystem;
        protected AdviserMessagePanel AdviserMessagePanel;
        
        private float _lastSpawnTime;

        public void GetServices(GameInitSystem gameInitSystem, Timer timer, AdviserMessagePanel adviserMessagePanel)
        {
            AdviserMessagePanel = adviserMessagePanel;
            GameInitSystem = gameInitSystem;
            Timer = timer;
        }

        protected void CreateWaveOfEnemy()
        {
            if (_lastSpawnTime <= MinValue)
            {
                GameInitSystem.SpawnEnemy();

                _lastSpawnTime = Delay;
            }

            _lastSpawnTime -= Time.deltaTime;
        }
    }
}