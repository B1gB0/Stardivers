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
        private readonly Collider[] _colliderBuffer = new Collider[32];
        private readonly List<EnemyActor> _enemyBuffer = new(32); 
        
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
            ParticleEffectsService.PlayEffect(ParticleEffectType.MineExplosion, Transform.position);
            AudioSoundsService.PlaySound(SoundsType.Mines);

            foreach (EnemyActor explosiveObject in GetEnemies())
            {
                explosiveObject.Health.TakeDamage(Damage);
            }
        
            gameObject.SetActive(false);
        }
    
        protected List<EnemyActor> GetEnemies()
        {
            _enemyBuffer.Clear();
            
            int hitCount = Physics.OverlapSphereNonAlloc(Transform.position, ExplosionRadius, _colliderBuffer);
            
            for (int i = 0; i < hitCount; i++)
            {
                Collider hit = _colliderBuffer[i];
                
                if (hit.attachedRigidbody != null && 
                    hit.gameObject.TryGetComponent(out EnemyActor enemyActor))
                {
                    _enemyBuffer.Add(enemyActor);
                }
            }

            return _enemyBuffer;
            
            // Collider[] hits = Physics.OverlapSphere(Transform.position, ExplosionRadius);
            //
            // List<EnemyActor> enemies = new();
            //
            // foreach (Collider hit in hits)
            //     if (hit.attachedRigidbody != null && hit.gameObject.TryGetComponent(out EnemyActor enemyActor))
            //         enemies.Add(enemyActor);
            //
            // return enemies;
        }
    }
}