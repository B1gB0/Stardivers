using System.Collections;
using System.Collections.Generic;
using Project.Scripts.Projectiles;
using UnityEngine;

namespace Project.Scripts.ECS.EntityActors
{
    public class CapsuleParts : ExplodingObject
    {
        private const float DefaultDamage = 100f;
        private const float Force = 300f;
        private const float DefaultExplodingRigidbodyRadius = 40f;
        private const float DefaultExplodingDamageRadius = 30f;

        [SerializeField] private ParticleSystem _effect;
        [SerializeField] private LayerMask _layer;

        private void Start()
        {
            Damage = DefaultDamage;
            ExplosionRadius = DefaultExplodingDamageRadius;
            
            _effect = Instantiate(_effect, transform);

            foreach (Rigidbody explodingObject in GetExplodingRigidbodyObjects())
            {
                 explodingObject.AddExplosionForce(Force, transform.position, DefaultExplodingRigidbodyRadius); 
            }

            foreach (var enemy in GetEnemies())
            {
                enemy.Health.TakeDamage(Damage);
            }
        }

        protected override IEnumerator LifeRoutine()
        {
            yield return new WaitForSeconds(LifeTime);
        
            Destroy(gameObject);
        }

        private List<Rigidbody> GetExplodingRigidbodyObjects()
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, DefaultExplodingRigidbodyRadius, _layer);

            List<Rigidbody> parts = new List<Rigidbody>();

            foreach (Collider hit in hits)
                if (hit.attachedRigidbody != null)
                    parts.Add(hit.attachedRigidbody);

            return parts;
        }
    }
}