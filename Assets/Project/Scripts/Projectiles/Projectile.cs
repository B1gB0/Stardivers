using System;
using System.Collections;
using Project.Scripts.ECS.EntityActors;
using UnityEngine;

namespace Project.Scripts.Projectiles
{
    public abstract class Projectile : MonoBehaviour
    {
        [field: SerializeField] public float LifeTime { get; private set; } = 4f;
        
        protected float Damage;
        protected float ProjectileSpeed;
    
        protected Vector3 Direction;

        protected Transform Transform;

        private void Awake()
        {
            Transform = transform;
        }

        private void OnEnable()
        {
            StartCoroutine(LifeRoutine());
        }
        
        protected virtual void FixedUpdate()
        {
            Transform.position += Direction * (ProjectileSpeed * Time.fixedDeltaTime);
        }

        protected virtual void OnTriggerEnter(Collider collision)
        {
            if(collision.gameObject.TryGetComponent(out EnemyAlienActor enemy))
            {
                enemy.Health.TakeDamage(Damage);
                gameObject.SetActive(false);
            }
        }

        private void OnDisable()
        {
            StopCoroutine(LifeRoutine());
        }

        protected virtual IEnumerator LifeRoutine()
        {
            yield return new WaitForSeconds(LifeTime);
        
            gameObject.SetActive(false);
        }
        
        public virtual void SetDirection(Transform enemy)
        {
            Direction = (enemy.position - Transform.position).normalized;
            Transform.forward = Direction;
        }

        public virtual void SetCharacteristics(float damage, float bulletSpeed)
        {
            Damage = damage;
            ProjectileSpeed = bulletSpeed;
        }
    }
}