using Project.Scripts.ECS.EntityActors;
using UnityEngine;

namespace Project.Scripts.Levels.Triggers
{
    public class EndSecondWaveTrigger : Trigger
    {
        [SerializeField] private EnemySpawnSecondWaveTrigger _enemySpawnSecondWaveTrigger;
        
        private void OnTriggerEnter(Collider trigger)
        {
            if (trigger.TryGetComponent(out PlayerActor _))
            {
                _enemySpawnSecondWaveTrigger.CompleteSpawn();
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