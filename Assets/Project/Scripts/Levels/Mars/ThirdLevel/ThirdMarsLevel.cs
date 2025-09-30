using Project.Scripts.Levels.Triggers;
using UnityEngine;

namespace Project.Scripts.Levels.Mars.ThirdLevel
{
    public class ThirdMarsLevel : Level
    {
        [SerializeField] private EnemySpawnFirstWaveTrigger _enemySpawnFirstWaveTrigger;
        [SerializeField] private EnemySpawnSecondWaveTrigger _enemySpawnSecondWaveTrigger;
        [SerializeField] private EntranceTrigger _entranceLastLvlTrigger;
        [SerializeField] private TruckPlayerTrigger _truckPlayerTrigger;
        [SerializeField] private TruckFinalPointTrigger _truckFinalPointTrigger;

        private void OnEnable()
        {
            IsInitiatedSpawners += SpawnResources;
        }

        private void OnDisable()
        {
            IsInitiatedSpawners -= SpawnResources;
        }

        public override void OnStartLevel()
        {
            base.OnStartLevel();
            
            _enemySpawnFirstWaveTrigger.EnemySpawned += _entranceLastLvlTrigger.Deactivate;
            _enemySpawnSecondWaveTrigger.EnemySpawned += OnCreateBigEnemiesWave;
            
            _truckFinalPointTrigger.IsFinalPointReached += EntranceToNextLvlTrigger.Activate;
            _truckFinalPointTrigger.IsFinalPointReached += _entranceLastLvlTrigger.Activate;
            _truckFinalPointTrigger.IsFinalPointReached += EndLevelTrigger.Activate;
            _truckFinalPointTrigger.IsFinalPointReached += _truckPlayerTrigger.Deactivate;
        }

        private void FixedUpdate()
        {
            if (_enemySpawnFirstWaveTrigger.IsEnemySpawned)
            {
                CreateWaveOfEnemy(FirstWaveEnemy);
            }
            
            if (_enemySpawnSecondWaveTrigger.IsEnemySpawned)
            {
                CreateWaveOfEnemy(SecondWaveEnemy);
            }
        }

        protected override void CreateWaveOfEnemy(int numberWaveEnemy)
        {
            if (numberWaveEnemy == FirstWaveEnemy)
            {
                base.CreateWaveOfEnemy(FirstWaveEnemy);
            }
            else
            {
                if (LastSpawnTime <= MinValue)
                {
                    CreateWaveOfSmallEnemies(numberWaveEnemy);
                    CreateWaveOfGunnerEnemies(numberWaveEnemy);
                
                    LastSpawnTime = SpawnWaveOfEnemyDelay;
                }

                LastSpawnTime -= Time.fixedDeltaTime;
            }
        }

        private void OnCreateBigEnemiesWave()
        {
            CreateWaveOfBigEnemies(SecondWaveEnemy);
        }
        
        private void OnDestroy()
        {
            _enemySpawnFirstWaveTrigger.EnemySpawned -= _entranceLastLvlTrigger.Deactivate;
            _enemySpawnSecondWaveTrigger.EnemySpawned -= OnCreateBigEnemiesWave;

            _truckFinalPointTrigger.IsFinalPointReached -= EntranceToNextLvlTrigger.Activate;
            _truckFinalPointTrigger.IsFinalPointReached -= _entranceLastLvlTrigger.Activate;
            _truckFinalPointTrigger.IsFinalPointReached -= EndLevelTrigger.Activate;
            _truckFinalPointTrigger.IsFinalPointReached -= _truckPlayerTrigger.Deactivate;
        }
    }
}