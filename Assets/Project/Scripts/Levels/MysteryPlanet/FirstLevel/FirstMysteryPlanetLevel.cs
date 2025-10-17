using UnityEngine;

namespace Project.Scripts.Levels.MysteryPlanet.FirstLevel
{
    public class FirstMysteryPlanetLevel : Level
    {
        [SerializeField] private int _timeOfWaves = 90;

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
            
            WelcomePlanetTextTrigger.IsWelcomeToPlanet += DialogueSetter.OnWelcomePlanet;
            
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
            WelcomePlanetTextTrigger.IsWelcomeToPlanet -= DialogueSetter.OnWelcomePlanet;
            
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