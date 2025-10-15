using UnityEngine;

namespace Project.Scripts.Levels.Mars.FirstLevel
{
    public class FirstMarsLevel : Level
    {
        [SerializeField] private int _timeOfWaves = 90;

        private void OnEnable()
        {
            WelcomePlanetTextTrigger.IsWelcomeToPlanet += DialogueSetter.OnWelcomePlanet;
            
            IsInitiatedSpawners += SpawnResources;
        }

        private void OnDisable()
        {
            WelcomePlanetTextTrigger.IsWelcomeToPlanet -= DialogueSetter.OnWelcomePlanet;
            
            IsInitiatedSpawners -= SpawnResources;
        }

        public override void OnStartLevel()
        {
            base.OnStartLevel();
            
            Timer.SetTime(_timeOfWaves);

            PauseService.OnGameStarted += Timer.ResumeTimer;
            PauseService.OnGamePaused += Timer.PauseTimer;

            EnemySpawnFirstWaveTrigger.EnemySpawned += Timer.Show;
            EnemySpawnFirstWaveTrigger.EnemySpawned += DialogueSetter.OnEnemySpawnTrigger;
            
            Timer.IsEndAttack += DialogueSetter.OnEndAttack;
            Timer.IsEndAttack += EnemySpawnFirstWaveTrigger.CompleteSpawn;
            Timer.IsEndAttack += EntranceToNextLvlTrigger.Activate;
            Timer.IsEndAttack += EndLevelTrigger.Activate;
        }

        private void FixedUpdate()
        {
            if (EnemySpawnFirstWaveTrigger.IsEnemySpawned)
            {
                CreateWaveOfEnemy(FirstWaveEnemy);
            }
        }

        private void OnDestroy()
        {
            PauseService.OnGameStarted -= Timer.ResumeTimer;
            PauseService.OnGamePaused -= Timer.PauseTimer;
            
            EnemySpawnFirstWaveTrigger.EnemySpawned -= Timer.Show;
            EnemySpawnFirstWaveTrigger.EnemySpawned -= DialogueSetter.OnEnemySpawnTrigger;
            
            Timer.IsEndAttack -= DialogueSetter.OnEndAttack;
            Timer.IsEndAttack -= EnemySpawnFirstWaveTrigger.CompleteSpawn;
            Timer.IsEndAttack -= EntranceToNextLvlTrigger.Activate;
            Timer.IsEndAttack -= EndLevelTrigger.Activate;
        }
    }
}