using Project.Scripts.ECS.EntityActors;
using UnityEngine;
using System;

namespace Project.Scripts.Levels.Triggers
{
    public class EnemySpawnTrigger : Trigger
    {
        [SerializeField] private ParticleSystem _zoneEffect;
        
        public bool IsEnemySpawned { get; private set; }

        public event Action EnemySpawned;

        private void OnTriggerEnter(Collider trigger)
        {
            if (trigger.TryGetComponent(out PlayerActor _))
            {
                IsEnemySpawned = true;
                _zoneEffect.gameObject.SetActive(false);
                EnemySpawned?.Invoke();
            }
        }

        private void OnTriggerExit(Collider trigger)
        {
            if (trigger.TryGetComponent(out PlayerActor _))
            {
                Deactivate();
            }
        }

        public void CompleteSpawn()
        {
            IsEnemySpawned = false;
        }
    }
}