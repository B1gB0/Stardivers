using Project.Scripts.Levels.Triggers;
using UnityEngine;

namespace Project.Scripts.Levels.MysteryPlanet.FirstLevel
{
    public class FirstMysteryPlanetLevel : Level
    {
        [SerializeField] private EnemySpawnFirstWaveTrigger enemySpawnFirstWaveTrigger;
        [SerializeField] private EntranceTrigger _entranceLastLvlTrigger;
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
            
        }

        private void FixedUpdate()
        {
            if (enemySpawnFirstWaveTrigger.IsEnemySpawned)
            {
                CreateWaveOfEnemy(FirstWaveEnemy);
            }
        }
    }
}