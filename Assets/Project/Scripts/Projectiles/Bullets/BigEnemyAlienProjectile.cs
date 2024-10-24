using Build.Game.Scripts.ECS.EntityActors;
using UnityEngine;

namespace Project.Scripts.Projectiles.Bullets
{
    public class BigEnemyAlienProjectile : Projectile
    {
        private Vector3 _direction;
    
        private float _projectileSpeed;
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
            transform.position += _direction * (_projectileSpeed * Time.fixedDeltaTime);
        }

        public void SetDirection(Transform enemy)
        {
            _direction = (enemy.position - transform.position).normalized;
            transform.LookAt(enemy);
        }
    
        public void SetCharacteristics(float damage, float projectileSpeed)
        {
            _damage = damage;
            _projectileSpeed = projectileSpeed;
        }
    }
}