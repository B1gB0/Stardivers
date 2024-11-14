using Project.Scripts.Projectiles.Enemy;
using UnityEngine;

namespace Project.Scripts.Weapon.Enemy
{
    public class BigEnemyAlienWeapon : EnemyWeapon
    {
        [SerializeField] private Transform _shootPoint;
        [SerializeField] private float _damage;

        private Transform _target;
        private BigEnemyAlienProjectile _projectile;
        
        private ObjectPool<BigEnemyAlienProjectile> _projectilePool;

        public override void Shoot()
        {
            _projectile = _projectilePool.GetFreeElement();

            _projectile.transform.position = _shootPoint.position;
            
            _projectile.SetDamage(_damage);           
            _projectile.SetDirection(_target);
        }

        public void SetData(Transform target, ObjectPool<BigEnemyAlienProjectile> poolProjectile)
        {
            _target = target;
            _projectilePool = poolProjectile;
        }
    }
}