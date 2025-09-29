using System.Collections.Generic;
using UnityEngine;

namespace Project.Scripts.ECS.Data
{
    [CreateAssetMenu(menuName = "InitData/LevelData")]
    public class LevelInitData : InitData
    {
        public List<Vector3> SmallEnemyAlienSpawnPoints;
        public List<Vector3> BigEnemyAlienSpawnPoints;
        public List<Vector3> GunnerEnemyAlienSpawnPoints;
        public List<Vector3> StoneSpawnPoints;
        public List<Vector3> GoldCoreSpawnPoints;
        public List<Vector3> HealingCoreSpawnPoints;
        public Vector3 PlayerSpawnPoint;
    }
}
