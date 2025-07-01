using System.Collections.Generic;
using Project.Game.Scripts;
using Project.Scripts.ECS.EntityActors;
using Project.Scripts.Services;
using UnityEngine;

namespace Project.Scripts.Projectiles
{
    public class ExplodingProjectile : Projectile
    {
        protected float ExplosionRadius;
        
        private ParticleSystem _explosionEffect;
        private AudioSoundsService _audioSoundsService;

        public void GetExplosionEffects(ParticleSystem effect, AudioSoundsService audioSoundsService)
        {
            _explosionEffect = effect;
            _audioSoundsService = audioSoundsService;
        }
        
        protected virtual void Explode()
        {
            _explosionEffect.transform.position = Transform.position;
            _explosionEffect.Play();
            _audioSoundsService.PlaySound(Sounds.Mines);

            foreach (EnemyAlienActor explosiveObject in GetExplosiveObjects())
            {
                explosiveObject.Health.TakeDamage(Damage);
            }
        
            gameObject.SetActive(false);
        }
    
        protected virtual List<EnemyAlienActor> GetExplosiveObjects()
        {
            Collider[] hits = Physics.OverlapSphere(Transform.position, ExplosionRadius);

            List<EnemyAlienActor> enemies = new();

            foreach (Collider hit in hits)
                if (hit.attachedRigidbody != null && hit.gameObject.TryGetComponent(out EnemyAlienActor enemyActor))
                    enemies.Add(enemyActor);

            return enemies;
        }
    }
}