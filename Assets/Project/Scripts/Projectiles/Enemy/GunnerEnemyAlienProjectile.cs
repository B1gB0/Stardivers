using Project.Scripts.ECS.EntityActors;
using UnityEngine;

namespace Project.Scripts.Projectiles.Enemy
{
    public class GunnerEnemyAlienProjectile : Projectile
    {
        private const float BulletSpeed = 5f;
        
        private Vector3 _direction;
        private float _damage;
    
        private void OnTriggerEnter(Collider collision)
        {
            if(collision.gameObject.TryGetComponent(out PlayerActor player))
            {
                player.Health.TakeDamage(_damage);
                gameObject.SetActive(false);
            }
        }
        
        private void FixedUpdate()
        {
            transform.position += _direction * (BulletSpeed * Time.fixedDeltaTime);
        }

        public void SetDirection(Transform player)
        {
            _direction = (player.position - transform.position).normalized;
            transform.LookAt(player);
        }

        public void SetDamage(float damage)
        {
            _damage = damage;
        }
    }
}