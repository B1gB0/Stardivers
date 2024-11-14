using System.Collections.Generic;
using Project.Scripts.ECS.EntityActors;
using UnityEngine;

namespace Build.Game.Scripts.ECS.EntityActors
{
    public class CapsuleParts : MonoBehaviour
    {
        private const float Damage = 20f;
        
        [SerializeField] private ParticleSystem _effect;
        [SerializeField] private LayerMask _layer;

        private float _force = 300f;
        private float _radius = 40f;
        private float _delay = 4f;
        
        private void Start()
        {
            _effect = Instantiate(_effect, transform);

            foreach (Rigidbody explodableObject in GetExplodableObjects())
            {
                 explodableObject.AddExplosionForce(_force, transform.position, _radius); 
            }

            foreach (var enemy in GetEnemies())
            {
                enemy.Health.TakeDamage(Damage);
            }
            
            Destroy(gameObject, _delay);
        }
        
        private List<Rigidbody> GetExplodableObjects()
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, _radius, _layer);

            List<Rigidbody> parts = new List<Rigidbody>();

            foreach (Collider hit in hits)
                if (hit.attachedRigidbody != null)
                    parts.Add(hit.attachedRigidbody);

            return parts;
        }

        private List<EnemyAlienActor> GetEnemies()
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, _radius);

            List<EnemyAlienActor> enemies = new ();

            foreach (Collider hit in hits)
                if (hit.attachedRigidbody != null && hit.gameObject.TryGetComponent(out EnemyAlienActor enemyActor))
                    enemies.Add(enemyActor);

            return enemies;
        }
    }
}