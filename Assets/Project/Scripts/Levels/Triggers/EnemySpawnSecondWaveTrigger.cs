using System;
using Project.Scripts.ECS.EntityActors;
using UnityEngine;

namespace Project.Scripts.Levels.Triggers
{
    public class EnemySpawnSecondWaveTrigger : Trigger
    {
        public bool IsEnemySpawned { get; private set; }

        public event Action EnemySpawned;

        private void OnTriggerEnter(Collider trigger)
        {
            if (trigger.TryGetComponent(out PlayerActor _))
            {
                IsEnemySpawned = true;
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