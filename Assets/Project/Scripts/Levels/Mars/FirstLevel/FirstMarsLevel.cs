using Project.Scripts.Levels.Triggers;
using UnityEngine;

namespace Project.Scripts.Levels.Mars.FirstLevel
{
    public class FirstMarsLevel : Level
    {
        private readonly FirstLevelAdviserTextsSetter _firstLevelAdviserTextsSetter = new ();
        
        [SerializeField] private EnemySpawnTrigger _enemySpawnTrigger;
        [SerializeField] private WelcomePlanetTextTrigger welcomePlanetTextTrigger;
        [SerializeField] private int _timeOfWaves = 90;

        private void OnEnable()
        {
            welcomePlanetTextTrigger.IsWelcomeToPlanet += _firstLevelAdviserTextsSetter.SetAndShowWelcomePlanetText;
            
            IsInitiatedSpawners += SpawnResources;
        }

        private void OnDisable()
        {
            welcomePlanetTextTrigger.IsWelcomeToPlanet -= _firstLevelAdviserTextsSetter.SetAndShowWelcomePlanetText;
            
            IsInitiatedSpawners -= SpawnResources;
        }

        public override void OnStartLevel()
        {
            base.OnStartLevel();
            
            Timer.SetTime(_timeOfWaves);
            
            _firstLevelAdviserTextsSetter.GetAdviserPanel(AdviserMessagePanel);
            
            PauseService.OnGameStarted += Timer.ResumeTimer;
            PauseService.OnGamePaused += Timer.PauseTimer;

            _enemySpawnTrigger.EnemySpawned += Timer.Show;
            _enemySpawnTrigger.EnemySpawned += _firstLevelAdviserTextsSetter.SetAndShowEnemySpawnTriggerText;
            
            Timer.IsEndAttack += _firstLevelAdviserTextsSetter.SetAndShowEndAttackText;
            Timer.IsEndAttack += _enemySpawnTrigger.CompleteSpawn;
            Timer.IsEndAttack += EntranceToNextLvlTrigger.Activate;
            Timer.IsEndAttack += EndLevelTrigger.Activate;
        }

        private void Update()
        {
            if (_enemySpawnTrigger.IsEnemySpawned)
            {
                CreateWaveOfEnemy();
            }
        }

        private void OnDestroy()
        {
            PauseService.OnGameStarted -= Timer.ResumeTimer;
            PauseService.OnGamePaused -= Timer.PauseTimer;
            
            _enemySpawnTrigger.EnemySpawned -= Timer.Show;
            _enemySpawnTrigger.EnemySpawned -= _firstLevelAdviserTextsSetter.SetAndShowEnemySpawnTriggerText;
            
            Timer.IsEndAttack -= _firstLevelAdviserTextsSetter.SetAndShowEndAttackText;
            Timer.IsEndAttack -= _enemySpawnTrigger.CompleteSpawn;
            Timer.IsEndAttack -= EntranceToNextLvlTrigger.Activate;
            Timer.IsEndAttack -= EndLevelTrigger.Activate;
        }
    }
}