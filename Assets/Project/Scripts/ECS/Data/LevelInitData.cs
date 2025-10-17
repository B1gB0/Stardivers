using System.Collections.Generic;
using UnityEngine;

namespace Project.Scripts.ECS.Data
{
    [CreateAssetMenu(menuName = "InitData/LevelData")]
    public class LevelInitData : InitData
    {
        public List<Vector3> EnemyPatrolPositions; 

        public List<Vector3> FirstWaveSmallEnemyAlienSpawnPositions;
        public List<Vector3> FirstWaveBigEnemyAlienSpawnPositions;
        public List<Vector3> FirstWaveGunnerEnemyAlienSpawnPositions;
        
        public List<Vector3> SecondWaveSmallEnemyAlienSpawnPositions;
        public List<Vector3> SecondWaveBigEnemyAlienSpawnPositions;
        public List<Vector3> SecondWaveGunnerEnemyAlienSpawnPositions;
        
        public List<Vector3> StoneSpawnPositions;
        public List<Vector3> GoldCoreSpawnPositions;
        public List<Vector3> HealingCoreSpawnPositions;
        
        public Vector3 PlayerSpawnPosition;
    }
}
