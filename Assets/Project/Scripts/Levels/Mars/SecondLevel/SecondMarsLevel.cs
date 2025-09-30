using Project.Scripts.Levels.Triggers;
using Project.Scripts.UI.View;
using UnityEngine;

namespace Project.Scripts.Levels.Mars.SecondLevel
{
    public class SecondMarsLevel : Level
    {
        [field: SerializeField] public BallisticRocket _ballisticRocket { get; private set; }
        
        [SerializeField] private BallisticRocketTrigger _ballisticRocketTrigger;
        [SerializeField] private EnemySpawnFirstWaveTrigger _enemySpawnFirstWaveTrigger;
        [SerializeField] private EntranceTrigger _entranceLastLvlTrigger;

        private BallisticRocketProgressBar _ballisticRocketProgressBar;

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
            
            _enemySpawnFirstWaveTrigger.EnemySpawned += _ballisticRocketTrigger.Activate;
            _enemySpawnFirstWaveTrigger.EnemySpawned += _entranceLastLvlTrigger.Deactivate;
            _enemySpawnFirstWaveTrigger.EnemySpawned += _ballisticRocketProgressBar.Show;

            _ballisticRocket.LaunchCompleted += _enemySpawnFirstWaveTrigger.CompleteSpawn;
            _ballisticRocket.LaunchCompleted += EndLevelTrigger.Activate;
            _ballisticRocket.LaunchCompleted += EntranceToNextLvlTrigger.Activate;
            _ballisticRocket.LaunchCompleted += _entranceLastLvlTrigger.Activate;
        }

        private void FixedUpdate()
        {
            if (_enemySpawnFirstWaveTrigger.IsEnemySpawned)
            {
                CreateWaveOfEnemy(FirstWaveEnemy);
            }
        }
        
        public void GetBallisticProgressBar(BallisticRocketProgressBar ballisticRocketProgressBar)
        {
            _ballisticRocketProgressBar = ballisticRocketProgressBar;
            _ballisticRocket.ProgressChanged += _ballisticRocketProgressBar.OnChangedValues;
        }

        private void OnDestroy()
        {
            _ballisticRocket.ProgressChanged -= _ballisticRocketProgressBar.OnChangedValues;
            
            _enemySpawnFirstWaveTrigger.EnemySpawned -= _ballisticRocketTrigger.Activate;
            _enemySpawnFirstWaveTrigger.EnemySpawned -= _entranceLastLvlTrigger.Deactivate;
            _enemySpawnFirstWaveTrigger.EnemySpawned -= _ballisticRocketProgressBar.Show;

            _ballisticRocket.LaunchCompleted -= _enemySpawnFirstWaveTrigger.CompleteSpawn;
            _ballisticRocket.LaunchCompleted -= EndLevelTrigger.Activate;
            _ballisticRocket.LaunchCompleted -= EntranceToNextLvlTrigger.Activate;
            _ballisticRocket.LaunchCompleted -= _entranceLastLvlTrigger.Activate;
        }
    }
}