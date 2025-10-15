using System;
using System.Collections.Generic;
using Project.Scripts.ECS.Data;
using Project.Scripts.ECS.System;
using Project.Scripts.Levels.Mars.FirstLevel;
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
        protected const float MinValue = 0f;
        private const string LeaderboardName = "BestPlayers";

        protected const int FirstWaveEnemy = 0;
        protected const int SecondWaveEnemy = 1;

        protected readonly List<EnemyWave> EnemyWaves = new();
        
        protected DialogueSetter DialogueSetter;

        [field: SerializeField] public bool IsLaunchPlayerCapsule { get; private set; }
        [field: SerializeField] public EndLevelTrigger EndLevelTrigger { get; private set; }
        [field: SerializeField] public EntranceTrigger EntranceToNextLvlTrigger { get; private set; }
        [field: SerializeField] public int QuantityGoldCore { get; private set; }
        [field: SerializeField] public int QuantityHealingCore { get; private set; }

        [SerializeField] protected EnemySpawnFirstWaveTrigger EnemySpawnFirstWaveTrigger;
        [SerializeField] protected WelcomePlanetTextTrigger WelcomePlanetTextTrigger;
        [SerializeField] protected float SpawnWaveOfEnemyDelay = 10f;
        [SerializeField] protected int CountSmallEnemy;
        [SerializeField] protected int CountBigEnemy;
        [SerializeField] protected int CountGunnerEnemy;
        
        [SerializeField] private int _countEnemyWaves;

        protected EnemySpawner EnemySpawner;
        protected Timer Timer;
        protected DialoguePanel DialoguePanel;
        protected PauseService PauseService;
        protected float LastSpawnTime;

        private GameInitSystem _gameInitSystem;
        private ResourcesSpawner _resourcesSpawner;
        private LevelInitData _levelInitData;
        private ILevelTextService _levelTextService;

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
            DialogueSetter = new DialogueSetter(DialoguePanel, _levelTextService);
            
            EndLevelTrigger.Deactivate();
            EntranceToNextLvlTrigger.Deactivate();

            SpawnPlayer();
        }

        public void GetServices(
            GameInitSystem gameInitSystem,
            Timer timer,
            DialoguePanel dialoguePanel,
            PauseService pauseService,
            LevelInitData levelInitData,
            ILevelTextService levelTextService)
        {
            _gameInitSystem = gameInitSystem;
            PauseService = pauseService;
            DialoguePanel = dialoguePanel;
            Timer = timer;
            _levelInitData = levelInitData;
            _levelTextService = levelTextService;

            InitSpawners(gameInitSystem);
        }

        protected virtual void CreateWaveOfEnemy(int numberWaveEnemy)
        {
            if (LastSpawnTime <= MinValue)
            {
                CreateWaveOfSmallEnemies(numberWaveEnemy);
                CreateWaveOfBigEnemies(numberWaveEnemy);
                CreateWaveOfGunnerEnemies(numberWaveEnemy);

                LastSpawnTime = SpawnWaveOfEnemyDelay;
            }

            LastSpawnTime -= Time.fixedDeltaTime;
        }

        protected void CreateWaveOfSmallEnemies(int numberWaveEnemy)
        {
            EnemySpawner.SpawnSmallAlienEnemy(EnemyWaves[numberWaveEnemy].SmallEnemySpawnPositions, CountSmallEnemy);
        }
        
        protected void CreateWaveOfBigEnemies(int numberWaveEnemy)
        {
            EnemySpawner.SpawnBigEnemyAlien(EnemyWaves[numberWaveEnemy].BigEnemySpawnPositions, CountBigEnemy);
        }
        
        protected void CreateWaveOfGunnerEnemies(int numberWaveEnemy)
        {
            EnemySpawner.SpawnGunnerAlienEnemy(EnemyWaves[numberWaveEnemy].GunnerEnemySpawnPositions, CountGunnerEnemy);
        }

        protected void SpawnResources()
        {
            _resourcesSpawner.Spawn(QuantityGoldCore, QuantityHealingCore);
        }

        private void SetCountSpawnPoints()
        {
            foreach (var wave in EnemyWaves)
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

                EnemyWaves.Add(enemyWave);
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
            EnemySpawner = new EnemySpawner(gameInitSystem);

            IsInitiatedSpawners?.Invoke();
        }
    }
}