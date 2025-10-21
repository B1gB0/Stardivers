using Project.Scripts.Projectiles.Enemy;

namespace Project.Scripts.Weapon.Enemy
{
    public class BigEnemyAlienWeapon : GenericEnemyWeapon<BigAlienEnemyProjectile>
    {
        // private const float DefaultBulletSpeed = 5f;
        //
        // [SerializeField] private Transform _shootPoint;
        //
        // private float _damage;
        // private Transform _target;
        // private BigAlienEnemyProjectile _projectile;
        //
        // private ObjectPool<BigAlienEnemyProjectile> _projectilePool;
        //
        // public override void Shoot()
        // {
        //     _projectile = _projectilePool.GetFreeElement();
        //
        //     _projectile.transform.position = _shootPoint.position;
        //     
        //     _projectile.SetCharacteristics(_damage, DefaultBulletSpeed);           
        //     _projectile.SetDirection(_target.position);
        // }
        //
        // public void SetData(Transform target, ObjectPool<BigAlienEnemyProjectile> poolProjectile, float damage)
        // {
        //     _target = target;
        //     _projectilePool = poolProjectile;
        //     _damage = damage;
        // }
    }
}