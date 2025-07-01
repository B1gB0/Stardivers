using Project.Scripts.ECS.EntityActors;
using UnityEngine;

namespace Project.Scripts.Projectiles.Enemy
{
    public class BigAlienEnemyProjectile : Projectile
    {
        private const float DefaultBulletSpeed = 5f;
        
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

        public override void SetDirection(Transform player)
        {
            Direction = (player.position - Transform.position).normalized;
            Transform.LookAt(player);
        }

        public void SetDamage(float damage)
        {
            Damage = damage;
        }
    }
}