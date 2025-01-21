using Project.Scripts.ECS.EntityActors;
using UnityEngine;

namespace Project.Scripts.ECS.Data
{
    [CreateAssetMenu(menuName = "InitData/SmallAlienEnemyData")]
    public class SmallAlienEnemyInitData : InitData
    {
        [field: SerializeField] public SmallAlienEnemy SmallAlienEnemyPrefab { get; private set; }

        [field: SerializeField] public ParticleSystem HitEffect { get; private set; }

        [field: SerializeField] public float DefaultDamage { get; private set; } = 1f;
    }
}