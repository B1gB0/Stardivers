using Project.Scripts.Levels.Triggers;
using UnityEngine;

namespace Project.Scripts.Levels.Mars.ThirdLevel
{
    public class ThirdMarsLevel : Level
    {
        [SerializeField] private EnemySpawnSecondWaveTrigger _enemySpawnSecondWaveTrigger;
        [SerializeField] private EntranceTrigger _entranceLastLvlTrigger;
        [SerializeField] private TruckPlayerTrigger _truckPlayerTrigger;
        [SerializeField] private TruckFinalPointTrigger _truckFinalPointTrigger;

        private void OnEnable()
        {
            WelcomePlanetTextTrigger.IsWelcomeToPlanet += DialogueSetter.OnWelcomePlanet;
            
            IsInitiatedSpawners += SpawnResources;
        }

        private void OnDisable()
        {
            WelcomePlanetTextTrigger.IsWelcomeToPlanet -= DialogueSetter.OnWelcomePlanet;
            
            IsInitiatedSpawners -= SpawnResources;
        }

        public override void OnStartLevel()
        {
            base.OnStartLevel();
            
            EnemySpawnFirstWaveTrigger.EnemySpawned += _entranceLastLvlTrigger.Deactivate;
            EnemySpawnFirstWaveTrigger.EnemySpawned += DialogueSetter.OnEnemySpawnTrigger;
            _enemySpawnSecondWaveTrigger.EnemySpawned += OnCreateBigEnemiesWave;

            _truckFinalPointTrigger.IsFinalPointReached += DialogueSetter.OnEndAttack;
            _truckFinalPointTrigger.IsFinalPointReached += EntranceToNextLvlTrigger.Activate;
            _truckFinalPointTrigger.IsFinalPointReached += _entranceLastLvlTrigger.Activate;
            _truckFinalPointTrigger.IsFinalPointReached += EndLevelTrigger.Activate;
            _truckFinalPointTrigger.IsFinalPointReached += _truckPlayerTrigger.Deactivate;
        }

        private void FixedUpdate()
        {
            if (EnemySpawnFirstWaveTrigger.IsEnemySpawned)
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
            EnemySpawnFirstWaveTrigger.EnemySpawned -= _entranceLastLvlTrigger.Deactivate;
            EnemySpawnFirstWaveTrigger.EnemySpawned -= DialogueSetter.OnEnemySpawnTrigger;
            _enemySpawnSecondWaveTrigger.EnemySpawned -= OnCreateBigEnemiesWave;

            _truckFinalPointTrigger.IsFinalPointReached -= DialogueSetter.OnEndAttack;
            _truckFinalPointTrigger.IsFinalPointReached -= EntranceToNextLvlTrigger.Activate;
            _truckFinalPointTrigger.IsFinalPointReached -= _entranceLastLvlTrigger.Activate;
            _truckFinalPointTrigger.IsFinalPointReached -= EndLevelTrigger.Activate;
            _truckFinalPointTrigger.IsFinalPointReached -= _truckPlayerTrigger.Deactivate;
        }
    }
}