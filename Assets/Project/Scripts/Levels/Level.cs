using System;
using System.Collections.Generic;
using Project.Scripts.ECS.Data;
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
        private const float MinValue = 0f;
        private const string LeaderboardName = "BestPlayers";

        private const int FirstWaveEnemy = 0;
        private const int SecondWaveEnemy = 1;

        private readonly List<EnemyWave> _enemyWaves = new ();
        
        [field: SerializeField] public bool IsLaunchPlayerCapsule { get; private set; }
        [field: SerializeField] public EndLevelTrigger EndLevelTrigger { get; private set; }
        [field: SerializeField] public EntranceTrigger EntranceToNextLvlTrigger { get; private set; }
        [field: SerializeField] public int QuantityGoldCore { get; private set; }
        [field: SerializeField] public int QuantityHealingCore { get; private set; }

        [SerializeField] protected float SpawnWaveOfEnemyDelay = 10f;
        [SerializeField] private int _countEnemyWaves;

        protected EnemySpawner EnemySpawner;
        protected Timer Timer;
        protected AdviserMessagePanel AdviserMessagePanel;
        protected PauseService PauseService;

        private float _lastSpawnTime;
        private GameInitSystem _gameInitSystem;
        private ResourcesSpawner _resourcesSpawner;
        private LevelInitData _levelInitData;

        public int SmallEnemyCountPoints { get; private set; }
        public int BigEnemyCountPoints { get; private set; }
        public int GunnerEnemyCountPoints { get; private set; }

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
            PauseService pauseService,
            LevelInitData levelInitData)
        {
            _gameInitSystem = gameInitSystem;
            PauseService = pauseService;
            AdviserMessagePanel = adviserMessagePanel;
            Timer = timer;
            _levelInitData = levelInitData;

            InitSpawners(gameInitSystem);
        }

        protected virtual void CreateWaveOfEnemy()
        {
            if (_lastSpawnTime <= MinValue)
            {
                EnemySpawner.SpawnSmallAlienEnemy(_enemyWaves[FirstWaveEnemy].SmallEnemySpawnPositions);
                EnemySpawner.SpawnBigEnemyAlien(_enemyWaves[FirstWaveEnemy].BigEnemySpawnPositions);
                EnemySpawner.SpawnGunnerAlienEnemy(_enemyWaves[FirstWaveEnemy].GunnerEnemySpawnPositions);

                _lastSpawnTime = SpawnWaveOfEnemyDelay;
            }

            _lastSpawnTime -= Time.fixedDeltaTime;
        }

        protected void SpawnResources()
        {
            _resourcesSpawner.Spawn(QuantityGoldCore, QuantityHealingCore);
        }

        private void SetCountSpawnPoints()
        {
            foreach (var wave in _enemyWaves)
            {
                SmallEnemyCountPoints += wave.SmallEnemySpawnPositions.Count;
                BigEnemyCountPoints += wave.BigEnemySpawnPositions.Count;
                GunnerEnemyCountPoints += wave.GunnerEnemySpawnPositions.Count;
            }
        }

        private void InitEnemyWaves()
        {
            for (int i = 0; i < _countEnemyWaves; i++)
            {
                EnemyWave enemyWave = new EnemyWave();

                switch (i)
                {
                    case FirstWaveEnemy:
                        enemyWave.GetEnemyPositions(
                            _levelInitData.FirstWaveSmallEnemyAlienSpawnPositions,
                            _levelInitData.FirstWaveBigEnemyAlienSpawnPositions,
                            _levelInitData.FirstWaveGunnerEnemyAlienSpawnPositions
                        );
                        break;
                    case SecondWaveEnemy:
                        enemyWave.GetEnemyPositions(
                            _levelInitData.SecondWaveSmallEnemyAlienSpawnPositions,
                            _levelInitData.SecondWaveBigEnemyAlienSpawnPositions,
                            _levelInitData.SecondWaveGunnerEnemyAlienSpawnPositions
                        );
                        break;
                    default:
                        throw new Exception("There is not enough data for new waves");
                }
                
                _enemyWaves.Add(enemyWave);
            }
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
            InitEnemyWaves();
            SetCountSpawnPoints();

            _resourcesSpawner = new ResourcesSpawner(gameInitSystem, _levelInitData);
            EnemySpawner = new EnemySpawner(gameInitSystem, _levelInitData);

            IsInitiatedSpawners?.Invoke();
        }
    }
}