using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
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
        protected const int FirstWaveEnemy = 0;
        protected const int SecondWaveEnemy = 1;
        
        private const string LeaderboardName = "BestPlayers";
        
        private readonly List<EnemyWave> EnemyWaves = new();

        [field: SerializeField] public bool IsLaunchPlayerCapsule { get; private set; }
        [field: SerializeField] public EndLevelTrigger EndLevelTrigger { get; private set; }
        [field: SerializeField] public EntranceTrigger EntranceToNextLvlTrigger { get; private set; }
        [field: SerializeField] public Transform ArrowPoint { get; private set; }
        [field: SerializeField] public int QuantityGoldCore { get; private set; }
        [field: SerializeField] public int QuantityHealingCore { get; private set; }
        [field: SerializeField] public int QuantityIceCrystals { get; private set; }
        
        [SerializeField] protected WelcomePlanetTextTrigger WelcomePlanetTextTrigger;
        [SerializeField] protected float SpawnWaveOfEnemyDelay = 10f;
        [SerializeField] protected int CountSmallEnemy;
        [SerializeField] protected int CountBigEnemy;
        [SerializeField] protected int CountGunnerEnemy;
        
        [SerializeField] private int _countEnemyWaves;

        protected ResourcesSpawner ResourcesSpawner;
        protected EnemySpawner EnemySpawner;
        protected DialogueSetter DialogueSetter;
        protected DialoguePanel DialoguePanel;
        protected PauseService PauseService;
        protected ViewFactory ViewFactory;
        protected ICurrencyService CurrencyService;
        protected float LastSpawnTime;
        protected Arrow Arrow;

        private GameInitSystem _gameInitSystem;
        private LevelInitData _levelInitData;
        private ILevelTextService _levelTextService;

        public event Action IsInitiatedSpawners;
        public event Action OnAlienCocoonViewShow;

        private void OnDestroy()
        {
            YG2.SaveProgress();
            YG2.SetLeaderboard(LeaderboardName, YG2.saves.AcumulatedScore);
        }

        public virtual async UniTask OnStartLevel()
        {
            DialogueSetter = new DialogueSetter(DialoguePanel, _levelTextService);
            Arrow = await ViewFactory.CreateArrow(ArrowPoint);
            
            EndLevelTrigger.Deactivate();
            EntranceToNextLvlTrigger.Deactivate();

            SpawnPlayer();
        }

        public void GetServices(
            GameInitSystem gameInitSystem,
            DialoguePanel dialoguePanel,
            PauseService pauseService,
            LevelInitData levelInitData,
            ILevelTextService levelTextService,
            ViewFactory viewFactory,
            ICurrencyService currencyService)
        {
            _gameInitSystem = gameInitSystem;
            PauseService = pauseService;
            DialoguePanel = dialoguePanel;
            _levelInitData = levelInitData;
            _levelTextService = levelTextService;
            ViewFactory = viewFactory;
            CurrencyService = currencyService;

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
        
        protected void CreateAllAlienEnemyTurrets()
        {
            EnemySpawner.SpawnAlienEnemyTurret(_levelInitData.EnemyTurretsSpawnPoints,
                _levelInitData.PlayerSpawnPosition);
        }

        protected void SpawnResources()
        {
            ResourcesSpawner.Spawn(QuantityGoldCore, QuantityHealingCore);
        }

        protected void SpawnAlienCocoons()
        {
            ResourcesSpawner.SpawnAlienCocoons();
            OnAlienCocoonViewShow?.Invoke();
        }

        protected void SpawnIceCrystals()
        {
            ResourcesSpawner.SpawnIceCrystal(QuantityIceCrystals);
        }
        
        protected void ArrowLookAtOutpost()
        {
            Arrow.OnLookAtTarget(EndLevelTrigger.transform);
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

            ResourcesSpawner = new ResourcesSpawner(gameInitSystem, _levelInitData);
            EnemySpawner = new EnemySpawner(gameInitSystem);

            IsInitiatedSpawners?.Invoke();
        }
    }
}