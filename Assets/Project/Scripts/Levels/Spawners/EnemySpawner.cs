using System.Collections.Generic;
using Project.Scripts.ECS.EntityActors;
using Project.Scripts.ECS.System;
using UnityEngine;

namespace Project.Scripts.Levels.Spawners
{
    public class EnemySpawner
    {
        private const int MinValue = 0;
        private const float RandomPositionFactor = 2f;
        private const int CorrectCountFactor = 1;
        
        private readonly GameInitSystem _gameInitSystem;

        private int _counterSmallEnemies;
        private int _counterBigEnemies;
        private int _counterGunnerEnemies;

        public EnemySpawner(GameInitSystem gameInitSystem)
        {
            _gameInitSystem = gameInitSystem;
        }
        
        public void SpawnGunnerAlienEnemy(List<Vector3> spawnPointPositions, int countEnemies)
        {   
            if(spawnPointPositions.Count == MinValue)
                return;
            
            foreach (var enemyPosition in spawnPointPositions)
            {
                if(_counterGunnerEnemies > countEnemies - CorrectCountFactor)
                    return;
                
                GunnerEnemy gunnerEnemy = _gameInitSystem.CreateGunnerAlienEnemy(_gameInitSystem.Player);

                gunnerEnemy.NavMeshAgent.enabled = false;

                var enemySpawnPosition = enemyPosition + Vector3.one * Random.Range(-RandomPositionFactor,
                    RandomPositionFactor);
                enemySpawnPosition.y = enemyPosition.y;

                gunnerEnemy.transform.position = enemySpawnPosition;
                
                gunnerEnemy.NavMeshAgent.enabled = true;
                
                gunnerEnemy.Die += OnKillGunnerEnemy;
                _counterGunnerEnemies++;
            }
        }

        public void SpawnSmallAlienEnemy(List<Vector3> spawnPointPositions, int countEnemies)
        {
            if(spawnPointPositions.Count == MinValue)
                return;

            foreach (var enemyPosition in spawnPointPositions)
            {
                if(_counterSmallEnemies > countEnemies - CorrectCountFactor)
                    return;
                
                SmallEnemy smallEnemy = _gameInitSystem.CreateSmallAlienEnemy(_gameInitSystem.Player);

                smallEnemy.NavMeshAgent.enabled = false;

                var enemySpawnPosition = enemyPosition + Vector3.one * Random.Range(-RandomPositionFactor,
                    RandomPositionFactor);
                enemySpawnPosition.y = enemyPosition.y;

                smallEnemy.transform.position = enemySpawnPosition;

                smallEnemy.NavMeshAgent.enabled = true;

                smallEnemy.Die += OnKillSmallEnemy;
                _counterSmallEnemies++;
            }
        }

        public void SpawnBigEnemyAlien(List<Vector3> spawnPointPositions, int countEnemies)
        {
            if(spawnPointPositions.Count == MinValue)
                return;
            
            foreach (var enemyPosition in spawnPointPositions)
            {
                if(_counterBigEnemies > countEnemies - CorrectCountFactor)
                    return;
                
                BigEnemy bigEnemy = _gameInitSystem.CreateBigAlienEnemy(_gameInitSystem.Player);

                bigEnemy.NavMeshAgent.enabled = false;

                var enemySpawnPosition = enemyPosition + Vector3.one * Random.Range(-RandomPositionFactor,
                    RandomPositionFactor);
                enemySpawnPosition.y = enemyPosition.y;

                bigEnemy.transform.position = enemySpawnPosition;
                
                bigEnemy.NavMeshAgent.enabled = true;
                
                bigEnemy.Die += OnKillBigEnemy;
                _counterBigEnemies++;
            }
        }

        public void SpawnAlienEnemyTurret(List<Vector3> spawnPointPositions, Vector3 playerSpawnPoint)
        {
            if(spawnPointPositions.Count == MinValue)
                return;

            foreach (var enemyPosition in spawnPointPositions)
            {
                var direction = (playerSpawnPoint - enemyPosition)
                    .normalized;
                
                EnemyTurret enemyTurret = _gameInitSystem.CreateEnemyTurret(_gameInitSystem.Player, enemyPosition, 
                    direction);

                var enemySpawnPosition = enemyPosition + Vector3.one * Random.Range(-RandomPositionFactor,
                    RandomPositionFactor);
                enemySpawnPosition.y = enemyPosition.y;

                enemyTurret.transform.position = enemySpawnPosition;
            }
        }

        private void OnKillSmallEnemy(EnemyActor enemyActor)
        {
            _counterSmallEnemies--;
            enemyActor.Die -= OnKillSmallEnemy;
        }
        
        private void OnKillBigEnemy(EnemyActor enemyActor)
        {
            _counterBigEnemies--;
            enemyActor.Die -= OnKillBigEnemy;
        }
        
        private void OnKillGunnerEnemy(EnemyActor enemyActor)
        {
            _counterGunnerEnemies--;
            enemyActor.Die -= OnKillGunnerEnemy;
        }
    }
}