using System.Collections;
using System.Collections.Generic;
using Build.Game.Scripts.ECS.EntityActors;
using Project.Game.Scripts;
using Project.Scripts.ECS.EntityActors;
using UnityEngine;

namespace  Project.Scripts.Projectiles.Grenades
{
    public class FragGrenade : Projectile
    {
        private const float ThrowTime = 2f;
        
        private ParticleSystem _explosionEffect;
        private AudioSoundsService _audioSoundsService;
        
        private Transform _enemyPosition;
        
        private float _damage;
        private float _explosionRadius;
        private float _grenadeSpeed;

        private void OnTriggerEnter(Collider collision)
        {
            if(collision.gameObject.TryGetComponent(out SmallAlienEnemy enemy))
            {
                Explode();
                StopCoroutine(LifeRoutine());
            }
        }

        private void FixedUpdate()
        {
            StartCoroutine(ThrowGrenade());
            transform.position = Vector3.MoveTowards(transform.position, _enemyPosition.position, 
                _grenadeSpeed * Time.fixedDeltaTime);
        }
        
        public void SetDirection(Transform enemyPosition)
        {
            _enemyPosition = enemyPosition;
        }
        
        public void SetCharacteristics(float damage, float explosionRadius, float grenadeSpeed)
        {
            _damage = damage;
            _explosionRadius = explosionRadius;
            _grenadeSpeed = grenadeSpeed;
        }
        
        public void GetExplosionEffects(ParticleSystem effect, AudioSoundsService audioSoundsService)
        {
            _explosionEffect = effect;
            _audioSoundsService = audioSoundsService;
        }

        protected override IEnumerator LifeRoutine()
        {
            yield return new WaitForSeconds(LifeTime);
            
            Explode();
        }

        private void Explode()
        {
            _explosionEffect.transform.position = transform.position;
            _explosionEffect.Play();
            _audioSoundsService.PlaySound(Sounds.FragGrenades);
        
            foreach (SmallAlienEnemy explosiveObject in GetExplosiveObjects())
            {
                explosiveObject.Health.TakeDamage(_damage);
            }
                
            gameObject.SetActive(false);
        }
        
        private List<SmallAlienEnemy> GetExplosiveObjects()
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, _explosionRadius);
        
            List<SmallAlienEnemy> enemies = new();
        
            foreach (Collider hit in hits)
                if (hit.attachedRigidbody != null && hit.gameObject.TryGetComponent(out SmallAlienEnemy enemyActor))
                    enemies.Add(enemyActor);
        
            return enemies;
        }
        
        private IEnumerator ThrowGrenade()
        {
            transform.up += Vector3.up;

            yield return new WaitForSeconds(ThrowTime);
        }
    }
}