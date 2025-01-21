using System.Collections.Generic;
using Project.Scripts.ECS.System;
using UnityEngine;

namespace Project.Scripts.Levels.Spawners
{
    public class ResourcesSpawner
    {
        private const float RandomPositionFactor = 2f;
        private const int MinValue = 0;

        private readonly GameInitSystem _gameInitSystem;

        public ResourcesSpawner(GameInitSystem gameInitSystem)
        {
            _gameInitSystem = gameInitSystem;
        }
        
        public void Spawn(int quantityGoldCore, int quantityHealingCore)
        {
            List<Vector3> sortedSpawnPoints = new List<Vector3>();

            foreach (var stoneSpawnPoint in _gameInitSystem.StoneSpawnPoints)
            {
                var stoneSpawnPosition = stoneSpawnPoint + 
                                         Vector3.one * Random.Range(-RandomPositionFactor, RandomPositionFactor);
                stoneSpawnPosition.y = stoneSpawnPoint.y;

                _gameInitSystem.CreateStone(stoneSpawnPosition);   
            }

            sortedSpawnPoints = GetSortedRandomSpawnPoints(_gameInitSystem.GoldCoreSpawnPoints, quantityGoldCore);

            foreach (var goldCoreSpawnPoint in sortedSpawnPoints)
            {
                var goldCoreSpawnPosition = goldCoreSpawnPoint + 
                                            Vector3.one * Random.Range(-RandomPositionFactor, RandomPositionFactor);
                goldCoreSpawnPosition.y = goldCoreSpawnPoint.y;

                _gameInitSystem.CreateGoldCore(goldCoreSpawnPosition);   
            }
            
            sortedSpawnPoints = GetSortedRandomSpawnPoints(_gameInitSystem.HealingCoreSpawnPoints, quantityHealingCore);
            
            foreach (var healingCoreSpawnPoint in sortedSpawnPoints)
            {
                var healingCoreSpawnPosition = healingCoreSpawnPoint + 
                                               Vector3.one * Random.Range(-RandomPositionFactor, RandomPositionFactor);
                healingCoreSpawnPosition.y = healingCoreSpawnPoint.y;

                _gameInitSystem.CreateHealingCore(healingCoreSpawnPosition);   
            }
        }

        private List<Vector3> GetSortedRandomSpawnPoints(List<Vector3> spawnPointsData, int quantityPoints)
        {
            List<Vector3> freeSpawnPoints = spawnPointsData;
            List<Vector3> sortedSpawnPoints = new List<Vector3>();

            int counterSpawnPoints = freeSpawnPoints.Count - 1;

            for (int i = 0; i < quantityPoints; i++)
            {
                Vector3 randomPoint = freeSpawnPoints[Random.Range(MinValue, counterSpawnPoints)];
                sortedSpawnPoints.Add(randomPoint);
                freeSpawnPoints.Remove(randomPoint);
                counterSpawnPoints--;
            }

            return sortedSpawnPoints;
        }
    }
}