using Build.Game.Scripts.ECS.EntityActors;
using UnityEngine;

namespace Project.Scripts.Projectiles.Bullets
{
    public class FourBarrelMachineGunBullet : Projectile
    {
        private Vector3 _direction;
    
        private float _bulletSpeed;
        private float _damage;
    
        private void OnTriggerEnter(Collider collision)
        {
            if(collision.gameObject.TryGetComponent(out EnemyActor enemy))
            {
                enemy.Health.TakeDamage(_damage);
                gameObject.SetActive(false);
            }
        }
    
        private void FixedUpdate()
        {
            transform.position += _direction * (_bulletSpeed * Time.fixedDeltaTime);
        }

        public void SetDirection(Vector3 direction)
        {
            _direction = direction;
            transform.forward = direction;
        }

        public void SetCharacteristics(float damage, float bulletSpeed)
        {
            _damage = damage;
            _bulletSpeed = bulletSpeed;
        }
    }
}