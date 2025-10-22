using Project.Scripts.Levels.Triggers;
using Project.Scripts.UI.View;
using UnityEngine;

namespace Project.Scripts.Levels.MysteryPlanet.SecondLevel
{
    public class SecondMysteryPlanetLevel : Level
    {
        [field: SerializeField] public RadioTower _radioTower { get; private set; }
        
        [SerializeField] private EnemySpawnTriggerWithEffect _enemySpawnTriggerWithEffect;
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

            _enemySpawnTriggerWithEffect.EnemySpawned += _radioTowerTrigger.Activate;
            _enemySpawnTriggerWithEffect.EnemySpawned += _entranceLastLvlTrigger.Deactivate;
            _enemySpawnTriggerWithEffect.EnemySpawned += _missionProgressBar.Show;
            _enemySpawnTriggerWithEffect.EnemySpawned += DialogueSetter.OnEnemySpawnTriggerWithEffect;

            _radioTower.InstallationDishCompleted += DialogueSetter.OnEndAttack;
            _radioTower.InstallationDishCompleted += _enemySpawnTriggerWithEffect.CompleteSpawn;
            _radioTower.InstallationDishCompleted += EndLevelTrigger.Activate;
            _radioTower.InstallationDishCompleted += EntranceToNextLvlTrigger.Activate;
            _radioTower.InstallationDishCompleted += _entranceLastLvlTrigger.Activate;
        }

        private void FixedUpdate()
        {
            if (_enemySpawnTriggerWithEffect.IsEnemySpawned)
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
            
            _enemySpawnTriggerWithEffect.EnemySpawned -= _radioTowerTrigger.Activate;
            _enemySpawnTriggerWithEffect.EnemySpawned -= _entranceLastLvlTrigger.Deactivate;
            _enemySpawnTriggerWithEffect.EnemySpawned -= _missionProgressBar.Show;
            _enemySpawnTriggerWithEffect.EnemySpawned -= DialogueSetter.OnEnemySpawnTriggerWithEffect;

            _radioTower.InstallationDishCompleted -= DialogueSetter.OnEndAttack;
            _radioTower.InstallationDishCompleted -= _enemySpawnTriggerWithEffect.CompleteSpawn;
            _radioTower.InstallationDishCompleted -= EndLevelTrigger.Activate;
            _radioTower.InstallationDishCompleted -= EntranceToNextLvlTrigger.Activate;
            _radioTower.InstallationDishCompleted -= _entranceLastLvlTrigger.Activate;
        }
    }
}