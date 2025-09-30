using Project.Scripts.Levels.Triggers;
using UnityEngine;

namespace Project.Scripts.Levels.Mars.FirstLevel
{
    public class FirstMarsLevel : Level
    {
        private readonly FirstLevelAdviserTextsSetter _firstLevelAdviserTextsSetter = new ();
        
        [SerializeField] private EnemySpawnFirstWaveTrigger _enemySpawnFirstWaveTrigger;
        [SerializeField] private WelcomePlanetTextTrigger _welcomePlanetTextTrigger;
        [SerializeField] private int _timeOfWaves = 90;

        private void OnEnable()
        {
            _welcomePlanetTextTrigger.IsWelcomeToPlanet += _firstLevelAdviserTextsSetter.SetAndShowWelcomePlanetText;
            
            IsInitiatedSpawners += SpawnResources;
        }

        private void OnDisable()
        {
            _welcomePlanetTextTrigger.IsWelcomeToPlanet -= _firstLevelAdviserTextsSetter.SetAndShowWelcomePlanetText;
            
            IsInitiatedSpawners -= SpawnResources;
        }

        public override void OnStartLevel()
        {
            base.OnStartLevel();
            
            Timer.SetTime(_timeOfWaves);
            
            _firstLevelAdviserTextsSetter.GetAdviserPanel(AdviserMessagePanel);
            
            PauseService.OnGameStarted += Timer.ResumeTimer;
            PauseService.OnGamePaused += Timer.PauseTimer;

            _enemySpawnFirstWaveTrigger.EnemySpawned += Timer.Show;
            _enemySpawnFirstWaveTrigger.EnemySpawned += _firstLevelAdviserTextsSetter.SetAndShowEnemySpawnFirstWaveTriggerText;
            
            Timer.IsEndAttack += _firstLevelAdviserTextsSetter.SetAndShowEndAttackText;
            Timer.IsEndAttack += _enemySpawnFirstWaveTrigger.CompleteSpawn;
            Timer.IsEndAttack += EntranceToNextLvlTrigger.Activate;
            Timer.IsEndAttack += EndLevelTrigger.Activate;
        }

        private void FixedUpdate()
        {
            if (_enemySpawnFirstWaveTrigger.IsEnemySpawned)
            {
                CreateWaveOfEnemy(FirstWaveEnemy);
            }
        }

        private void OnDestroy()
        {
            PauseService.OnGameStarted -= Timer.ResumeTimer;
            PauseService.OnGamePaused -= Timer.PauseTimer;
            
            _enemySpawnFirstWaveTrigger.EnemySpawned -= Timer.Show;
            _enemySpawnFirstWaveTrigger.EnemySpawned -= _firstLevelAdviserTextsSetter.SetAndShowEnemySpawnFirstWaveTriggerText;
            
            Timer.IsEndAttack -= _firstLevelAdviserTextsSetter.SetAndShowEndAttackText;
            Timer.IsEndAttack -= _enemySpawnFirstWaveTrigger.CompleteSpawn;
            Timer.IsEndAttack -= EntranceToNextLvlTrigger.Activate;
            Timer.IsEndAttack -= EndLevelTrigger.Activate;
        }
    }
}