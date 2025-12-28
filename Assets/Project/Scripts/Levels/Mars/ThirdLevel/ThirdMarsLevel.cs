using Cysharp.Threading.Tasks;
using Project.Scripts.Levels.Triggers;
using Project.Scripts.UI.View;
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
        [SerializeField] private Truck _truck;
        
        private MissionProgressBar _missionProgressBar;

        private void OnEnable()
        {
            IsInitiatedSpawners += SpawnResources;
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

        private void OnDisable()
        {
            IsInitiatedSpawners -= SpawnResources;
        }
        
        private void OnDestroy()
        {
            WelcomePlanetTextTrigger.IsWelcomeToPlanet -= DialogueSetter.OnWelcomePlanet;
            
            _truck.ProgressChanged -= _missionProgressBar.OnChangeValuesSmoothly;
            
            _enemySpawnTriggerWithEffect.EnemySpawned -= _entranceLastLvlTrigger.Deactivate;
            _enemySpawnTriggerWithEffect.EnemySpawned -= LookAtTransport;
            _enemySpawnTriggerWithEffect.EnemySpawned -= DialogueSetter.OnEnemySpawnTriggerWithEffect;
            _enemySpawnTriggerWithEffect.EnemySpawned -= _missionProgressBar.Show;
            
            _enemySpawnTriggerWithoutEffect.EnemySpawned -= OnCreateBigEnemiesWave;

            _truckFinalPointTrigger.IsFinalPointReached -= DialogueSetter.OnEndAttack;
            _truckFinalPointTrigger.IsFinalPointReached -= ArrowLookAtOutpost;
            _truckFinalPointTrigger.IsFinalPointReached -= EntranceToNextLvlTrigger.Activate;
            _truckFinalPointTrigger.IsFinalPointReached -= _entranceLastLvlTrigger.Activate;
            _truckFinalPointTrigger.IsFinalPointReached -= EndLevelTrigger.Activate;
            _truckFinalPointTrigger.IsFinalPointReached -= _truckPlayerTrigger.Deactivate;
            _truckFinalPointTrigger.IsFinalPointReached -= _missionProgressBar.Hide;
        }

        public override async UniTask OnStartLevel()
        {
            await base.OnStartLevel();

            Arrow.OnLookAtTarget(_enemySpawnTriggerWithEffect.transform);
            
            _missionProgressBar = await ViewFactory.CreateMissionProgressBar();
            _missionProgressBar.SetData();

            WelcomePlanetTextTrigger.IsWelcomeToPlanet += DialogueSetter.OnWelcomePlanet;
            
            _truck.ProgressChanged += _missionProgressBar.OnChangeValuesSmoothly;

            _enemySpawnTriggerWithEffect.EnemySpawned += _entranceLastLvlTrigger.Deactivate;
            _enemySpawnTriggerWithEffect.EnemySpawned += LookAtTransport;
            _enemySpawnTriggerWithEffect.EnemySpawned += DialogueSetter.OnEnemySpawnTriggerWithEffect;
            _enemySpawnTriggerWithEffect.EnemySpawned += _missionProgressBar.Show;
            
            _enemySpawnTriggerWithoutEffect.EnemySpawned += OnCreateBigEnemiesWave;

            _truckFinalPointTrigger.IsFinalPointReached += DialogueSetter.OnEndAttack;
            _truckFinalPointTrigger.IsFinalPointReached += ArrowLookAtOutpost;
            _truckFinalPointTrigger.IsFinalPointReached += EntranceToNextLvlTrigger.Activate;
            _truckFinalPointTrigger.IsFinalPointReached += _entranceLastLvlTrigger.Activate;
            _truckFinalPointTrigger.IsFinalPointReached += EndLevelTrigger.Activate;
            _truckFinalPointTrigger.IsFinalPointReached += _truckPlayerTrigger.Deactivate;
            _truckFinalPointTrigger.IsFinalPointReached += _missionProgressBar.Hide;
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

        private void LookAtTransport()
        {
            Arrow.OnLookAtTarget(_truckPlayerTrigger.transform);
        }
    }
}