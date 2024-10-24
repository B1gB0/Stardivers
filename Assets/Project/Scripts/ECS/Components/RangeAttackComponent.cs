using Project.Scripts.Projectiles.Bullets;

namespace Build.Game.Scripts.ECS.Components
{
    public struct RangeAttackComponent
    {
        public BigEnemyAlienProjectile Projectile;
        public float Damage;
        public float ProjectileSpeed;
    }
}