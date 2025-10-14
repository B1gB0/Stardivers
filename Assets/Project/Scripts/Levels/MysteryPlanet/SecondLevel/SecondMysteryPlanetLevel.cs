using System;
using Project.Scripts.Levels.Mars.SecondLevel;
using Project.Scripts.Levels.Triggers;
using Project.Scripts.UI.View;
using UnityEngine;

namespace Project.Scripts.Levels.MysteryPlanet.SecondLevel
{
    public class SecondMysteryPlanetLevel : Level
    {
        [field: SerializeField] public RadioTower _radioTower { get; private set; }
        
        [SerializeField] private RadioTowerTrigger _radioTowerTrigger;
        [SerializeField] private EnemySpawnFirstWaveTrigger _enemySpawnFirstWaveTrigger;
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
            
            base.OnStartLevel();
            
            _enemySpawnFirstWaveTrigger.EnemySpawned += _radioTowerTrigger.Activate;
            _enemySpawnFirstWaveTrigger.EnemySpawned += _entranceLastLvlTrigger.Deactivate;
            _enemySpawnFirstWaveTrigger.EnemySpawned += _missionProgressBar.Show;

            _radioTower.InstallationDishCompleted += _enemySpawnFirstWaveTrigger.CompleteSpawn;
            _radioTower.InstallationDishCompleted += EndLevelTrigger.Activate;
            _radioTower.InstallationDishCompleted += EntranceToNextLvlTrigger.Activate;
            _radioTower.InstallationDishCompleted += _entranceLastLvlTrigger.Activate;
        }

        private void FixedUpdate()
        {
            if (_enemySpawnFirstWaveTrigger.IsEnemySpawned)
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
            _radioTower.ProgressChanged -= _missionProgressBar.OnChangedValues;
            
            _enemySpawnFirstWaveTrigger.EnemySpawned -= _radioTowerTrigger.Activate;
            _enemySpawnFirstWaveTrigger.EnemySpawned -= _entranceLastLvlTrigger.Deactivate;
            _enemySpawnFirstWaveTrigger.EnemySpawned -= _missionProgressBar.Show;

            _radioTower.InstallationDishCompleted += _enemySpawnFirstWaveTrigger.CompleteSpawn;
            _radioTower.InstallationDishCompleted += EndLevelTrigger.Activate;
            _radioTower.InstallationDishCompleted += EntranceToNextLvlTrigger.Activate;
            _radioTower.InstallationDishCompleted += _entranceLastLvlTrigger.Activate;
        }
    }
}