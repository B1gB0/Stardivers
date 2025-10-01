using System.Collections;
using Project.Game.Scripts;
using Project.Scripts.ECS.EntityActors;
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
            if(collision.gameObject.TryGetComponent(out EnemyAlienActor enemy))
            {
                Explode();
                StopCoroutine(LifeRoutine());
            }
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
            ExplosionEffect.transform.position = Transform.position;
            ExplosionEffect.Play();
            AudioSoundsService.PlaySound(Sounds.FragGrenades);
        
            foreach (EnemyAlienActor explosiveObject in GetEnemies())
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