using System.Collections.Generic;
using Project.Scripts.Audio.Sounds;
using Project.Scripts.ECS.EntityActors;
using Project.Scripts.ParticleEffects.Effects;
using Project.Scripts.Services;
using UnityEngine;

namespace Project.Scripts.Projectiles
{
    public abstract class ExplodingObject : Projectile
    {
        protected float ExplosionRadius;
        
        protected AudioSoundsService AudioSoundsService;
        protected ParticleEffectsService ParticleEffectsService;

        public void GetExplosionEffects(ParticleEffectsService particleEffectsService, 
            AudioSoundsService audioSoundsService)
        {
            ParticleEffectsService = particleEffectsService;
            AudioSoundsService = audioSoundsService;
        }
        
        protected virtual void Explode()
        {
            AudioSoundsService.PlaySound(SoundsType.Mines);

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