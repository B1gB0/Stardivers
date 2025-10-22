using Project.Scripts.Levels.Triggers;
using UnityEngine;

namespace Project.Scripts.Levels.Mars.ThirdLevel
{
    public class ThirdMarsLevel : Level
    {
        [SerializeField] private EnemySpawnTriggerWithEffect _enemySpawnTriggerWithEffect;
        [SerializeField] private EnemySpawnTriggerWithoutEffect _enemySpawnTriggerWithoutEffect;
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
            
            WelcomePlanetTextTrigger.IsWelcomeToPlanet += DialogueSetter.OnWelcomePlanet;
            
            _enemySpawnTriggerWithEffect.EnemySpawned += _entranceLastLvlTrigger.Deactivate;
            _enemySpawnTriggerWithEffect.EnemySpawned += DialogueSetter.OnEnemySpawnTriggerWithEffect;
            _enemySpawnTriggerWithoutEffect.EnemySpawned += OnCreateBigEnemiesWave;

            _truckFinalPointTrigger.IsFinalPointReached += DialogueSetter.OnEndAttack;
            _truckFinalPointTrigger.IsFinalPointReached += EntranceToNextLvlTrigger.Activate;
            _truckFinalPointTrigger.IsFinalPointReached += _entranceLastLvlTrigger.Activate;
            _truckFinalPointTrigger.IsFinalPointReached += EndLevelTrigger.Activate;
            _truckFinalPointTrigger.IsFinalPointReached += _truckPlayerTrigger.Deactivate;
        }

        private void FixedUpdate()
        {
            if (_enemySpawnTriggerWithEffect.IsEnemySpawned)
            {
                CreateWaveOfEnemy(FirstWaveEnemy);
            }
            
            if (_enemySpawnTriggerWithoutEffect.IsEnemySpawned)
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
            WelcomePlanetTextTrigger.IsWelcomeToPlanet -= DialogueSetter.OnWelcomePlanet;
            
            _enemySpawnTriggerWithEffect.EnemySpawned -= _entranceLastLvlTrigger.Deactivate;
            _enemySpawnTriggerWithEffect.EnemySpawned -= DialogueSetter.OnEnemySpawnTriggerWithEffect;
            _enemySpawnTriggerWithoutEffect.EnemySpawned -= OnCreateBigEnemiesWave;

            _truckFinalPointTrigger.IsFinalPointReached -= DialogueSetter.OnEndAttack;
            _truckFinalPointTrigger.IsFinalPointReached -= EntranceToNextLvlTrigger.Activate;
            _truckFinalPointTrigger.IsFinalPointReached -= _entranceLastLvlTrigger.Activate;
            _truckFinalPointTrigger.IsFinalPointReached -= EndLevelTrigger.Activate;
            _truckFinalPointTrigger.IsFinalPointReached -= _truckPlayerTrigger.Deactivate;
        }
    }
}