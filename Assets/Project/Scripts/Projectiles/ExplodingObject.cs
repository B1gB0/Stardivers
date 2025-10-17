using System.Collections.Generic;
using Project.Game.Scripts;
using Project.Scripts.ECS.EntityActors;
using Project.Scripts.Services;
using UnityEngine;

namespace Project.Scripts.Projectiles
{
    public abstract class ExplodingObject : Projectile
    {
        protected float ExplosionRadius;
        
        protected ParticleSystem ExplosionEffect;
        protected AudioSoundsService AudioSoundsService;

        public void GetExplosionEffects(ParticleSystem effect, AudioSoundsService audioSoundsService)
        {
            ExplosionEffect = effect;
            AudioSoundsService = audioSoundsService;
        }
        
        protected virtual void Explode()
        {
            ExplosionEffect.transform.position = Transform.position;
            ExplosionEffect.Play();
            AudioSoundsService.PlaySound(Sounds.Mines);

            foreach (EnemyActor explosiveObject in GetEnemies())
            {
                explosiveObject.Health.TakeDamage(Damage);
            }
        
            gameObject.SetActive(false);
        }
    
        protected List<EnemyActor> GetEnemies()
        {
            Collider[] hits = Physics.OverlapSphere(Transform.position, ExplosionRadius);

            List<EnemyActor> enemies = new();

            foreach (Collider hit in hits)
                if (hit.attachedRigidbody != null && hit.gameObject.TryGetComponent(out EnemyActor enemyActor))
                    enemies.Add(enemyActor);

            return enemies;
        }
    }
}