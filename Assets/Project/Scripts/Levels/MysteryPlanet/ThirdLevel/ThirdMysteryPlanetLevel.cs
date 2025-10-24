using Project.Scripts.Levels.Triggers;
using UnityEngine;

namespace Project.Scripts.Levels.MysteryPlanet.ThirdLevel
{
    public class ThirdMysteryPlanetLevel : Level
    {
        [SerializeField] private EnemySpawnTriggerWithoutEffect _enemySpawnTriggerWithoutEffect;
        [SerializeField] private EntranceTrigger _entranceLastLvlTrigger;

        private void OnEnable()
        {
            IsInitiatedSpawners += SpawnResources;
            IsInitiatedSpawners += SpawnAlienCocoons;
        }

        private void OnDisable()
        {
            IsInitiatedSpawners -= SpawnResources;
            IsInitiatedSpawners -= SpawnAlienCocoons;
        }

        public override void OnStartLevel()
        {
            base.OnStartLevel();

            WelcomePlanetTextTrigger.IsWelcomeToPlanet += DialogueSetter.OnWelcomePlanet;
            
            _enemySpawnTriggerWithoutEffect.EnemySpawned += _entranceLastLvlTrigger.Deactivate;
            _enemySpawnTriggerWithoutEffect.EnemySpawned += CreateAllAlienEnemyTurrets;
            _enemySpawnTriggerWithoutEffect.EnemySpawned += DialogueSetter.OnEnemySpawnTriggerWithEffect;
        }

        private void FixedUpdate()
        {
            if (_enemySpawnTriggerWithoutEffect.IsEnemySpawned)
            {
                CreateWaveOfEnemy(FirstWaveEnemy);
            }
        }

        private void OnDestroy()
        {
            WelcomePlanetTextTrigger.IsWelcomeToPlanet -= DialogueSetter.OnWelcomePlanet;
            
            _enemySpawnTriggerWithoutEffect.EnemySpawned -= _entranceLastLvlTrigger.Deactivate;
            _enemySpawnTriggerWithoutEffect.EnemySpawned -= CreateAllAlienEnemyTurrets;
            _enemySpawnTriggerWithoutEffect.EnemySpawned -= DialogueSetter.OnEnemySpawnTriggerWithEffect;
        }
    }
}