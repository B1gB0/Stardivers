using Project.Scripts.Levels.Triggers;
using Project.Scripts.UI.View;
using UnityEngine;

namespace Project.Scripts.Levels.Mars.SecondLevel
{
    public class SecondMarsLevel : Level
    {
        [field: SerializeField] public BallisticRocket _ballisticRocket { get; private set; }
        
        [SerializeField] private BallisticRocketTrigger _ballisticRocketTrigger;
        [SerializeField] private EntranceTrigger _entranceLastLvlTrigger;

        private MissionProgressBar _missionProgressBar;

        private void Start()
        {
            _ballisticRocketTrigger.Deactivate();
        }

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
            
            _missionProgressBar.SetData();

            WelcomePlanetTextTrigger.IsWelcomeToPlanet += DialogueSetter.OnWelcomePlanet;
            
            EnemySpawnFirstWaveTrigger.EnemySpawned += _ballisticRocketTrigger.Activate;
            EnemySpawnFirstWaveTrigger.EnemySpawned += _entranceLastLvlTrigger.Deactivate;
            EnemySpawnFirstWaveTrigger.EnemySpawned += _missionProgressBar.Show;
            EnemySpawnFirstWaveTrigger.EnemySpawned += DialogueSetter.OnEnemySpawnTrigger;

            _ballisticRocket.LaunchCompleted += DialogueSetter.OnEndAttack;
            _ballisticRocket.LaunchCompleted += EnemySpawnFirstWaveTrigger.CompleteSpawn;
            _ballisticRocket.LaunchCompleted += EndLevelTrigger.Activate;
            _ballisticRocket.LaunchCompleted += EntranceToNextLvlTrigger.Activate;
            _ballisticRocket.LaunchCompleted += _entranceLastLvlTrigger.Activate;
        }

        private void FixedUpdate()
        {
            if (EnemySpawnFirstWaveTrigger.IsEnemySpawned)
            {
                CreateWaveOfEnemy(FirstWaveEnemy);
            }
        }
        
        public void GetBallisticProgressBar(MissionProgressBar missionProgressBar)
        {
            _missionProgressBar = missionProgressBar;
            _ballisticRocket.ProgressChanged += _missionProgressBar.OnChangedValues;
        }

        private void OnDestroy()
        {
            WelcomePlanetTextTrigger.IsWelcomeToPlanet -= DialogueSetter.OnWelcomePlanet;
            
            _ballisticRocket.ProgressChanged -= _missionProgressBar.OnChangedValues;
            
            EnemySpawnFirstWaveTrigger.EnemySpawned -= _ballisticRocketTrigger.Activate;
            EnemySpawnFirstWaveTrigger.EnemySpawned -= _entranceLastLvlTrigger.Deactivate;
            EnemySpawnFirstWaveTrigger.EnemySpawned -= _missionProgressBar.Show;
            EnemySpawnFirstWaveTrigger.EnemySpawned -= DialogueSetter.OnEnemySpawnTrigger;

            _ballisticRocket.LaunchCompleted -= DialogueSetter.OnEndAttack;
            _ballisticRocket.LaunchCompleted -= EnemySpawnFirstWaveTrigger.CompleteSpawn;
            _ballisticRocket.LaunchCompleted -= EndLevelTrigger.Activate;
            _ballisticRocket.LaunchCompleted -= EntranceToNextLvlTrigger.Activate;
            _ballisticRocket.LaunchCompleted -= _entranceLastLvlTrigger.Activate;
        }
    }
}