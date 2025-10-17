using Project.Scripts.ECS.EntityActors;
using UnityEngine;

namespace Project.Scripts.Projectiles.Enemy
{
    public class GunnerAlienEnemyProjectile : Projectile
    {
        private const float DefaultBulletSpeed = 5f;
        private const float DefaultDirectionY = 0f;
        
        private void Start()
        {
            ProjectileSpeed = DefaultBulletSpeed;
        }
        
        protected override void OnTriggerEnter(Collider collision)
        {
            if(collision.gameObject.TryGetComponent(out PlayerActor player))
            {
                player.Health.TakeDamage(Damage);
                gameObject.SetActive(false);
            }
        }

        public override void SetDirection(Vector3 targetPosition)
        {
            Direction = (targetPosition - Transform.position).normalized;
            Transform.LookAt(targetPosition);
            Direction.y = DefaultDirectionY;
            Direction = Direction.normalized;
        }

        public void SetDamage(float damage)
        {
            Damage = damage;
        }
    }
}