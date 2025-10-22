using Project.Scripts.Levels.Triggers;
using UnityEngine;

namespace Project.Scripts.Levels.MysteryPlanet.FirstLevel
{
    public class FirstMysteryPlanetLevel : Level
    {
        [SerializeField] private EnemySpawnTriggerWithEffect _enemySpawnTriggerWithEffect;
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
            
            _enemySpawnTriggerWithEffect.EnemySpawned += Timer.Show;
            _enemySpawnTriggerWithEffect.EnemySpawned += DialogueSetter.OnEnemySpawnTriggerWithEffect;
            
            Timer.IsEndAttack += DialogueSetter.OnEndAttack;
            Timer.IsEndAttack += _enemySpawnTriggerWithEffect.CompleteSpawn;
            Timer.IsEndAttack += EntranceToNextLvlTrigger.Activate;
            Timer.IsEndAttack += EndLevelTrigger.Activate;
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
            
            PauseService.OnGameStarted -= Timer.ResumeTimer;
            PauseService.OnGamePaused -= Timer.PauseTimer;
            
            _enemySpawnTriggerWithEffect.EnemySpawned -= Timer.Show;
            _enemySpawnTriggerWithEffect.EnemySpawned -= DialogueSetter.OnEnemySpawnTriggerWithEffect;
            
            Timer.IsEndAttack -= DialogueSetter.OnEndAttack;
            Timer.IsEndAttack -= _enemySpawnTriggerWithEffect.CompleteSpawn;
            Timer.IsEndAttack -= EntranceToNextLvlTrigger.Activate;
            Timer.IsEndAttack -= EndLevelTrigger.Activate;
        }
    }
}