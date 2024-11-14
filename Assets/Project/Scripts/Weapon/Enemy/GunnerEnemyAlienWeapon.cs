using Project.Scripts.Projectiles.Enemy;
using UnityEngine;

namespace Project.Scripts.Weapon.Enemy
{
    public class GunnerEnemyAlienWeapon : EnemyWeapon
    {
        [SerializeField] private Transform _shootPoint;
        [SerializeField] private float _damage;

        private Transform _target;
        private GunnerEnemyAlienProjectile _projectile;
        
        private ObjectPool<GunnerEnemyAlienProjectile> _projectilePool;
        
        public override void Shoot()
        {
            _projectile = _projectilePool.GetFreeElement();

            _projectile.transform.position = _shootPoint.position;
            
            _projectile.SetDamage(_damage);           
            _projectile.SetDirection(_target);
        }

        public void SetData(Transform target, ObjectPool<GunnerEnemyAlienProjectile> projectilePool)
        {
            _target = target;
            _projectilePool = projectilePool;
        }
    }
}