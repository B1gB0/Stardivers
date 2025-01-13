using Project.Scripts.Levels.Triggers;
using Project.Scripts.UI.View;
using UnityEngine;

namespace Project.Scripts.Levels.Mars.SecondLevel
{
    public class SecondMarsLevel : Level
    {
        [field: SerializeField] public BallisticRocket _ballisticRocket { get; private set; }
        
        [SerializeField] private BallisticRocketTrigger _ballisticRocketTrigger;
        [SerializeField] private EnemySpawnTrigger _enemySpawnTrigger;

        private BallisticRocketProgressBar _ballisticRocketProgressBar;

        private void Start()
        {
            SpawnPlayer();
            
            _enemySpawnTrigger.EnemySpawned += _ballisticRocketTrigger.Activate;

            _ballisticRocket.LaunchCompleted += _enemySpawnTrigger.CompleteSpawn;
            _ballisticRocket.LaunchCompleted += EndLevelTrigger.Activate;
            _ballisticRocket.LaunchCompleted += EntranceTrigger.Activate;
        }

        private void Update()
        {
            if (_enemySpawnTrigger.IsEnemySpawned)
            {
                CreateWaveOfEnemy();
            }
        }
        
        public void GetBallisticProgressBar(BallisticRocketProgressBar ballisticRocketProgressBar)
        {
            _ballisticRocketProgressBar = ballisticRocketProgressBar;
            _ballisticRocketProgressBar.Show();
            _ballisticRocket.ProgressChanged += _ballisticRocketProgressBar.OnChangedValues;
        }

        private void OnDestroy()
        {
            _ballisticRocket.ProgressChanged -= _ballisticRocketProgressBar.OnChangedValues;
            
            _enemySpawnTrigger.EnemySpawned -= _ballisticRocketTrigger.Activate;

            _ballisticRocket.LaunchCompleted -= _enemySpawnTrigger.CompleteSpawn;
            _ballisticRocket.LaunchCompleted -= EndLevelTrigger.Activate;
            _ballisticRocket.LaunchCompleted -= EntranceTrigger.Activate;
        }
    }
}