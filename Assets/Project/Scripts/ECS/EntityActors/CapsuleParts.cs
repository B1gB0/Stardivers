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
        private const float DefaultExplodingRigidbodyRadius = 20f;
        private const float DefaultExplodingDamageRadius = 20f;
        
        private readonly Collider[] _hitsBuffer = new Collider[32];
        private readonly List<Rigidbody> _cachedRigidbodies = new ();
        
        [SerializeField] private LayerMask _layerCapsuleParts;

        private void Start()
        {
            Damage = DefaultDamage;
            ExplosionRadius = DefaultExplodingDamageRadius;

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
            _cachedRigidbodies.Clear();
    
            int hitCount = Physics.OverlapSphereNonAlloc(
                transform.position, 
                DefaultExplodingRigidbodyRadius, 
                _hitsBuffer, 
                _layerCapsuleParts
            );

            for (int i = 0; i < hitCount; i++)
            {
                if (_hitsBuffer[i].attachedRigidbody != null)
                    _cachedRigidbodies.Add(_hitsBuffer[i].attachedRigidbody);
            }

            return _cachedRigidbodies;
        }
    }
}