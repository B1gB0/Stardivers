using System.Collections;
using System.Collections.Generic;
using Project.Game.Scripts;
using Project.Scripts.ECS.EntityActors;
using Project.Scripts.Services;
using UnityEngine;

namespace  Project.Scripts.Projectiles.Grenades
{
    public class FragGrenade : ExplodingProjectile
    {
        private const float ThrowTime = 2f;
        
        private ParticleSystem _explosionEffect;
        private AudioSoundsService _audioSoundsService;
        
        private Transform _enemyPosition;

        protected override void FixedUpdate()
        {
            StartCoroutine(ThrowGrenade());
            Transform.position = Vector3.MoveTowards(Transform.position, _enemyPosition.position, 
                ProjectileSpeed * Time.fixedDeltaTime);
        }

        protected override void OnTriggerEnter(Collider collision)
        {
            if(collision.gameObject.TryGetComponent(out EnemyAlienActor enemy))
            {
                Explode();
                StopCoroutine(LifeRoutine());
            }
        }

        public override void SetDirection(Transform enemyPosition)
        {
            _enemyPosition = enemyPosition;
        }
        
        public void SetCharacteristics(float damage, float explosionRadius, float projectileSpeed)
        {
            Damage = damage;
            ProjectileSpeed = projectileSpeed;
            ExplosionRadius = explosionRadius;
        }

        protected override IEnumerator LifeRoutine()
        {
            yield return new WaitForSeconds(LifeTime);
            
            Explode();
        }

        protected override void Explode()
        {
            _explosionEffect.transform.position = Transform.position;
            _explosionEffect.Play();
            _audioSoundsService.PlaySound(Sounds.FragGrenades);
        
            foreach (EnemyAlienActor explosiveObject in GetExplosiveObjects())
            {
                explosiveObject.Health.TakeDamage(Damage);
            }
                
            gameObject.SetActive(false);
        }
        
        protected override List<EnemyAlienActor> GetExplosiveObjects()
        {
            Collider[] hits = Physics.OverlapSphere(Transform.position, ExplosionRadius);
        
            List<EnemyAlienActor> enemies = new();
        
            foreach (Collider hit in hits)
                if (hit.attachedRigidbody != null && hit.gameObject.TryGetComponent(out EnemyAlienActor enemyActor))
                    enemies.Add(enemyActor);
        
            return enemies;
        }
        
        private IEnumerator ThrowGrenade()
        {
            Transform.up += Vector3.up;

            yield return new WaitForSeconds(ThrowTime);
        }
    }
}