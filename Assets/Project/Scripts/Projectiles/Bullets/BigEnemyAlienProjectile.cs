using System.Collections;
using Build.Game.Scripts.ECS.EntityActors;
using UnityEngine;

namespace Project.Scripts.Projectiles.Bullets
{
    public class BigEnemyAlienProjectile : Projectile
    {
        private Coroutine _coroutine;
        private float _damage;
    
        private void OnTriggerEnter(Collider collision)
        {
            if(collision.gameObject.TryGetComponent(out PlayerActor player))
            {
                player.Health.TakeDamage(_damage);
            }
        }

        public void SetCharacteristics(float damage)
        {
            _damage = damage;
        }
    }
}