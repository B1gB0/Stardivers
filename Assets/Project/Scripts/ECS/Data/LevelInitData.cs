using System.Collections.Generic;
using Project.Scripts.Levels;
using UnityEngine;
using UnityEngine.Rendering;

namespace Project.Scripts.ECS.Data
{
    [CreateAssetMenu(menuName = "InitData/LevelData")]
    public class LevelInitData : InitData
    {
        // [field: SerializeField] public Level LevelPrefab { get; private set; }

        public List<Vector3> SmallEnemyAlienSpawnPoints;
        public List<Vector3> BigEnemyAlienSpawnPoints;
        public List<Vector3> GunnerEnemyAlienSpawnPoints;
        public List<Vector3> StoneSpawnPoints;
        public List<Vector3> GoldCoreSpawnPoints;
        public List<Vector3> HealingCoreSpawnPoints;
        public Vector3 PlayerSpawnPoint;
    }
}
