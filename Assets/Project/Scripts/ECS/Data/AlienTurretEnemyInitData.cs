using Project.Scripts.ECS.EntityActors;
using Project.Scripts.Projectiles.Enemy;
using UnityEngine;

namespace Project.Scripts.ECS.Data
{
    [CreateAssetMenu(menuName = "InitData/AlienTurretEnemyInitData")]
    public class AlienTurretEnemyInitData : InitData
    {
        [field: SerializeField] public EnemyTurret AlienTurretEnemyPrefab { get; private set; }
        [field: SerializeField] public AlienEnemyTurretProjectile ProjectilePrefab { get; private set; }
    }
}