using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Project.Scripts.Levels.Triggers;
using Project.Scripts.UI.View;
using UnityEngine;

namespace Project.Scripts.Levels.Mars.SecondLevel
{
    public class SecondMarsLevel : Level
    {
        [SerializeField] private List<GameObject> _enemySpawnedPointers;
        [SerializeField] private List<GameObject> _enemyOutpostPointers;

        [SerializeField] private EnemySpawnTriggerWithEffect _enemySpawnTriggerWithEffect;
        [SerializeField] private BallisticRocketTrigger _ballisticRocketTrigger;
        [SerializeField] private EntranceTrigger _entranceLastLvlTrigger;

        private MissionProgressBar _missionProgressBar;

        [field: SerializeField] public BallisticRocket BallisticRocket { get; private set; }

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

        public override async UniTask OnStartLevel()
        {
            HideOutpostPointers();

            await base.OnStartLevel();

            Arrow.OnLookAtTarget(_enemySpawnTriggerWithEffect.transform);

            _missionProgressBar = await ViewFactory.CreateMissionProgressBar();

            _missionProgressBar.SetData();

            BallisticRocket.ProgressChanged += _missionProgressBar.OnChangedValues;

            WelcomePlanetTextTrigger.IsWelcomeToPlanet += DialogueSetter.OnWelcomePlanet;

            _enemySpawnTriggerWithEffect.EnemySpawned += _ballisticRocketTrigger.Activate;
            _enemySpawnTriggerWithEffect.EnemySpawned += HideEnemySpawnedPointers;
            _enemySpawnTriggerWithEffect.EnemySpawned += LookArrowAtBallisticRocketTrigger;
            _enemySpawnTriggerWithEffect.EnemySpawned += _entranceLastLvlTrigger.Deactivate;
            _enemySpawnTriggerWithEffect.EnemySpawned += _missionProgressBar.Show;
            _enemySpawnTriggerWithEffect.EnemySpawned += DialogueSetter.OnEnemySpawnTriggerWithEffect;

            BallisticRocket.LaunchCompleted += DialogueSetter.OnEndAttack;
            BallisticRocket.LaunchCompleted += ShowOutpostPointers;
            BallisticRocket.LaunchCompleted += ArrowLookAtOutpost;
            BallisticRocket.LaunchCompleted += _enemySpawnTriggerWithEffect.CompleteSpawn;
            BallisticRocket.LaunchCompleted += EndLevelTrigger.Activate;
            BallisticRocket.LaunchCompleted += EntranceToNextLvlTrigger.Activate;
            BallisticRocket.LaunchCompleted += _entranceLastLvlTrigger.Activate;
            BallisticRocket.LaunchCompleted += _ballisticRocketTrigger.Deactivate;
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

            BallisticRocket.ProgressChanged -= _missionProgressBar.OnChangedValues;

            _enemySpawnTriggerWithEffect.EnemySpawned -= _ballisticRocketTrigger.Activate;
            _enemySpawnTriggerWithEffect.EnemySpawned -= HideEnemySpawnedPointers;
            _enemySpawnTriggerWithEffect.EnemySpawned -= LookArrowAtBallisticRocketTrigger;
            _enemySpawnTriggerWithEffect.EnemySpawned -= _entranceLastLvlTrigger.Deactivate;
            _enemySpawnTriggerWithEffect.EnemySpawned -= _missionProgressBar.Show;
            _enemySpawnTriggerWithEffect.EnemySpawned -= DialogueSetter.OnEnemySpawnTriggerWithEffect;

            BallisticRocket.LaunchCompleted -= DialogueSetter.OnEndAttack;
            BallisticRocket.LaunchCompleted -= ShowOutpostPointers;
            BallisticRocket.LaunchCompleted -= ArrowLookAtOutpost;
            BallisticRocket.LaunchCompleted -= _enemySpawnTriggerWithEffect.CompleteSpawn;
            BallisticRocket.LaunchCompleted -= EndLevelTrigger.Activate;
            BallisticRocket.LaunchCompleted -= EntranceToNextLvlTrigger.Activate;
            BallisticRocket.LaunchCompleted -= _entranceLastLvlTrigger.Activate;
        }

        private void LookArrowAtBallisticRocketTrigger()
        {
            Arrow.OnLookAtTarget(_ballisticRocketTrigger.transform);
        }

        private void ShowOutpostPointers()
        {
            foreach (var pointer in _enemyOutpostPointers)
            {
                pointer.SetActive(true);
            }
        }

        private void HideOutpostPointers()
        {
            foreach (var pointer in _enemyOutpostPointers)
            {
                pointer.SetActive(false);
            }
        }

        private void HideEnemySpawnedPointers()
        {
            foreach (var pointer in _enemySpawnedPointers)
            {
                pointer.SetActive(false);
            }
        }
    }
}