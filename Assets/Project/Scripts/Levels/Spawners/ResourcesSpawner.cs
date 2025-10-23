using System.Collections.Generic;
using Project.Scripts.ECS.Data;
using Project.Scripts.ECS.System;
using UnityEngine;

namespace Project.Scripts.Levels.Spawners
{
    public class ResourcesSpawner
    {
        private const float RandomPositionFactor = 2f;
        private const int MinValue = 0;

        private readonly GameInitSystem _gameInitSystem;
        private readonly LevelInitData _levelInitData;

        public ResourcesSpawner(GameInitSystem gameInitSystem, LevelInitData levelInitData)
        {
            _gameInitSystem = gameInitSystem;
            _levelInitData = levelInitData;
        }
        
        public void Spawn(int quantityGoldCore, int quantityHealingCore)
        {
            SpawnStones();
            SpawnGoldCores(quantityGoldCore);
            SpawnHealingCores(quantityHealingCore);
            SpawnAlienCocoons();
        }

        private void SpawnAlienCocoons()
        {
            if(_levelInitData.AlienCocoonSpawnPoints.Count == MinValue)
                return;
            
            foreach (var alienCocoonSpawnPoint in _levelInitData.AlienCocoonSpawnPoints)
            {
                var alienCocoonSpawnPosition = alienCocoonSpawnPoint + Vector3.one;
                alienCocoonSpawnPosition.y = alienCocoonSpawnPoint.y;

                _gameInitSystem.CreateAlienCocoon(alienCocoonSpawnPosition);
            }
        }

        private void SpawnHealingCores(int quantityHealingCore)
        {
            var sortedSpawnPoints = 
                GetSortedRandomSpawnPoints(_levelInitData.HealingCoreSpawnPositions, quantityHealingCore);

            foreach (var healingCoreSpawnPoint in sortedSpawnPoints)
            {
                var healingCoreSpawnPosition = healingCoreSpawnPoint +
                                               Vector3.one * Random.Range(-RandomPositionFactor, RandomPositionFactor);
                healingCoreSpawnPosition.y = healingCoreSpawnPoint.y;

                _gameInitSystem.CreateHealingCore(healingCoreSpawnPosition);
            }
        }

        private void SpawnGoldCores(int quantityGoldCore)
        {
            var sortedSpawnPoints = 
                GetSortedRandomSpawnPoints(_levelInitData.GoldCoreSpawnPositions, quantityGoldCore);

            foreach (var goldCoreSpawnPoint in sortedSpawnPoints)
            {
                var goldCoreSpawnPosition = goldCoreSpawnPoint +
                                            Vector3.one * Random.Range(-RandomPositionFactor, RandomPositionFactor);
                goldCoreSpawnPosition.y = goldCoreSpawnPoint.y;

                _gameInitSystem.CreateGoldCore(goldCoreSpawnPosition);
            }
        }

        private void SpawnStones()
        {
            foreach (var stoneSpawnPoint in _levelInitData.StoneSpawnPositions)
            {
                var stoneSpawnPosition = stoneSpawnPoint +
                                         Vector3.one * Random.Range(-RandomPositionFactor, RandomPositionFactor);
                stoneSpawnPosition.y = stoneSpawnPoint.y;

                _gameInitSystem.CreateStone(stoneSpawnPosition);
            }
        }

        private List<Vector3> GetSortedRandomSpawnPoints(List<Vector3> spawnPointsData, int quantityPoints)
        {
            var sortedSpawnPoints = new List<Vector3>();

            int counterSpawnPoints = spawnPointsData.Count - 1;

            for (int i = 0; i < quantityPoints; i++)
            {
                var randomPoint = spawnPointsData[Random.Range(MinValue, counterSpawnPoints)];
                sortedSpawnPoints.Add(randomPoint);
                spawnPointsData.Remove(randomPoint);
                counterSpawnPoints--;
            }

            return sortedSpawnPoints;
        }
    }
}