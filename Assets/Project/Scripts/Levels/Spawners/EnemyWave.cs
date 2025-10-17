using System.Collections.Generic;
using UnityEngine;

namespace Project.Scripts.Levels.Spawners
{
    public class EnemyWave
    {
        public List<Vector3> SmallEnemySpawnPositions { get; private set; }
        public List<Vector3> BigEnemySpawnPositions { get; private set; }
        public List<Vector3> GunnerEnemySpawnPositions { get; private set; }

        public void GetEnemyPositions(List<Vector3> smallEnemyPositions, List<Vector3> bigEnemyPositions,
            List<Vector3> gunnerEnemyPositions)
        {
            SmallEnemySpawnPositions = smallEnemyPositions;
            BigEnemySpawnPositions = bigEnemyPositions;
            GunnerEnemySpawnPositions = gunnerEnemyPositions;
        }
    }
}