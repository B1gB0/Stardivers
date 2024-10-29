using Project.Scripts.Projectiles.Bullets;
using UnityEngine;

namespace Build.Game.Scripts.ECS.Components
{
    public struct RangeAttackComponent
    {
        public BigEnemyAlienProjectile Projectile;
        public float Damage;
    }
}