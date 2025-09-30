using System.Collections.Generic;
using Project.Scripts.ECS.Data;
using Project.Scripts.ECS.EntityActors;
using Project.Scripts.ECS.System;
using UnityEngine;

namespace Project.Scripts.Levels.Spawners
{
    public class EnemySpawner
    {
        private const int MinValue = 0;
        private const float RandomPositionFactor = 2f;
        
        private readonly GameInitSystem _gameInitSystem;
        private readonly LevelInitData _levelInitData;
        
        public EnemySpawner(GameInitSystem gameInitSystem, LevelInitData levelInitData)
        {
            _gameInitSystem = gameInitSystem;
            _levelInitData = levelInitData;
        }
        
        public void SpawnGunnerAlienEnemy(List<Vector3> spawnPointPositions)
        {
            if(spawnPointPositions.Count == MinValue)
                return;
            
            foreach (var enemyPosition in spawnPointPositions)
            {
                GunnerAlienEnemy gunnerEnemy = _gameInitSystem.CreateGunnerAlienEnemy(_gameInitSystem.Player);

                gunnerEnemy.NavMeshAgent.enabled = false;

                var enemySpawnPosition = enemyPosition + Vector3.one * Random.Range(-RandomPositionFactor,
                    RandomPositionFactor);
                enemySpawnPosition.y = enemyPosition.y;

                gunnerEnemy.transform.position = enemySpawnPosition;
                
                gunnerEnemy.NavMeshAgent.enabled = true;
            }
        }

        public void SpawnSmallAlienEnemy(List<Vector3> spawnPointPositions)
        {
            if(spawnPointPositions.Count == MinValue)
                return;
            
            foreach (var enemyPosition in spawnPointPositions)
            {
                SmallAlienEnemy smallAlienEnemy = _gameInitSystem.CreateSmallAlienEnemy(_gameInitSystem.Player);

                smallAlienEnemy.NavMeshAgent.enabled = false;

                var enemySpawnPosition = enemyPosition + Vector3.one * Random.Range(-RandomPositionFactor,
                    RandomPositionFactor);
                enemySpawnPosition.y = enemyPosition.y;

                smallAlienEnemy.transform.position = enemySpawnPosition;

                smallAlienEnemy.NavMeshAgent.enabled = true;
            }
        }

        public void SpawnBigEnemyAlien(List<Vector3> spawnPointPositions)
        {
            if(spawnPointPositions.Count == MinValue)
                return;
            
            foreach (var enemyPosition in spawnPointPositions)
            {
                BigAlienEnemy bigEnemy = _gameInitSystem.CreateBigAlienEnemy(_gameInitSystem.Player);

                bigEnemy.NavMeshAgent.enabled = false;

                var enemySpawnPosition = enemyPosition + Vector3.one * Random.Range(-RandomPositionFactor,
                    RandomPositionFactor);
                enemySpawnPosition.y = enemyPosition.y;

                bigEnemy.transform.position = enemySpawnPosition;
                
                bigEnemy.NavMeshAgent.enabled = true;
            }
        }
    }
}