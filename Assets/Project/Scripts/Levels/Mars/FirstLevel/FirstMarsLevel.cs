using Project.Scripts.Levels.Triggers;
using Project.Scripts.UI.View;
using UnityEngine;

namespace Project.Scripts.Levels.Mars.FirstLevel
{
    public class FirstMarsLevel : Level
    {
        [SerializeField] private EnemySpawnTriggerWithEffect _enemySpawnTriggerWithEffect;
        [SerializeField] private int _timeOfWaves = 90;

        private Timer _timer;
        private ObjectiveTextView _objectiveTextView;

        private void OnEnable()
        {
            IsInitiatedSpawners += SpawnResources;
        }

        private void OnDisable()
        {
            IsInitiatedSpawners -= SpawnResources;
        }

        public override async void OnStartLevel()
        {
            base.OnStartLevel();
            
            _timer = await ViewFactory.CreateTimer();
            _objectiveTextView = await ViewFactory.CreateObjectiveText();
            _objectiveTextView.Hide();
            
            WelcomePlanetTextTrigger.IsWelcomeToPlanet += DialogueSetter.OnWelcomePlanet;
            
            _timer.SetTime(_timeOfWaves);

            PauseService.OnGameStarted += _timer.ResumeTimer;
            PauseService.OnGamePaused += _timer.PauseTimer;

            _enemySpawnTriggerWithEffect.EnemySpawned += _timer.Show;
            _enemySpawnTriggerWithEffect.EnemySpawned += DialogueSetter.OnEnemySpawnTriggerWithEffect;
            
            _timer.IsEndAttack += DialogueSetter.OnEndAttack;
            _timer.IsEndAttack += _enemySpawnTriggerWithEffect.CompleteSpawn;
            _timer.IsEndAttack += EntranceToNextLvlTrigger.Activate;
            _timer.IsEndAttack += EndLevelTrigger.Activate;
            _timer.IsEndAttack += _objectiveTextView.Show;
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
            
            PauseService.OnGameStarted -= _timer.ResumeTimer;
            PauseService.OnGamePaused -= _timer.PauseTimer;
            
            _enemySpawnTriggerWithEffect.EnemySpawned -= _timer.Show;
            _enemySpawnTriggerWithEffect.EnemySpawned -= DialogueSetter.OnEnemySpawnTriggerWithEffect;
            
            _timer.IsEndAttack -= DialogueSetter.OnEndAttack;
            _timer.IsEndAttack -= _enemySpawnTriggerWithEffect.CompleteSpawn;
            _timer.IsEndAttack -= EntranceToNextLvlTrigger.Activate;
            _timer.IsEndAttack -= EndLevelTrigger.Activate;
            _timer.IsEndAttack -= _objectiveTextView.Show;
        }
    }
}