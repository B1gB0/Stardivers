using Project.Scripts.ECS.EntityActors;
using Project.Scripts.Projectiles.Enemy;
using UnityEngine;

namespace Project.Scripts.ECS.Data
{
    [CreateAssetMenu(menuName = "InitData/GunnerAlienEnemyData")]
    public class GunnerAlienEnemyInitData : InitData
    {
        [field: SerializeField] public GunnerEnemy GunnerEnemyPrefab { get; private set; }
        [field: SerializeField] public GunnerAlienEnemyProjectile ProjectilePrefab { get; private set; }
    }
}