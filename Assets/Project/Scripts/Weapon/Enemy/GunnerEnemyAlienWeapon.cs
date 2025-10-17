using Project.Scripts.Projectiles.Enemy;
using UnityEngine;

namespace Project.Scripts.Weapon.Enemy
{
    public class GunnerEnemyAlienWeapon : EnemyWeapon
    {
        [SerializeField] private Transform _shootPoint;

        private Transform _target;
        private GunnerAlienEnemyProjectile _projectile;
        private float _damage;
        
        private ObjectPool<GunnerAlienEnemyProjectile> _projectilePool;
        
        public override void Shoot()
        {
            _projectile = _projectilePool.GetFreeElement();

            _projectile.transform.position = _shootPoint.position;
            
            _projectile.SetDamage(_damage);           
            _projectile.SetDirection(_target.position);
        }

        public void SetData(Transform target, ObjectPool<GunnerAlienEnemyProjectile> projectilePool, float damage)
        {
            _target = target;
            _projectilePool = projectilePool;
            _damage = damage;
        }
    }
}