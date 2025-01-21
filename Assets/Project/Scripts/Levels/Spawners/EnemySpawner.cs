using Project.Scripts.ECS.EntityActors;
using Project.Scripts.ECS.System;
using UnityEngine;

namespace Project.Scripts.Levels.Spawners
{
    public class EnemySpawner
    {
        private const float RandomPositionFactor = 2f;
        
        private readonly GameInitSystem _gameInitSystem;
        
        public EnemySpawner(GameInitSystem gameInitSystem)
        {
            _gameInitSystem = gameInitSystem;
        }
        
        public void SpawnGunnerAlienEnemy()
        {
            if(_gameInitSystem.SmallEnemyAlienSpawnPoints.Count == 0)
                return;
            
            foreach (var enemySpawnPoint in _gameInitSystem.GunnerEnemyAlienSpawnPoints)
            {
                GunnerAlienEnemy gunnerEnemy = _gameInitSystem.CreateGunnerAlienEnemy(_gameInitSystem.Player);

                gunnerEnemy.NavMeshAgent.enabled = false;

                var enemySpawnPosition = enemySpawnPoint + Vector3.one * Random.Range(-RandomPositionFactor,
                    RandomPositionFactor);
                enemySpawnPosition.y = enemySpawnPoint.y;

                gunnerEnemy.transform.position = enemySpawnPosition;
                
                gunnerEnemy.NavMeshAgent.enabled = true;
            }
        }

        public void SpawnSmallAlienEnemy()
        {
            if(_gameInitSystem.SmallEnemyAlienSpawnPoints.Count == 0)
                return;
            
            foreach (var enemySpawnPoint in _gameInitSystem.SmallEnemyAlienSpawnPoints)
            {
                SmallAlienEnemy smallAlienEnemy = _gameInitSystem.CreateSmallAlienEnemy(_gameInitSystem.Player);

                smallAlienEnemy.NavMeshAgent.enabled = false;

                var enemySpawnPosition = enemySpawnPoint;
                enemySpawnPosition.y = enemySpawnPoint.y;

                smallAlienEnemy.transform.position = enemySpawnPosition;

                smallAlienEnemy.NavMeshAgent.enabled = true;
            }
        }

        public void SpawnBigEnemyAlien()
        {
            if(_gameInitSystem.BigEnemyAlienSpawnPoints.Count == 0)
                return;
            
            foreach (var enemySpawnPoint in _gameInitSystem.BigEnemyAlienSpawnPoints)
            {
                BigAlienEnemy bigEnemy = _gameInitSystem.CreateBigAlienEnemy(_gameInitSystem.Player);

                bigEnemy.NavMeshAgent.enabled = false;

                var enemySpawnPosition = enemySpawnPoint + Vector3.one * Random.Range(-RandomPositionFactor,
                    RandomPositionFactor);
                enemySpawnPosition.y = enemySpawnPoint.y;

                bigEnemy.transform.position = enemySpawnPosition;
                
                bigEnemy.NavMeshAgent.enabled = true;
            }
        }
    }
}