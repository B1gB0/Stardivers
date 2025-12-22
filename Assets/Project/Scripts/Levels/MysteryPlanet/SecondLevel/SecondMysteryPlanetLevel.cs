using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Project.Scripts.Levels.Triggers;
using Project.Scripts.UI.View;
using UnityEngine;

namespace Project.Scripts.Levels.MysteryPlanet.SecondLevel
{
    public class SecondMysteryPlanetLevel : Level
    {
        [field: SerializeField] public RadioTower _radioTower { get; private set; }
        
        [SerializeField] private List<GameObject> _enemySpawnedPointers;
        [SerializeField] private List<GameObject> _enemyOutpostPointers;
        
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
            IsInitiatedSpawners += SpawnIceCrystals;
        }

        private void OnDisable()
        {
            IsInitiatedSpawners -= SpawnResources;
            IsInitiatedSpawners -= SpawnIceCrystals;
        }

        public override async UniTask OnStartLevel()
        {
            HideOutpostPointers();
            
            await base.OnStartLevel();
            
            Arrow.OnLookAtTarget(_enemySpawnTriggerWithEffect.transform);
            
            _missionProgressBar = await ViewFactory.CreateMissionProgressBar();
            
            _missionProgressBar.SetData();
            
            WelcomePlanetTextTrigger.IsWelcomeToPlanet += DialogueSetter.OnWelcomePlanet;
            
            _radioTower.ProgressChanged += _missionProgressBar.OnChangedValues;

            _enemySpawnTriggerWithEffect.EnemySpawned += _radioTowerTrigger.Activate;
            _enemySpawnTriggerWithEffect.EnemySpawned += HideEnemySpawnedPointers;
            _enemySpawnTriggerWithEffect.EnemySpawned += LookArrowAtBallisticRocketTrigger;
            _enemySpawnTriggerWithEffect.EnemySpawned += _entranceLastLvlTrigger.Deactivate;
            _enemySpawnTriggerWithEffect.EnemySpawned += _missionProgressBar.Show;
            _enemySpawnTriggerWithEffect.EnemySpawned += DialogueSetter.OnEnemySpawnTriggerWithEffect;

            _radioTower.InstallationDishCompleted += DialogueSetter.OnEndAttack;
            _radioTower.InstallationDishCompleted += ShowOutpostPointers;
            _radioTower.InstallationDishCompleted += ArrowLookAtOutpost;
            _radioTower.InstallationDishCompleted += _enemySpawnTriggerWithEffect.CompleteSpawn;
            _radioTower.InstallationDishCompleted += EndLevelTrigger.Activate;
            _radioTower.InstallationDishCompleted += EntranceToNextLvlTrigger.Activate;
            _radioTower.InstallationDishCompleted += _entranceLastLvlTrigger.Activate;
            _radioTower.InstallationDishCompleted += _radioTowerTrigger.Deactivate;
        }

        private void FixedUpdate()
        {
            if (_enemySpawnTriggerWithEffect.IsEnemySpawned)
            {
                CreateWaveOfEnemy(FirstWaveEnemy);
            }
        }

        private void OnDestroy()
        {
            WelcomePlanetTextTrigger.IsWelcomeToPlanet -= DialogueSetter.OnWelcomePlanet;
            
            _radioTower.ProgressChanged -= _missionProgressBar.OnChangedValues;
            
            _enemySpawnTriggerWithEffect.EnemySpawned -= _radioTowerTrigger.Activate;
            _enemySpawnTriggerWithEffect.EnemySpawned -= HideEnemySpawnedPointers;
            _enemySpawnTriggerWithEffect.EnemySpawned -= LookArrowAtBallisticRocketTrigger;
            _enemySpawnTriggerWithEffect.EnemySpawned -= _entranceLastLvlTrigger.Deactivate;
            _enemySpawnTriggerWithEffect.EnemySpawned -= _missionProgressBar.Show;
            _enemySpawnTriggerWithEffect.EnemySpawned -= DialogueSetter.OnEnemySpawnTriggerWithEffect;

            _radioTower.InstallationDishCompleted -= DialogueSetter.OnEndAttack;
            _radioTower.InstallationDishCompleted -= ShowOutpostPointers;
            _radioTower.InstallationDishCompleted -= ArrowLookAtOutpost;
            _radioTower.InstallationDishCompleted -= _enemySpawnTriggerWithEffect.CompleteSpawn;
            _radioTower.InstallationDishCompleted -= EndLevelTrigger.Activate;
            _radioTower.InstallationDishCompleted -= EntranceToNextLvlTrigger.Activate;
            _radioTower.InstallationDishCompleted -= _entranceLastLvlTrigger.Activate;
            _radioTower.InstallationDishCompleted -= _radioTowerTrigger.Deactivate;
        }
        
        private void LookArrowAtBallisticRocketTrigger()
        {
            Arrow.OnLookAtTarget(_radioTowerTrigger.transform);
        }
        
        private void ShowOutpostPointers()
        {
            foreach (var pointer in _enemyOutpostPointers)
            {
                pointer.gameObject.SetActive(true);
            }
        }
        
        private void HideOutpostPointers()
        {
            foreach (var pointer in _enemyOutpostPointers)
            {
                pointer.gameObject.SetActive(false);
            }
        }

        private void HideEnemySpawnedPointers()
        {
            foreach (var pointer in _enemySpawnedPointers)
            {
                pointer.gameObject.SetActive(false);
            }
        }
    }
}