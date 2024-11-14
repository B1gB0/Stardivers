using Project.Scripts.ECS.EntityActors;
using Project.Scripts.Projectiles.Enemy;
using UnityEngine;

namespace Project.Scripts.ECS.Data
{
    [CreateAssetMenu(menuName = "InitData/GunnerAlienEnemyData")]
    public class GunnerAlienEnemyInitData : InitData
    {
        [field: SerializeField] public GunnerAlienEnemy GunnerAlienEnemyPrefab { get; private set; }
        
        [field: SerializeField] public GunnerEnemyAlienProjectile ProjectilePrefab { get; private set; }
    }
}