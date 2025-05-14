using Project.Scripts.ECS.EntityActors;
using Project.Scripts.Projectiles.Enemy;
using UnityEngine;

namespace Project.Scripts.ECS.Data
{
    [CreateAssetMenu(menuName = "InitData/BigAlienEnemyData")]
    public class BigAlienEnemyInitData : InitData
    {
        [field: SerializeField] public BigAlienEnemy BigAlienEnemyPrefab { get; private set; }
        [field: SerializeField] public ParticleSystem HitEffect { get; private set; }
        [field: SerializeField] public BigAlienEnemyProjectile ProjectilePrefab { get; private set; }
    }
}