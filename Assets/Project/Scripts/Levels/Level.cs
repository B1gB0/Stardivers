using System;
using Project.Scripts.ECS.System;
using Project.Scripts.Levels.Spawners;
using Project.Scripts.Levels.Triggers;
using Project.Scripts.Services;
using Project.Scripts.UI.Panel;
using Project.Scripts.UI.View;
using UnityEngine;
using YG;

namespace Project.Scripts.Levels
{
    public abstract class Level : MonoBehaviour
    {
        private const string LeaderboardName = "BestPlayers";
        protected const float MinValue = 0f;
        
        [field: SerializeField] public bool IsLaunchPlayerCapsule { get; private set; }
        [field: SerializeField] public EndLevelTrigger EndLevelTrigger { get; private set; }
        [field: SerializeField] public EntranceTrigger EntranceToNextLvlTrigger { get; private set; }
        [field: SerializeField] public int QuantityGoldCore { get; private set; }
        [field: SerializeField] public int QuantityHealingCore { get; private set; }

        [SerializeField] protected float SpawnWaveOfEnemyDelay = 10f;
        
        protected EnemySpawner EnemySpawner;

        protected Timer Timer;
        protected AdviserMessagePanel AdviserMessagePanel;
        protected PauseService PauseService;

        protected float LastSpawnTime;
        
        private GameInitSystem _gameInitSystem;
        private ResourcesSpawner _resourcesSpawner;

        public event Action IsInitiatedSpawners;

        private void OnDestroy()
        {
            YG2.SaveProgress();
            YG2.SetLeaderboard(LeaderboardName, YG2.saves.AcumulatedScore);
        }

        public virtual void OnStartLevel()
        {
            SpawnPlayer();
        }

        public void GetServices(
            GameInitSystem gameInitSystem,
            Timer timer,
            AdviserMessagePanel adviserMessagePanel,
            PauseService pauseService)
        {
            _gameInitSystem = gameInitSystem;
            PauseService = pauseService;
            AdviserMessagePanel = adviserMessagePanel;
            Timer = timer;
            
            InitSpawners(gameInitSystem);
        }

        protected virtual void CreateWaveOfEnemy()
        {
            if (LastSpawnTime <= MinValue)
            {
                EnemySpawner.SpawnSmallAlienEnemy();

                LastSpawnTime = SpawnWaveOfEnemyDelay;
            }

            LastSpawnTime -= Time.deltaTime;
        }

        protected void SpawnResources()
        {
            _resourcesSpawner.Spawn(QuantityGoldCore, QuantityHealingCore);
        }
        
        private void SpawnPlayer()
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

        private void InitSpawners(GameInitSystem gameInitSystem)
        {
            _resourcesSpawner = new ResourcesSpawner(gameInitSystem);
            EnemySpawner = new EnemySpawner(gameInitSystem);

            IsInitiatedSpawners?.Invoke();
        }
    }
}