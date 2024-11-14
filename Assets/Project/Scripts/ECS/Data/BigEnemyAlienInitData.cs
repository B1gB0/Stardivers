using Project.Scripts.ECS.EntityActors;
using Project.Scripts.Projectiles.Enemy;
using UnityEngine;

namespace Project.Scripts.ECS.Data
{
    [CreateAssetMenu(menuName = "InitData/BigAlienEnemyData")]
    public class BigEnemyAlienInitData : InitData
    {
        [field: SerializeField] public BigAlienEnemy BigAlienEnemyPrefab { get; private set; }
        
        [field: SerializeField] public BigEnemyAlienProjectile ProjectilePrefab { get; private set; }
    }
}