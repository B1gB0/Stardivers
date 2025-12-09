using System.Collections;
using Cysharp.Threading.Tasks;
using Project.Scripts.Audio.Sounds;
using Project.Scripts.ECS.EntityActors;
using Project.Scripts.ParticleEffects.Effects;
using UnityEngine;

namespace  Project.Scripts.Projectiles.Grenades
{
    public class FragGrenade : ExplodingObject
    {
        private const float ThrowTime = 2f;

        private Vector3 _enemyPosition;

        protected override void FixedUpdate()
        {
            StartCoroutine(ThrowGrenade());
            Transform.position = Vector3.MoveTowards(Transform.position, _enemyPosition, 
                ProjectileSpeed * Time.fixedDeltaTime);
        }

        protected override void OnTriggerEnter(Collider collision)
        {
            if(collision.gameObject.TryGetComponent(out EnemyActor enemy))
            {
                Explode();
                StopCoroutine(LifeRoutine());
            }
            
            CheckDefaultAndResourceLayer(collision);
        }

        public override void SetDirection(Vector3 targetPosition)
        {
            _enemyPosition = targetPosition;
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
            ParticleEffectsService.PlayEffect(ParticleEffectType.FragGrenadeExplosion, Transform.position);
            AudioSoundsService.PlaySound(SoundsType.FragGrenades).Forget();
        
            foreach (EnemyActor explosiveObject in GetEnemies())
            {
                explosiveObject.Health.TakeDamage(Damage);
            }
                
            gameObject.SetActive(false);
        }

        private IEnumerator ThrowGrenade()
        {
            Transform.up += Vector3.up;

            yield return new WaitForSeconds(ThrowTime);
        }
    }
}