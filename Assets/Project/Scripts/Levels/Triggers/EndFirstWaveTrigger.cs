using Project.Scripts.ECS.EntityActors;
using UnityEngine;

namespace Project.Scripts.Levels.Triggers
{
    public class EndFirstWaveTrigger : Trigger
    {
        [SerializeField] private EnemySpawnFirstWaveTrigger _enemySpawnFirstWaveTrigger;
        
        private void OnTriggerEnter(Collider trigger)
        {
            if (trigger.TryGetComponent(out PlayerActor _))
            {
                _enemySpawnFirstWaveTrigger.CompleteSpawn();
            }
        }

        private void OnTriggerExit(Collider trigger)
        {
            if (trigger.TryGetComponent(out PlayerActor _))
            {
                Deactivate();
            }
        }
    }
}