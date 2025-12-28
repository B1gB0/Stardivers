using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Project.Scripts.Levels.Triggers;
using Project.Scripts.UI.View;
using UnityEngine;

namespace Project.Scripts.Levels.Mars.FirstLevel
{
    public class FirstMarsLevel : Level
    {
        [SerializeField] private List<GameObject> _enemySpawnedPointers;
        [SerializeField] private List<GameObject> _enemyOutpostPointers;
        
        [SerializeField] private EnemySpawnTriggerWithEffect _enemySpawnTriggerWithEffect;
        [SerializeField] private int _timeOfWaves = 90;

        private Timer _timer;
        private ObjectiveTextView _objectiveTextView;

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
        }
        
        private void OnDisable()
        {
            IsInitiatedSpawners -= SpawnResources;
        }

        private void OnDestroy()
        {
            WelcomePlanetTextTrigger.IsWelcomeToPlanet -= DialogueSetter.OnWelcomePlanet;
            
            PauseService.OnGameStarted -= _timer.ResumeTimer;
            PauseService.OnGamePaused -= _timer.PauseTimer;
            
            _enemySpawnTriggerWithEffect.EnemySpawned -= _timer.Show;
            _enemySpawnTriggerWithEffect.EnemySpawned -= HideEnemySpawnedPointers;
            _enemySpawnTriggerWithEffect.EnemySpawned -= Arrow.Hide;
            _enemySpawnTriggerWithEffect.EnemySpawned -= DialogueSetter.OnEnemySpawnTriggerWithEffect;
            
            _timer.IsEndAttack -= DialogueSetter.OnEndAttack;
            _timer.IsEndAttack -= Arrow.Show;
            _timer.IsEndAttack -= ShowOutpostPointers;
            _timer.IsEndAttack -= ArrowLookAtOutpost;
            _timer.IsEndAttack -= _enemySpawnTriggerWithEffect.CompleteSpawn;
            _timer.IsEndAttack -= EntranceToNextLvlTrigger.Activate;
            _timer.IsEndAttack -= EndLevelTrigger.Activate;
            _timer.IsEndAttack -= _objectiveTextView.Show;
        }
        
        public override async UniTask OnStartLevel()
        {
            HideOutpostPointers();
            
            await base.OnStartLevel();
            
            Arrow.OnLookAtTarget(_enemySpawnTriggerWithEffect.transform);

            _timer = await ViewFactory.CreateTimer();
            _objectiveTextView = await ViewFactory.CreateObjectiveText();

            WelcomePlanetTextTrigger.IsWelcomeToPlanet += DialogueSetter.OnWelcomePlanet;
            
            _timer.SetTime(_timeOfWaves);

            PauseService.OnGameStarted += _timer.ResumeTimer;
            PauseService.OnGamePaused += _timer.PauseTimer;

            _enemySpawnTriggerWithEffect.EnemySpawned += _timer.Show;
            _enemySpawnTriggerWithEffect.EnemySpawned += HideEnemySpawnedPointers;
            _enemySpawnTriggerWithEffect.EnemySpawned += Arrow.Hide;
            _enemySpawnTriggerWithEffect.EnemySpawned += DialogueSetter.OnEnemySpawnTriggerWithEffect;
            
            _timer.IsEndAttack += DialogueSetter.OnEndAttack;
            _timer.IsEndAttack += Arrow.Show;
            _timer.IsEndAttack += ShowOutpostPointers;
            _timer.IsEndAttack += ArrowLookAtOutpost;
            _timer.IsEndAttack += _enemySpawnTriggerWithEffect.CompleteSpawn;
            _timer.IsEndAttack += EntranceToNextLvlTrigger.Activate;
            _timer.IsEndAttack += EndLevelTrigger.Activate;
            _timer.IsEndAttack += _objectiveTextView.Show;
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