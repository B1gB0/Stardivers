using Project.Scripts.ECS.EntityActors;
using UnityEngine;

namespace Project.Scripts.Projectiles.Enemy
{
    public abstract class EnemyProjectile : Projectile
    {
        protected override void OnTriggerEnter(Collider collision)
        {
            if(collision.gameObject.TryGetComponent(out PlayerActor player))
            {
                player.Health.TakeDamage(Damage);
                gameObject.SetActive(false);
            }
        }
    }
}