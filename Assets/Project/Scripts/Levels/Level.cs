using System;
using Project.Scripts.ECS.System;
using Project.Scripts.Levels.Triggers;
using Project.Scripts.UI.Panel;
using Project.Scripts.UI.View;
using UnityEngine;

namespace Project.Scripts.Levels
{
    public abstract class Level : MonoBehaviour
    {
        private const float MinValue = 0f;
        
        [field: SerializeField] public bool IsLaunchedPlayerCapsule { get; private set; }
        
        [field: SerializeField] public EndLevelTrigger EndLevelTrigger { get; private set; }
        [field: SerializeField] public EntranceTrigger EntranceTrigger { get; private set; }
        
        [SerializeField] private float _delay = 10f;
        
        protected Timer Timer;
        protected AdviserMessagePanel AdviserMessagePanel;
        
        private GameInitSystem _gameInitSystem;
        private float _lastSpawnTime;

        public void GetServices(GameInitSystem gameInitSystem, Timer timer, AdviserMessagePanel adviserMessagePanel)
        {
            _gameInitSystem = gameInitSystem;
            AdviserMessagePanel = adviserMessagePanel;
            Timer = timer;
        }

        protected void CreateWaveOfEnemy()
        {
            if (_lastSpawnTime <= MinValue)
            {
                _gameInitSystem.SpawnEnemy();

                _lastSpawnTime = _delay;
            }

            _lastSpawnTime -= Time.deltaTime;
        }

        protected void SpawnPlayer()
        {
            if (IsLaunchedPlayerCapsule)
            {
                _gameInitSystem.CreateCapsule();
            }
            else
            {
                _gameInitSystem.SpawnPlayer();
            }
        }
    }
}