using Project.Scripts.Levels.Triggers;
using UnityEngine;

namespace Project.Scripts.Levels.MysteryPlanet.SecondLevel
{
    public class SecondMysteryPlanetLevel : Level
    {
        [SerializeField] private EnemySpawnTrigger _enemySpawnTrigger;
        [SerializeField] private EntranceTrigger _entranceLastLvlTrigger;

        private void OnEnable()
        {
            IsInitiatedSpawners += SpawnResources;
        }

        private void OnDisable()
        {
            IsInitiatedSpawners -= SpawnResources;
        }

        private void Update()
        {
            if (_enemySpawnTrigger.IsEnemySpawned)
            {
                CreateWaveOfEnemy();
            }
        }
    }
}