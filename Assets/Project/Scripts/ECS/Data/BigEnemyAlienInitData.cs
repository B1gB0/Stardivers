using Build.Game.Scripts.ECS.EntityActors;
using Project.Scripts.Projectiles.Bullets;
using UnityEngine;

namespace Project.Scripts.ECS.Data
{
    [CreateAssetMenu(menuName = "InitData/BigAlienEnemyData")]
    public class BigEnemyAlienInitData : InitData
    {
        [field: SerializeField] public BigAlienEnemyAlienActor BigAlienEnemyPrefab { get; private set; }
        
        [field: SerializeField] public BigEnemyAlienProjectile BigAlienEnemyAlienProjectilePrefab { get; private set; }

        [field: SerializeField] public float DefaultDamage { get; private set; } = 4f;
        
        [field: SerializeField] public float ProjectileSpeed { get; private set; } = 4f;
    }
}