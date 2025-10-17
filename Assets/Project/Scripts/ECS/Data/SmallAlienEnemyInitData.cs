using Project.Scripts.ECS.EntityActors;
using UnityEngine;

namespace Project.Scripts.ECS.Data
{
    [CreateAssetMenu(menuName = "InitData/SmallAlienEnemyData")]
    public class SmallAlienEnemyInitData : InitData
    {
        [field: SerializeField] public SmallEnemy SmallEnemyPrefab { get; private set; }
    }
}