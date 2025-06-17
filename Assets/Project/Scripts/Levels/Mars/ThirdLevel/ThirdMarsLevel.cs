using System;
using Project.Scripts.Levels.Triggers;
using UnityEngine;

namespace Project.Scripts.Levels.Mars.ThirdLevel
{
    public class ThirdMarsLevel : Level
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

        private void Start()
        {
            // SpawnPlayer();
        }

        public override void OnStartLevel()
        {
            base.OnStartLevel();
            
            
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