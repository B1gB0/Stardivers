using Project.Scripts.Levels.Triggers;
using Project.Scripts.UI.View;
using UnityEngine;

namespace Project.Scripts.Levels.MysteryPlanet.SecondLevel
{
    public class SecondMysteryPlanetLevel : Level
    {
        [field: SerializeField] public RadioTower _radioTower { get; private set; }
        
        [SerializeField] private RadioTowerTrigger _radioTowerTrigger;
        [SerializeField] private EntranceTrigger _entranceLastLvlTrigger;
        
        private MissionProgressBar _missionProgressBar;

        private void Start()
        {
            _radioTowerTrigger.Deactivate();
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

            EnemySpawnFirstWaveTrigger.EnemySpawned += _radioTowerTrigger.Activate;
            EnemySpawnFirstWaveTrigger.EnemySpawned += _entranceLastLvlTrigger.Deactivate;
            EnemySpawnFirstWaveTrigger.EnemySpawned += _missionProgressBar.Show;
            EnemySpawnFirstWaveTrigger.EnemySpawned += DialogueSetter.OnEnemySpawnTrigger;

            _radioTower.InstallationDishCompleted += DialogueSetter.OnEndAttack;
            _radioTower.InstallationDishCompleted += EnemySpawnFirstWaveTrigger.CompleteSpawn;
            _radioTower.InstallationDishCompleted += EndLevelTrigger.Activate;
            _radioTower.InstallationDishCompleted += EntranceToNextLvlTrigger.Activate;
            _radioTower.InstallationDishCompleted += _entranceLastLvlTrigger.Activate;
        }

        private void FixedUpdate()
        {
            if (EnemySpawnFirstWaveTrigger.IsEnemySpawned)
            {
                CreateWaveOfEnemy(FirstWaveEnemy);
            }
        }
        
        public void GetRadioTowerProgressBar(MissionProgressBar missionProgressBar)
        {
            _missionProgressBar = missionProgressBar;
            _radioTower.ProgressChanged += _missionProgressBar.OnChangedValues;
        }
        
        private void OnDestroy()
        {
            WelcomePlanetTextTrigger.IsWelcomeToPlanet -= DialogueSetter.OnWelcomePlanet;
            
            _radioTower.ProgressChanged -= _missionProgressBar.OnChangedValues;
            
            EnemySpawnFirstWaveTrigger.EnemySpawned -= _radioTowerTrigger.Activate;
            EnemySpawnFirstWaveTrigger.EnemySpawned -= _entranceLastLvlTrigger.Deactivate;
            EnemySpawnFirstWaveTrigger.EnemySpawned -= _missionProgressBar.Show;
            EnemySpawnFirstWaveTrigger.EnemySpawned -= DialogueSetter.OnEnemySpawnTrigger;

            _radioTower.InstallationDishCompleted -= DialogueSetter.OnEndAttack;
            _radioTower.InstallationDishCompleted -= EnemySpawnFirstWaveTrigger.CompleteSpawn;
            _radioTower.InstallationDishCompleted -= EndLevelTrigger.Activate;
            _radioTower.InstallationDishCompleted -= EntranceToNextLvlTrigger.Activate;
            _radioTower.InstallationDishCompleted -= _entranceLastLvlTrigger.Activate;
        }
    }
}