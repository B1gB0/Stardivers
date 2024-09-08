﻿using System.Collections;
using System.Collections.Generic;
using Build.Game.Scripts.ECS.EntityActors;
using UnityEngine;

namespace  Project.Scripts.Projectiles.Grenades
{
    public class FragGrenade : Projectile
    { 
        private const float ThrowTime = 2f;
        
        private ParticleSystem _explosionEffect;
        private Transform _enemyPosition;
        
        private float _damage;
        private float _explosionRadius;
        private float _grenadeSpeed;

        private void OnTriggerEnter(Collider collision)
        {
            if(collision.gameObject.TryGetComponent(out EnemyActor enemy))
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
        
        public void GetEffect(ParticleSystem effect)
        {
            _explosionEffect = effect;
        }
        
        private void Explode()
        {
            //_source.Play();
            _explosionEffect.transform.position = transform.position;
            _explosionEffect.Play();
        
            foreach (EnemyActor explosiveObject in GetExplosiveObjects())
            {
                explosiveObject.Health.TakeDamage(_damage);
            }
                
            gameObject.SetActive(false);
        }
        
        private List<EnemyActor> GetExplosiveObjects()
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, _explosionRadius);
        
            List<EnemyActor> enemies = new();
        
            foreach (Collider hit in hits)
                if (hit.attachedRigidbody != null && hit.gameObject.TryGetComponent(out EnemyActor enemyActor))
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