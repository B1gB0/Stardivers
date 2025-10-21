using Project.Scripts.Projectiles;
using UnityEngine;

namespace Project.Scripts.Weapon.Enemy
{
    public class GenericEnemyWeapon<T> : EnemyWeapon where T : Projectile
    {
        private const float DefaultBulletSpeed = 5f;
        
        [SerializeField] private Transform _shootPoint;

        private Transform _target;
        private T _projectile;
        private float _damage;
        
        private ObjectPool<T> _projectilePool;

        public override void Shoot()
        {
            _projectile = _projectilePool.GetFreeElement();
            _projectile.transform.position = _shootPoint.position;
            _projectile.SetCharacteristics(_damage, DefaultBulletSpeed);           
            _projectile.SetDirection(_target.position);
        }

        public virtual void SetData(Transform target, ObjectPool<T> projectilePool, float damage)
        {
            _target = target;
            _projectilePool = projectilePool;
            _damage = damage;
        }
    }
}