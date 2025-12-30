using System;
using Project.Scripts.ECS.EntityActors;
using UnityEngine;

namespace Project.Scripts.Levels.Triggers
{
    public class EnemySpawnTriggerWithoutEffect : Trigger
    {
        public event Action EnemySpawned;

        public bool IsEnemySpawned { get; private set; }

        private void OnTriggerEnter(Collider trigger)
        {
            if (!trigger.TryGetComponent(out PlayerActor _))
                return;

            IsEnemySpawned = true;
            EnemySpawned?.Invoke();
        }

        private void OnTriggerExit(Collider trigger)
        {
            if (trigger.TryGetComponent(out PlayerActor _))
                Deactivate();
        }

        public void CompleteSpawn()
        {
            IsEnemySpawned = false;
        }
    }
}