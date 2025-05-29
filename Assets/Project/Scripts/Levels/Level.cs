using System;
using Project.Scripts.ECS.System;
using Project.Scripts.Levels.Spawners;
using Project.Scripts.Levels.Triggers;
using Project.Scripts.UI.Panel;
using Project.Scripts.UI.View;
using UnityEngine;

namespace Project.Scripts.Levels
{
    public abstract class Level : MonoBehaviour
    {
        protected const float MinValue = 0f;
        
        [field: SerializeField] public bool IsLaunchPlayerCapsule { get; private set; }
        [field: SerializeField] public EndLevelTrigger EndLevelTrigger { get; private set; }
        [field: SerializeField] public EntranceTrigger EntranceToNextLvlTrigger { get; private set; }
        [field: SerializeField] public int QuantityGoldCore { get; private set; }
        [field: SerializeField] public int QuantityHealingCore { get; private set; }

        [SerializeField] protected float Delay = 10f;
        
        protected EnemySpawner EnemySpawner;

        protected Timer Timer;
        protected AdviserMessagePanel AdviserMessagePanel;

        protected float LastSpawnTime;
        
        private GameInitSystem _gameInitSystem;
        private ResourcesSpawner _resourcesSpawner;

        public event Action IsInitiatedSpawners;

        public void GetServices(GameInitSystem gameInitSystem, Timer timer, AdviserMessagePanel adviserMessagePanel)
        {
            _gameInitSystem = gameInitSystem;
            AdviserMessagePanel = adviserMessagePanel;
            Timer = timer;
            
            InitSpawners(gameInitSystem);
        }

        protected virtual void CreateWaveOfEnemy()
        {
            if (LastSpawnTime <= MinValue)
            {
                EnemySpawner.SpawnSmallAlienEnemy();

                LastSpawnTime = Delay;
            }

            LastSpawnTime -= Time.deltaTime;
        }

        protected void SpawnPlayer()
        {
            if (IsLaunchPlayerCapsule)
            {
                _gameInitSystem.CreateCapsule();
            }
            else
            {
                _gameInitSystem.SpawnPlayer();
            }
        }

        protected void SpawnResources()
        {
            _resourcesSpawner.Spawn(QuantityGoldCore, QuantityHealingCore);
        }

        private void InitSpawners(GameInitSystem gameInitSystem)
        {
            _resourcesSpawner = new ResourcesSpawner(gameInitSystem);
            EnemySpawner = new EnemySpawner(gameInitSystem);

            IsInitiatedSpawners?.Invoke();
        }
    }
}