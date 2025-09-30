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
            Debug.Log(_counterGunnerEnemies + " GunnerEnemies");
            
            if(spawnPointPositions.Count == MinValue)
                return;
            
            foreach (var enemyPosition in spawnPointPositions)
            {
                if(_counterGunnerEnemies > countEnemies - CorrectCountFactor)
                    return;
                
                GunnerAlienEnemy gunnerEnemy = _gameInitSystem.CreateGunnerAlienEnemy(_gameInitSystem.Player);

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
            Debug.Log(_counterSmallEnemies + " SmallEnemies");
            
            if(spawnPointPositions.Count == MinValue)
                return;

            foreach (var enemyPosition in spawnPointPositions)
            {
                if(_counterSmallEnemies > countEnemies - CorrectCountFactor)
                    return;
                
                SmallAlienEnemy smallAlienEnemy = _gameInitSystem.CreateSmallAlienEnemy(_gameInitSystem.Player);

                smallAlienEnemy.NavMeshAgent.enabled = false;

                var enemySpawnPosition = enemyPosition + Vector3.one * Random.Range(-RandomPositionFactor,
                    RandomPositionFactor);
                enemySpawnPosition.y = enemyPosition.y;

                smallAlienEnemy.transform.position = enemySpawnPosition;

                smallAlienEnemy.NavMeshAgent.enabled = true;

                smallAlienEnemy.Die += OnKillSmallEnemy;
                _counterSmallEnemies++;
            }
        }

        public void SpawnBigEnemyAlien(List<Vector3> spawnPointPositions, int countEnemies)
        {
            Debug.Log(_counterBigEnemies + " BigEnemies");
            
            if(spawnPointPositions.Count == MinValue)
                return;
            
            foreach (var enemyPosition in spawnPointPositions)
            {
                if(_counterBigEnemies > countEnemies - CorrectCountFactor)
                    return;
                
                BigAlienEnemy bigEnemy = _gameInitSystem.CreateBigAlienEnemy(_gameInitSystem.Player);

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

        private void OnKillSmallEnemy(EnemyAlienActor enemyAlienActor)
        {
            _counterSmallEnemies--;
            enemyAlienActor.Die -= OnKillSmallEnemy;
        }
        
        private void OnKillBigEnemy(EnemyAlienActor enemyAlienActor)
        {
            _counterBigEnemies--;
            enemyAlienActor.Die -= OnKillBigEnemy;
        }
        
        private void OnKillGunnerEnemy(EnemyAlienActor enemyAlienActor)
        {
            _counterGunnerEnemies--;
            enemyAlienActor.Die -= OnKillGunnerEnemy;
        }
    }
}