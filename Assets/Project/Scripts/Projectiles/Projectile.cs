using System.Collections;
using Project.Scripts.ECS.EntityActors;
using UnityEngine;

namespace Project.Scripts.Projectiles
{
    public abstract class Projectile : MonoBehaviour
    {
        private const float DefaultDirectionY = 0f;
        private const string Ground = nameof(Ground);
        private const string Resources = nameof(Resources);
        private const string HighGround = nameof(HighGround);
        
        [field: SerializeField] public float LifeTime { get; private set; } = 4f;
        
        protected float Damage;
        protected float ProjectileSpeed;
    
        protected Vector3 Direction;
        protected Transform Transform;

        private void Awake()
        {
            Transform = transform;
        }

        protected virtual void OnEnable()
        {
            StartCoroutine(LifeRoutine());
        }
        
        protected virtual void FixedUpdate()
        {
            Transform.position += Direction * (ProjectileSpeed * Time.fixedDeltaTime);
        }

        protected virtual void OnTriggerEnter(Collider collision)
        {
            CheckDefaultAndResourceLayer(collision);
            
            if(collision.gameObject.TryGetComponent(out EnemyActor enemy))
            {
                enemy.Health.TakeDamage(Damage);
                gameObject.SetActive(false);
            }
        }

        protected virtual void OnDisable()
        {
            StopCoroutine(LifeRoutine());
        }
        
        public virtual void SetDirection(Vector3 targetPosition)
        {
            Direction = (targetPosition - Transform.position).normalized;
            Transform.forward = Direction;
            Direction.y = DefaultDirectionY;
            Direction = Direction.normalized;
        }

        public virtual void SetCharacteristics(float damage, float bulletSpeed)
        {
            Damage = damage;
            ProjectileSpeed = bulletSpeed;
        }

        protected void CheckDefaultAndResourceLayer(Collider collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer(Ground) 
                || collision.gameObject.layer == LayerMask.NameToLayer(Resources) 
                || collision.gameObject.layer == LayerMask.NameToLayer(HighGround))
            {
                gameObject.SetActive(false);
            }
        }

        protected virtual IEnumerator LifeRoutine()
        {
            yield return new WaitForSeconds(LifeTime);
        
            gameObject.SetActive(false);
        }
        
        
    }
}