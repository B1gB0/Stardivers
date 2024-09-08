using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Build.Game.Scripts.ECS.EntityActors
{
    public class CapsuleParts : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _effect;
        [SerializeField] private LayerMask _layer;

        private float _force = 250f;
        private float _radius = 80f;
        private float _delay = 4f;
        
        private void Start()
        {
            _effect = Instantiate(_effect, transform);

            foreach (Rigidbody explodableObject in GetExplodableObjects())
            {
                 explodableObject.AddExplosionForce(_force, transform.position, _radius); 
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
    }
}